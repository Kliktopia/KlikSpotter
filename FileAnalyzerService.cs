using KlikSpotter.Configuration;
using SharpCompress.Readers;

namespace KlikSpotter;

internal partial class FileAnalyzerService
{
    private const int MAGIC_WORD_LENGTH = 4;
    private const int FILE_MIN_LENGTH = 1024;

    private static readonly string[] _archiveExtensions;
    private static readonly string[] _filesToLookFor;
    private static readonly string[] _filesToRemove;
    private static readonly char[] _directorySeparators;
    private static readonly char[] _illegalCharacters;
    private static readonly byte[] _installMakerPattern;
    private static readonly byte[] _magicWordBuffer;
    private static readonly IReadOnlyDictionary<string, byte[]> _searchPatterns;

    private readonly AppParameters _parameters;
    private readonly ILogger _logger;

    public FileAnalyzerService(AppParameters parameters)
    {
        (_parameters, _logger) = GetProviders(parameters);

        if (_parameters.DoShowVersion)
            WriteVersionToConsole();

        if (_parameters.DoDebug)
        {
            Parser.SaveSearchPatternsToJson(_searchPatterns);
            var y = Parser.LoadSearchPatternsFromJson(@"C:\Temp\junk.txt");
        }
    }

    public void ProcessFiles()
    {
        int gameCount = 0;
        int installerCount = 0;
        foreach (string? filePath in Directory.GetFiles(
            _parameters.SearchDirectory, "*.*",
            _parameters.DoRecursiveSearch ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
            .Where(IsFileTypeToLookFor))
        {
            string? directoryName = Path.GetDirectoryName(filePath),
                    fileName = Path.GetFileName(filePath),
                    fileExtension = Path.GetExtension(filePath);

            if (directoryName is null)
                continue;

            if (!Win32Helpers.TryGetPathRelativePath(_parameters.SearchDirectory, directoryName, out string? relativePath)
                || relativePath.Split(_directorySeparators, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault()?.StartsWith('!') == true)
                continue;

            byte[] fileData;

            try
            {
                fileData = File.ReadAllBytes(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Could not read File! ({exceptionType})", ex.GetType().Name);
#if DEBUG
                _logger.LogError("Exception: {exception}", ex);
#endif
                return;
            }

            if (fileData.Length < FILE_MIN_LENGTH)
            {
                _logger.LogInformation("'{file}' is too small to be a Klik application...", Path.Combine(relativePath, fileName));
                continue;
            }

            if (fileExtension.Equals(".exe", StringComparison.OrdinalIgnoreCase))
            {
                long offset = SearchForPattern(fileData, _installMakerPattern);
                bool found = false;

                if (offset != -1)
                {
                    string path = _parameters.OutputDirectory;

                    if (fileData[0] == 0 && fileData[1] == 0 && fileData[2] == 0 && fileData[3] == 0)
                    {
                        WriteIsMatchToLogger(Path.Combine(relativePath, fileName), "Install Creator Installer", installerCount++);
                        path = Path.Combine(path, "!IC_INSTALLERS", relativePath);
                    }
                    else
                    {
                        WriteIsMatchToLogger(Path.Combine(relativePath, fileName), "Install Maker Installer", installerCount++);
                        path = Path.Combine(path, "!IM_INSTALLERS", relativePath);
                    }

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    if (_parameters.DoCopy)
                        File.WriteAllBytes(Path.Combine(path, fileName), fileData);

                    MoveFileToFound(filePath, fileName, relativePath);

                    found = true;
                }
                else
                {
                    foreach ((string? key, byte[]? pattern) in _searchPatterns)
                    {
                        offset = SearchForPattern(fileData, pattern);
                        if (offset == -1)
                            continue;

                        WriteIsMatchToLogger(Path.Combine(relativePath, fileName), "Klik application", key, ++gameCount);

                        string gamesDirectory = Path.Combine(_parameters.OutputDirectory, "!GAMES", relativePath);

                        if (!Directory.Exists(gamesDirectory))
                            Directory.CreateDirectory(gamesDirectory);

                        string fileNameWithoutFileExtension = fileName[..^fileExtension.Length];

                        if (_parameters.DoCopy)
                        {
                            foreach (string fileWithSameName in Directory.GetFiles(directoryName, $"{fileNameWithoutFileExtension}.*"))
                                File.Copy(fileWithSameName, Path.Combine(gamesDirectory, $"{fileNameWithoutFileExtension}{Path.GetExtension(fileWithSameName)}"), overwrite: true);
                        }

                        string[] filesIsDirectory = Directory.GetFiles(directoryName, "*.txt");
                        if (filesIsDirectory.Length == 1 && _parameters.DoCopy)
                            File.Copy(filesIsDirectory[0], Path.Combine(gamesDirectory, Path.GetFileName(filesIsDirectory[0])), overwrite: true);

                        MoveFileToFound(filePath, fileName, relativePath);

                        found = true;
                        break;
                    }
                }

                if (found) continue;
            }

            try
            {
                using var fileStream = new MemoryStream(fileData);
                using (var archiveReader = ReaderFactory.Open(fileStream, new() { LeaveStreamOpen = true }))
                {
                    _logger.LogInformation("Analyzing Contents of '{file}'...", Path.Combine(relativePath, fileName));

                    //bool doWriteGame = false;
                    //bool doMoveGameToFound = false;

                    string appType = "Klik application";

                    while (archiveReader.MoveToNextEntry())
                    {
                        var entry = archiveReader.Entry;

                        if (entry.IsDirectory)
                            continue;

                        string? entryKey = entry.Key;
                        if (entryKey is null)
                            continue;

                        string? entryFilename = entryKey.Split(_directorySeparators).LastOrDefault();
                        if (entryFilename is null)
                            continue;

                        int startIndex = entryFilename.LastIndexOf('.');
                        if (startIndex < 1)
                            continue;

                        MagicWord? matchingMagicWord = null;

                        switch (entryFilename[startIndex..].ToLowerInvariant())
                        {
                            case ".mfa":
                                appType = "Multimedia Fusion 2 application";
                                GetMatchingMagicWord(archiveReader, MagicWords.MultiMediaFusion, out matchingMagicWord);
                                break;
                            case ".ccn" or ".hts":
                                appType = "Vitalize application";
                                GetMatchingMagicWord(archiveReader, MagicWords.Vitalize, out matchingMagicWord);
                                break;
                            case ".cca":
                                appType = "Click & Create application";
                                GetMatchingMagicWord(archiveReader, MagicWords.ClickAndCreate, out matchingMagicWord);
                                break;
                            case ".gam":
                                appType = "The Games Factory application";
                                GetMatchingMagicWord(archiveReader, MagicWords.TheGamesFactory, out matchingMagicWord);
                                break;
                        }

                        string? match = matchingMagicWord.HasValue ? matchingMagicWord.Value.ToString() : null;

                        if (match is null)
                        {
                            foreach (string fileToLookFor in _filesToLookFor)
                            {
                                if (string.Equals(entryFilename, fileToLookFor, StringComparison.OrdinalIgnoreCase))
                                {
                                    match = fileToLookFor;
                                    break;
                                }
                            }
                        }

                        if (match is null) continue;

                        WriteIsMatchToLogger(Path.Combine(relativePath, fileName), appType, match, gameCount++);
                        WriteGame(relativePath, fileName, fileData, "!GAMES");

                        break;
                    }
                }

                // Rewind the stream so we don't snub up the archive reader
                fileStream.Seek(0, SeekOrigin.Begin);

                using (var archiveReader = ReaderFactory.Open(fileStream, new() { LeaveStreamOpen = true }))
                {
                    // This loop is looking for installers and games in .exe files
                    while (archiveReader.MoveToNextEntry())
                    {
                        string? exeFileName = archiveReader.Entry.Key;
                        if (exeFileName?.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) != true)
                            continue;

                        _logger.LogInformation("Analyzing '{file}'...", exeFileName);

                        int size = (int)archiveReader.Entry.Size;
                        byte[] unpackedData = ArrayPool<byte>.Shared.Rent(size);

                        try
                        {
                            using var entryStream = archiveReader.OpenEntryStream();
                            entryStream.ReadAtLeast(unpackedData, size);

                            long offset;

                            // Check if the file is an Install Maker or Install Creator Installer
                            offset = SearchForPattern(unpackedData, _installMakerPattern);

                            if (offset != -1)
                            {
                                if (SectionIsBlank(unpackedData, offset + 10, 4))
                                {
                                    _logger.LogInformation("'{file}' is an Install Maker Installer! ({count:N0} found)", Path.Combine(relativePath, fileName), ++installerCount);
                                    WriteGame(relativePath, fileName, fileData, "!IM_INSTALLERS");
                                }
                                else
                                {
                                    _logger.LogInformation("'{file}' is an Install Creator Installer! ({count:N0} found)", Path.Combine(relativePath, fileName), ++installerCount);
                                    WriteGame(relativePath, fileName, fileData, "!IC_INSTALLERS");
                                }

                                break;
                            }

                            // Check if the file is a Klik & Play, Click & Create or The Games Factory Stand Alone Installer
                            offset = SearchForPattern(unpackedData, Encoding.ASCII.GetBytes("Klik & Play Stand Alone Installer"));

                            if (offset != -1)
                            {
                                _logger.LogInformation("'{file}' is a Klik & Play Stand Alone Installer! ({count:N0} found)", Path.Combine(relativePath, fileName), ++installerCount);
                                WriteGame(relativePath, fileName, fileData, "!KNP_INSTALLERS");

                                break;
                            }

                            // Check if the file is a Click & Create Stand Alone Installer
                            offset = SearchForPattern(unpackedData, Encoding.ASCII.GetBytes("C&C Stand Alone App Installer"));

                            if (offset != -1)
                            {
                                _logger.LogInformation("'{file}' is a Click & Create Stand Alone Installer! ({count:N0} found)", Path.Combine(relativePath, fileName), ++installerCount);
                                WriteGame(relativePath, fileName, fileData, "!CNC_INSTALLERS");

                                break;
                            }

                            // Check if the file is a The Games Factory Stand Alone Installer
                            offset = SearchForPattern(unpackedData, Encoding.ASCII.GetBytes("GFSAS"));

                            if (offset != -1)
                            {
                                // Must match with GFSA\0\0 or GFSA32.EXE
                                var slice = unpackedData.AsSpan().Slice((int)offset + 5, 6);

                                if (slice[0] == 0 && slice[1] == 0 ||
                                    slice[0] == '3' &&
                                    slice[1] == '2' &&
                                    slice[2] == '.' &&
                                    slice[3] == 'E' &&
                                    slice[4] == 'X' &&
                                    slice[5] == 'E')
                                {
                                    _logger.LogInformation("'{file}' is a The Games Factory Stand Alone Installer! ({count:N0} found)", Path.Combine(relativePath, fileName), ++installerCount);
                                    WriteGame(relativePath, fileName, fileData, "!TGF_INSTALLERS");

                                    break;
                                }
                            }

                            // Perhaps it's a Game?
                            foreach ((string? key, byte[]? pattern) in _searchPatterns)
                            {
                                offset = SearchForPattern(unpackedData, pattern);
                                if (offset == -1)
                                    continue;

                                WriteIsMatchToLogger(Path.Combine(relativePath, fileName), "Klik application", key, gameCount++);
                                WriteGame(relativePath, fileName, fileData, "!GAMES");

                                break;
                            }

                            // We found nothing, sir. Move along to the next file.
                        }
                        finally
                        {
                            ArrayPool<byte>.Shared.Return(unpackedData);
                        }
                    }
                }

                //if (doWriteGame)
                //    WriteGame(relativePath, fileName, fileData, "!GAMES");

                //if (doWriteGame || doMoveGameToFound)
                //{
                //    MoveFileToFound(filePath, fileName, relativePath);
                //    break;
                //}

                static MagicWord? GetMatchingMagicWord(IReader zipFile, MagicWord[] magicWords, out MagicWord? magicWord)
                    => TryGetMagicWord(zipFile, out magicWord) && magicWords.Contains(magicWord.Value)
                        ? magicWord
                        : null;

                static bool TryGetMagicWord(IReader zipFile, [NotNullWhen(true)] out MagicWord? magicWord)
                {
                    using var zipStream = zipFile.OpenEntryStream();
                    if (zipStream.ReadAtLeast(_magicWordBuffer, MAGIC_WORD_LENGTH, false) < MAGIC_WORD_LENGTH)
                    {
                        magicWord = null;
                        return false;
                    }

                    magicWord = _magicWordBuffer;
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (fileExtension.Equals(".exe", StringComparison.OrdinalIgnoreCase))
                {
                    WriteIsNotMatchToLogger(Path.Combine(relativePath, fileName));
                    continue;
                }

                _logger.LogError("Could not extract '{file}'! ({exception})", Path.Combine(relativePath, fileName), ex.GetType().Name);
#if DEBUG
                _logger.LogError("Exception: {exception}", ex);
#endif

                string problematicDirectory = Path.Combine(_parameters.OutputDirectory, "!PROBLEMATIC", relativePath);
                if (!Directory.Exists(problematicDirectory))
                    Directory.CreateDirectory(problematicDirectory);

                if (_parameters.DoCopy)
                    File.WriteAllBytes(Path.Combine(problematicDirectory, fileName), fileData);
            }
        }

        WriteResultToConsole(gameCount, installerCount);
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
                _logger.LogCritical("Directory '{path}' does not exists!", path);
                return false;
            }

            Directory.CreateDirectory(path);
        }

        return true;
    }

    private void MoveFileToFound(string file, string fileName, string relativeDir)
    {
        if (!_parameters.DoMoveOriginal)
            return;

        string movedDirectory = Path.Combine(_parameters.OutputDirectory, "!MOVED", relativeDir);
        if (!Directory.Exists(movedDirectory))
            Directory.CreateDirectory(movedDirectory);

        File.Move(file, Path.Combine(movedDirectory, fileName));
    }

    public void WriteGame(string relativeDir, string fileName, byte[] fileData, string? prefix = null)
    {
        string gameDirectory = Path.Combine(_parameters.OutputDirectory, prefix ?? ".", relativeDir, fileName.Replace('.', '_'));

        if (!Directory.Exists(gameDirectory))
            Directory.CreateDirectory(gameDirectory);

        IReader? archiveReader = null;

        try
        {
            var entryPaths = new Dictionary<int, List<string>>();
            int entryIndex = 0;

            using var fileStream = new MemoryStream(fileData);
            archiveReader = ReaderFactory.Open(fileStream, new() { LeaveStreamOpen = true });

            while (archiveReader.MoveToNextEntry())
            {
                entryIndex++;
                var entry = archiveReader.Entry;

                if (!entry.IsDirectory
                    && entry.Key is not null
                    && !_filesToRemove.Any(ftr => string.Equals(GetFileName(entry.Key), ftr, StringComparison.OrdinalIgnoreCase)))
                    entryPaths.Add(entryIndex, [.. entry.Key.Split(_directorySeparators)]);
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
                    _logger.LogError("Could not find Entry at Index {entryIndex} ({fileName})!", entryIndex, pathParts[^1]);
                    break;
                }

                string fullPath = $"{gameDirectory}\\{string.Join('\\', pathParts.Select(f => string.Join('_', f.Split(_illegalCharacters))))}";
                string directoryName = Path.GetDirectoryName(fullPath)!;

                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);

                archiveReader.WriteEntryTo(fullPath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not extract Game! ({exceptionType})", ex.GetType().Name);
#if DEBUG
            _logger.LogError("Exception: {exception}", ex);
#endif
        }
        finally
        {
            archiveReader?.Dispose();
        }

        static string GetFileName(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            int lastSlashIndex = path.LastIndexOfAny(_directorySeparators);
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

    private static void WriteToConsole(string line, AnsiConsoleColor? color)
    {
        if (!color.HasValue || !Win32Helpers.HasAnsiColors)
        {
            Console.Write(line);
            return;
        }

        Console.Write($"{color.Value}{line}{AnsiConsoleColor.Reset}");
    }

    [DoesNotReturn]
    public void WriteVersionToConsole()
    {
        var versionInfo = FileVersionInfo.GetVersionInfo(_parameters.ProcessPath);

        foreach (var (line, color) in GetLogo(versionInfo))
            WriteToConsole(line, color);

        Environment.Exit(0);
    }

    private static string CleanUpPath(string path)
    {
        if (path.StartsWith(".\\"))
            path = path[2..];

        return path.TrimEnd(_directorySeparators);
    }

    private void WriteIsMatchToLogger(string path, string type, int count)
    {
        path = CleanUpPath(path);
        _logger.LogInformation("\x1b[38;5;22m[{count,4}] '{path}' is a {type}!", count, path, type);
    }

    private void WriteIsMatchToLogger(string path, string type, string pattern, int count)
    {
        path = CleanUpPath(path);
        _logger.LogInformation("\x1b[38;5;13m[{count,4}] '{path}' is a {type}! (Match: '{pattern}')", count, path, type, pattern);
    }

    private void WriteIsNotMatchToLogger(string path)
    {
        path = CleanUpPath(path);
        _logger.LogInformation("'{file}' is not a Klik application...", path);
    }

    private void WriteResultToConsole(int gameCount, int installerCount)
    {
        _logger.LogInformation("{gamesCount:N0} Game(s) and {installersCount:N0} Installer(s) found.\n", gameCount, installerCount);
        _logger.LogInformation("{processName} ran to Completion.", Path.GetFileNameWithoutExtension(_parameters.ProcessPath));
    }

    private static long SearchForPattern(ReadOnlySpan<byte> data, ReadOnlySpan<byte> pattern)
    {
        for (int i = 0; i <= data.Length - pattern.Length; i++)
        {
            if (data.Slice(i, pattern.Length).SequenceEqual(pattern))
                return i;
        }

        return -1;
    }
}
