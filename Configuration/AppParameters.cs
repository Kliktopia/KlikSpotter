namespace KlikSpotter.Configuration;

internal readonly struct AppParameters: ICommandParameterSet
{
    public bool DoShowVersion { get; init; }
    public bool DoDebug { get; init; }
    public bool DoCopy { get; init; }
    public bool DoRecursiveSearch { get; init; }
    public bool DoMoveOriginal { get; init; }
    public bool DoSilently { get; init; }
    public bool DoFileLogging { get; init; }
    public bool DoResetConfig { get; init; }
    public string OutputDirectory { get; init; }
    public string SearchDirectory { get; init; }
    public string ProcessPath { get; init; }
    public string ProcessDirectory { get; init; }

    public AppParameters(
        [Option("debug"), Hidden]
        bool debug,
        [Option("version", ['v'], Description = "Prompt current version.", StopParsingOptions = true)]
        bool version,
        [Option("copy", ['c'], Description = "Copy (and extract) files that were detected as a Kliky to the output directory.")]
        bool copy,
        [Option("move", ['m'], Description = "Move files that were detected as Kliky to the output directory.")]
        bool move,
        [Option("recursive", ['r'], Description = "Recursively scan subdirectories.")]
        bool recursive,
        [Option("silent", ['s'], Description = "Spare me the the details.")]
        bool silent,
        [Option("file-logging", ['f'], Description = "Enable file logging.")]
        bool fileLogging,
        [Option("reset-config", Description = "Reset the configuration file to default settings.")]
        bool resetConfig,
        [Option("output", ['o'], Description = "The directory where to copy or move the found Kliky stuff.")]
        string? output,
        [Argument("input", Description = "The directory where to scan for Kliky stuff.")]
        string? input)
    {
        DoDebug = debug;
        DoShowVersion = version;
        DoCopy = copy;
        DoRecursiveSearch = recursive;
        DoMoveOriginal = move;
        DoSilently = silent;
        DoFileLogging = fileLogging;
        DoFileLogging = fileLogging;
        DoResetConfig = resetConfig;

        OutputDirectory = GetRootedPath(output ?? input);
        SearchDirectory = GetRootedPath(input);

        ProcessPath = Environment.ProcessPath!;
        ProcessDirectory = Path.GetDirectoryName(ProcessPath)!;

        static string GetRootedPath(string? path)
        {
            if (path is null) 
                path = Directory.GetCurrentDirectory();
            else if (!Path.IsPathRooted(path))
                path = Path.Join(Directory.GetCurrentDirectory(), path);

            return FileHelper.GetNormalizedPath(path);
        }
    }
}
