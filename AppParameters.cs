namespace KlikSpotter;

internal class AppParameters: ICommandParameterSet
{
    public bool DoShowVersion;
    public bool DoDebug;
    public bool DoCopy;
    public bool DoRecursiveSearch;
    public bool DoMoveOriginal;
    public bool DoSilently;
    public bool DoFileLogging;
    public string ProcessPath;
    public string ProcessDirectory;
    public string OutputDirectory;
    public string SearchDirectory;

    public AppParameters(
        [Option(StopParsingOptions = true), Hidden]
        bool debug,
        [Option('v', Description = "Prompt current version.", StopParsingOptions = true)]
        bool version,
        [Option('c', Description = "Copy (and extract) files that were detected as a Kliky.")]
        bool copy,
        [Option('r', Description = "Recursively scan subdirectories.")]
        bool recursive,
        [Option('m', Description = "Move files that were detected as Kliky to a separate directory. (!MOVED)")]
        bool move,
        [Option('s', Description = "Spare me the the details.")]
        bool silent,
        [Option('f', Description = "Enable file logging.")]
        bool fileLogging,
        [Option('o', Description = "The directory where to copy or move the found Kliky stuff.")]
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

        ProcessPath = Environment.ProcessPath!;
        ProcessDirectory = Path.GetDirectoryName(ProcessPath)!;
        SearchDirectory = input ?? ProcessDirectory;
        OutputDirectory = output ?? Path.Combine(SearchDirectory, "!FOUND");
    }
}
