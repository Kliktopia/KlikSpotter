namespace KlikSpotter.Configuration;

internal class AppConfiguration
{
    private IOrderedEnumerable<SearchPattern>? _searchPatterns;
    private IOrderedEnumerable<NamedDirectory>? _outputDirectories;
    private IOrderedEnumerable<FileExtension>? _fileExtensions;
    private IOrderedEnumerable<string>? _filesThatIndicatesMatch;
    private IOrderedEnumerable<string>? _filesToRemove;

    public IEnumerable<FileExtension> FileExtensions
    {
        get => _fileExtensions
            ?? (IOrderedEnumerable<FileExtension>)Enumerable.Empty<FileExtension>();

        init => _fileExtensions = (value ?? [])
            .OrderBy(pattern => pattern.Extension, StringComparer.OrdinalIgnoreCase);
    }

    public IEnumerable<string> FilesThatIndicatesMatch
    {
        get => _filesThatIndicatesMatch
            ?? (IOrderedEnumerable<string>)Enumerable.Empty<string>();

        init => _filesThatIndicatesMatch = (value ?? [])
            .OrderBy(pattern => pattern, StringComparer.OrdinalIgnoreCase);
    }

    public IEnumerable<string> FilesToRemove
    {
        get => _filesToRemove
            ?? (IOrderedEnumerable<string>)Enumerable.Empty<string>();

        init => _filesToRemove = (value ?? [])
            .OrderBy(pattern => pattern, StringComparer.OrdinalIgnoreCase);
    }

    public IEnumerable<SearchPattern> SearchPatterns
    {
        get => _searchPatterns
            ?? (IOrderedEnumerable<SearchPattern>)Enumerable.Empty<SearchPattern>();

        init => _searchPatterns = (value ?? [])
            .OrderBy(pattern => pattern, new SearchPatternComparer());
    }

    public IEnumerable<NamedDirectory> OutputDirectories
    {
        get => _outputDirectories
            ?? (IOrderedEnumerable<NamedDirectory>)Enumerable.Empty<NamedDirectory>();

        init => _outputDirectories = (value ?? [])
            .UnionBy(DefaultConfiguration.DirectoryNames, dir => dir.Type)
            .OrderBy(directory => (int)directory.Type);
    }

    public string GetDirectoryPath(DirectoryType type)
    {
        NamedDirectory directory = OutputDirectories.Single(directory => directory.Type == type);

        return Path.IsPathRooted(directory.Path)
            ? directory.Path
            : Path.Combine(Directory.GetCurrentDirectory(), directory.Path);
    }

    public void Save(string filePath)
    {
        ReadOnlySpan<byte> json = JsonSerializer.SerializeToUtf8Bytes(this, ConfigurationSerializerContext.Default.AppConfiguration);
        File.WriteAllBytes(filePath, json);
    }

    public static bool TryLoad(string filePath, [NotNullWhen(true)] out AppConfiguration? configuration)
    {
        if (!File.Exists(filePath))
        {
            configuration = null;
            return false;
        }

        ReadOnlySpan<byte> json = File.ReadAllBytes(filePath);
        configuration = JsonSerializer.Deserialize(json, ConfigurationSerializerContext.Default.AppConfiguration)
            ?? throw new JsonException("Failed to read the configuration file!");

        return true;
    }
}
