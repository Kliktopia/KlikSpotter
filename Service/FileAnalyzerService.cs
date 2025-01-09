namespace KlikSpotter.Service;

internal partial class FileAnalyzerService
{
    private const int MAGIC_WORD_LENGTH = 4;
    private const int FILE_MIN_SIZE = 1024;

    private readonly AppParameters _parameters;
    private readonly AppConfiguration _configuration;
    private readonly ILogger _logger;

    public FileAnalyzerService(AppParameters parameters) 
        => (_parameters, _configuration, _logger) = Initialize(parameters);

    public void ProcessFiles()
    {
        int gameCount = 0;
        int installerCount = 0;

        _logger.LogInformation("Scanning '{colYellow}{directory}{colReset}' for Klik apps...\n", 
            AnsiConsoleColor.BrightYellow, 
            _parameters.SearchDirectory, 
            AnsiConsoleColor.Reset);

        foreach (var file in GetFileCandidates())
        {
            if (file.DataLength < FILE_MIN_SIZE)
            {
                _logger.LogInformation("'{colYellow}{file}{colReset}' is much too small to be a Klik app...", 
                    AnsiConsoleColor.BrightYellow, 
                    file.RelativeFilePath, 
                    AnsiConsoleColor.Reset);
                continue;
            }

            bool foundSomething = false;

            if (file.IsArchive != true)
            {
                foreach (var pattern in _configuration.SearchPatterns)
                {
                    if (pattern.FileExtensions?.Count() > 0 && pattern.FileExtensions.All(ext
                        => !string.Equals(ext, file.Extension, StringComparison.OrdinalIgnoreCase)))
                        continue;

                    _logger.LogInformation("Analyzing '{colYellow}{file}{colReset}'. Looking for '{colYellow}{pattern}{colReset}'...", 
                        AnsiConsoleColor.BrightYellow, 
                        CleanUpPath(file.RelativeFilePath), 
                        AnsiConsoleColor.Reset,
                        AnsiConsoleColor.BrightYellow,
                        pattern.Alias,
                        AnsiConsoleColor.Reset);
                    long offset = SearchForPattern(file.Data, pattern.Data);
                    if (offset == -1)
                        continue;

                    foundSomething = true;

                    WriteIsMatchToLogger(file.RelativeFilePath, pattern.Indicators?.FirstOrDefault().ToString() ?? "Klik app", pattern.Alias, ++gameCount);

                    string gamesDirectory = Path.Combine(_parameters.OutputDirectory, _configuration.GetDirectoryPath(DirectoryType.Indeterminate), file.RelativeDirectoryPath);

                    if (!Directory.Exists(gamesDirectory))
                        Directory.CreateDirectory(gamesDirectory);

                    string fileNameWithoutFileExtension = file.Name[..^file.Extension.Length];

                    if (_parameters.DoCopy)
                    {
                        foreach (string fileWithSameName in Directory.GetFiles(file.DirectoryName, $"{fileNameWithoutFileExtension}.*"))
                            File.Copy(fileWithSameName, Path.Combine(gamesDirectory, $"{fileNameWithoutFileExtension}{Path.GetExtension(fileWithSameName)}"), overwrite: true);
                    }

                    string[] filesInDirectory = Directory.GetFiles(file.DirectoryName, "*.txt");
                    if (filesInDirectory.Length == 1 && _parameters.DoCopy)
                        File.Copy(filesInDirectory[0], Path.Combine(gamesDirectory, Path.GetFileName(filesInDirectory[0])), overwrite: true);

                    MoveFileToFound(file);

                    break;
                }

                if (foundSomething)
                    continue;
            }
            else
            {
                try
                {
                    using var archiveStream = new MemoryStream(file.Data);
                    using var archiveReader = ReaderFactory.Open(archiveStream);

                    while (archiveReader.MoveToNextEntry())
                    {
                        var entry = archiveReader.Entry;

                        if (entry.IsDirectory || entry.Size < FILE_MIN_SIZE)
                            continue;

                        string? entryKey = entry.Key;
                        if (entryKey is null)
                            continue;

                        string? entryFilename = entryKey.Split(FileHelper.DirectorySeparators).LastOrDefault();
                        if (string.IsNullOrEmpty(entryFilename))
                            continue;

                        if (_configuration.FilesThatIndicatesMatch.Any(ftr => string.Equals(entryFilename, ftr, StringComparison.OrdinalIgnoreCase)))
                            foundSomething = true;

                        using var entryStream = archiveReader.OpenEntryStream();

                        byte[] entryData = new byte[entry.Size];
                        entryStream.Write(entryData, 0, entryData.Length);

                        foreach (var pattern in _configuration.SearchPatterns)
                        {
                            if (pattern.FileExtensions?.Count() > 0 && pattern.FileExtensions.All(ext
                                => !string.Equals(ext, file.Extension, StringComparison.OrdinalIgnoreCase)))
                                continue;

                            _logger.LogInformation("Analyzing '{colYellow}{archive}{colReset}' -> '{colYellow}{file}{colReset}'. Looking for '{pattern}'...", 
                                AnsiConsoleColor.BrightYellow, 
                                CleanUpPath(file.RelativeFilePath), 
                                AnsiConsoleColor.Reset, 
                                AnsiConsoleColor.BrightYellow, 
                                entryKey, 
                                AnsiConsoleColor.Reset, 
                                pattern.Alias);

                            long offset = SearchForPattern(file.Data, pattern.Data);
                            if (offset == -1) continue;

                            foundSomething = true;

                            WriteIsMatchToLogger(file.RelativeFilePath, pattern.Indicators?.FirstOrDefault().ToString() ?? "Klik app", pattern.Alias, ++gameCount);
                            break;
                        }

                        if (foundSomething)
                            break;
                    }

                    if (foundSomething)
                        continue;
                }
                catch
                {
                    _logger.LogError("Could not extract '{colYellow}{file}{colReset}'!", 
                        AnsiConsoleColor.BrightYellow, 
                        file.RelativeFilePath,
                        AnsiConsoleColor.Reset);

                    string problematicDirectory = Path.Combine(_parameters.OutputDirectory, _configuration.GetDirectoryPath(DirectoryType.Failed), file.RelativeDirectoryPath);

                    if (!Directory.Exists(problematicDirectory))
                        Directory.CreateDirectory(problematicDirectory);

                    if (_parameters.DoCopy)
                        File.WriteAllBytes(Path.Combine(problematicDirectory, file.Name), file.Data);

                    continue;
                }
            }
            if (!foundSomething)
            {
                WriteIsNotMatchToLogger(file.RelativeFilePath);
                continue;
            }
        }

        WriteResultToConsole(gameCount, installerCount);
    }

    internal class FileCandidate(
        string filePath,
        string directoryName,
        string fileName,
        string fileExtension,
        string relativeFilePath,
        string relativeDirectoryPath,
        bool? isArchive)
    {
        private byte[]? _data;

        public readonly string FilePath = filePath;
        public readonly string DirectoryName = directoryName;
        public readonly string Name = fileName;
        public readonly string Extension = fileExtension;
        public readonly string RelativeFilePath = relativeFilePath;
        public readonly string RelativeDirectoryPath = relativeDirectoryPath;
        public readonly bool? IsArchive = isArchive;

        public byte[] Data => TryReadData(out _data) ? _data : [];
        public int DataLength { get; private set; } = -1;
        public bool IsRead { get; private set; } = false;

        public bool TryReadData([NotNullWhen(true)] out byte[]? data)
        {
            try
            {
                if (IsRead)
                {
                    data = _data!;
                    return true;
                }

                _data = File.ReadAllBytes(FilePath);
                DataLength = _data.Length;
                IsRead = true;

                data = _data;
            }
            catch
            {
                data = null;
            }

            return IsRead;
        }
    }

    private bool TryGetFileCandidate(string filePath, [NotNullWhen(true)] out FileCandidate? fileCandidate)
    {
        if (!Win32Helper.IsValidPath(filePath))
        {
            fileCandidate = null;
            return false;
        }

        string? directoryName = Path.GetDirectoryName(filePath);

        if (directoryName is null)
        {
            fileCandidate = null;
            return false;
        }

        string fileName = Path.GetFileName(filePath);
        string fileExtension = Path.GetExtension(filePath);

        if (!Win32Helper.TryGetPathRelativePath(_parameters.SearchDirectory, directoryName, out string? relativeDirectoryPath)
            || _configuration.OutputDirectories.Any(dir => dir.Path.Equals(relativeDirectoryPath.Split(FileHelper.DirectorySeparators, StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault(), StringComparison.OrdinalIgnoreCase)))
        {
            fileCandidate = null;
            return false;
        }

        string relativeFilePath = Path.Combine(relativeDirectoryPath, fileName);
        bool? isArchive = _configuration.FileExtensions.Single(ext
            => ext.Extension.Equals(fileExtension, StringComparison.OrdinalIgnoreCase)).IsArchive;

        fileCandidate = new
        (
            filePath,
            directoryName,
            fileName,
            fileExtension,
            relativeFilePath,
            relativeDirectoryPath,
            isArchive
        );

        return true;
    }

    private IEnumerable<FileCandidate> GetFileCandidates()
    {
        foreach (string? filePath in Directory.GetFiles(
            _parameters.SearchDirectory, "*.*",
            _parameters.DoRecursiveSearch ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
            .Where(path => _configuration.FileExtensions.Any(ext => ext.Extension.Equals(Path.GetExtension(path), StringComparison.OrdinalIgnoreCase)))
            .OrderBy(path => path, new FilePathComparer()))
        {
            string? directoryName = Path.GetDirectoryName(filePath);

            if (string.IsNullOrEmpty(directoryName) ||
                !Win32Helper.TryGetPathRelativePath(_parameters.SearchDirectory, directoryName, out string? relativePath))
            {
                _logger.LogError("Invalid file path '{colYellow}{file}{colReset}'!", 
                    AnsiConsoleColor.BrightYellow, 
                    AnsiConsoleColor.Reset, 
                    filePath);
                continue;
            }

            if (_configuration.OutputDirectories.Any(dir => dir.Path.Equals(relativePath.Split(FileHelper.DirectorySeparators, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault(), StringComparison.OrdinalIgnoreCase)))
                continue;

            if (!TryGetFileCandidate(filePath, out FileCandidate? file))
            {
                _logger.LogError("Could not find '{colYellow}{file}{colReset}'!", 
                    AnsiConsoleColor.BrightYellow, 
                    filePath, 
                    AnsiConsoleColor.Reset);
                continue;
            }

            if (!file.TryReadData(out _))
            {
                _logger.LogError("Could not read '{colYellow}{file}{colReset}'!", 
                    AnsiConsoleColor.BrightYellow, 
                    file.RelativeFilePath,
                    AnsiConsoleColor.Reset);
                continue;
            }

            yield return file;
        }
    }

    private static bool SectionIsBlank(ReadOnlySpan<byte> data, long offset, int length)
    {
        for (int i = (int)offset; i < offset + length; i++)
        {
            if (data[i] != 0)
                return false;
        }

        return true;
    }

    private bool TrySanitizeTargetDirectory([NotNullWhen(true)] ref string path, bool createIfNotExist)
    {
        if (!Path.IsPathRooted(path))
            path = Path.Combine(_parameters.ProcessPath, path);

        if (!Directory.Exists(path))
        {
            if (!createIfNotExist)
            {
                _logger.LogCritical("Directory '{colYellow}{path}{colReset}' does not exists!", 
                    AnsiConsoleColor.BrightYellow, 
                    path,
                    AnsiConsoleColor.Reset);

                return false;
            }

            Directory.CreateDirectory(path);
        }

        return true;
    }

    private void MoveFileToFound(FileCandidate file)
    {
        if (!_parameters.DoMoveOriginal)
            return;

        string movedDirectory = Path.Combine(_parameters.OutputDirectory, _configuration.GetDirectoryPath(DirectoryType.Moved), file.RelativeDirectoryPath);
        if (!Directory.Exists(movedDirectory))
            Directory.CreateDirectory(movedDirectory);

        File.Move(file.FilePath, Path.Combine(movedDirectory, file.Name));
    }

    public void ExtractArchive(FileCandidate file, string? prefix = null)
    {
        string gameDirectory = Path.Combine(_parameters.OutputDirectory, prefix ?? ".", file.RelativeDirectoryPath, file.Name.Replace('.', '_'));

        if (!Directory.Exists(gameDirectory))
            Directory.CreateDirectory(gameDirectory);

        IReader? archiveReader = null;

        try
        {
            var entryPaths = new Dictionary<int, List<string>>();
            int entryIndex = 0;

            using var fileStream = new MemoryStream(file.Data);
            archiveReader = ReaderFactory.Open(fileStream, new() { LeaveStreamOpen = true });

            while (archiveReader.MoveToNextEntry())
            {
                entryIndex++;
                var entry = archiveReader.Entry;

                if (!entry.IsDirectory
                    && entry.Key is not null
                    && !_configuration.FilesToRemove.Any(ftr => string.Equals(GetFileName(entry.Key), ftr, StringComparison.OrdinalIgnoreCase)))
                    entryPaths.Add(entryIndex, [.. entry.Key.Split(FileHelper.DirectorySeparators)]);
            }

            while (!entryPaths.Any(path => path.Value.Count <= 1))
            {
                // Check if all paths have the same string at the first index
                string? firstString = null;
                bool allSame = true;

                foreach (List<string> paths in entryPaths.Values)
                {
                    if (paths.Count == 0)
                        continue;

                    if (firstString is null)
                    {
                        firstString = paths[0];
                    }
                    else if (paths[0] != firstString)
                    {
                        allSame = false;
                        break;
                    }
                }

                if (!allSame) break;

                int[] keys = [.. entryPaths.Keys];
                foreach (int key in keys)
                    entryPaths[key].RemoveAt(0);
            }

            archiveReader.Dispose();
            fileStream.Seek(0, SeekOrigin.Begin);
            archiveReader = ReaderFactory.Open(fileStream, new() { LeaveStreamOpen = true });
            entryIndex = 0;

            foreach ((int i, List<string> pathParts) in entryPaths)
            {
                bool found = false;

                while (archiveReader.MoveToNextEntry())
                {
                    if (++entryIndex >= i)
                    {
                        entryIndex = i;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    _logger.LogError("Could not find Entry at index {colYellow}{index}{colReset} ({colYellow}{file}{colReset})!", 
                        AnsiConsoleColor.BrightYellow, 
                        entryIndex,
                        AnsiConsoleColor.Reset, 
                        AnsiConsoleColor.BrightYellow, 
                        pathParts[^1],
                        AnsiConsoleColor.Reset);
                    break;
                }

                string fullPath = $"{gameDirectory}\\{string.Join('\\', pathParts.Select(f => string.Join('_', f.Split(FileHelper.IllegalCharacters))))}";
                string directoryName = Path.GetDirectoryName(fullPath)!;

                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);

                archiveReader.WriteEntryTo(fullPath);
            }
        }
        catch
        {
            _logger.LogError("Could not extract archive!");
        }
        finally
        {
            archiveReader?.Dispose();
        }

        static string GetFileName(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            int lastSlashIndex = path.LastIndexOfAny(FileHelper.DirectorySeparators);
            return lastSlashIndex == -1 ? path : path[(lastSlashIndex + 1)..];
        }
    }

    private static bool StringArrayContainsAny(string[] array, params string[] values)
    {
        for (int i = 0; i < array.Length; i++)
        {
            for (int j = 0; j < values.Length; j++)
            {
                if (array[i].Equals(values[j], StringComparison.OrdinalIgnoreCase))
                    return true;
            }
        }

        return false;
    }

    private static string CleanUpPath(string path)
    {
        if (path.StartsWith(".\\"))
            path = path[2..];

        return path.TrimEnd(FileHelper.DirectorySeparators);
    }

    private void WriteIsMatchToLogger(string path, string type, string pattern, int count)
    {
        path = CleanUpPath(path);
        _logger.LogInformation("'{colGreen}{path}{colReset}' is a {colGreen}{type} app{colReset}! (Match: '{colGreen}{pattern}{colReset}')", 
            AnsiConsoleColor.BrightGreen, 
            path,
            AnsiConsoleColor.Reset, 
            AnsiConsoleColor.BrightGreen, 
            type,
            AnsiConsoleColor.Reset, 
            AnsiConsoleColor.BrightGreen, 
            pattern,
            AnsiConsoleColor.Reset);
    }

    private void WriteIsNotMatchToLogger(string path)
    {
        path = CleanUpPath(path);
        _logger.LogInformation("'{colYellow}{path}{colReset}' is not a Klik app...", 
            AnsiConsoleColor.BrightYellow, 
            path,
            AnsiConsoleColor.Reset);
    }

    private void WriteResultToConsole(int gameCount, int installerCount)
    {
        _logger.LogInformation("{count:N0} game(s) and {installersCount:N0} installer(s) found.\n", gameCount, installerCount);
        _logger.LogInformation("{process} ran to completion.", Path.GetFileNameWithoutExtension(_parameters.ProcessPath));
    }

    private static long SearchForPattern(ReadOnlySpan<byte> data, ReadOnlySpan<byte> pattern, int? exactPosition = null)
    {
        if (data.Length < pattern.Length)
            return -1;

        for (int position = exactPosition ?? 0; position <= data.Length - pattern.Length; position++)
        {
            if (data.Slice(position, pattern.Length).SequenceEqual(pattern))
                return position;
            else if (exactPosition.HasValue)
                break;
        }

        return -1;
    }
}
