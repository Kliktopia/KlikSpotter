namespace KlikSpotter;

internal class AppParameters: ICommandParameterSet
{
    public bool DoShowVersion;
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
        [Option('v', Description = "Prompt Current Version.", StopParsingOptions = true)]
        bool version,
        [Option('c', Description = "Copy (and Extract) Files that was Detected as a Klik Program.")]
        bool copy,
        [Option('r', Description = "Recursively Scan Sub Directories.")]
        bool recursive,
        [Option('m', Description = "Move Files that was Detected as a Klik Program to a Separate Directory. (!MOVED)")]
        bool move,
        [Option('s', Description = "Don't tell Me about the Details while Scanning.")]
        bool silent,
        [Option('f', Description = "Enable File Logging.")]
        bool fileLogging,
        [Option('o', Description = "The Directory where to Copy or Move the Klik Games, Programs and Installers.")]
        string? output,
        [Argument("input", Description = "The Directory where to Scan for Klik Games, Programs and Installers.")] string? input)
    {
        DoShowVersion = version;
        DoCopy = copy;
        DoRecursiveSearch = recursive;
        DoMoveOriginal = move;
        DoSilently = silent;
        DoFileLogging = fileLogging;
        DoFileLogging = fileLogging; ;

        ProcessPath = Environment.ProcessPath!;
        ProcessDirectory = Path.GetDirectoryName(ProcessPath)!;
        SearchDirectory = input ?? ProcessDirectory;
        OutputDirectory = output ?? Path.Combine(SearchDirectory, "!FOUND");
    }
}
