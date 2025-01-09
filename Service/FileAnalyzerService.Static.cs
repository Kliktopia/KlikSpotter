namespace KlikSpotter.Service;

internal partial class FileAnalyzerService
{
    private (AppParameters Parameters, AppConfiguration Configuration, ILogger Logger) Initialize(AppParameters parameters)
    {
        if (parameters.DoShowVersion)
        {
            var version = FileVersionInfo.GetVersionInfo(parameters.ProcessPath);
            ConsoleHelper.WriteVersionToConsole(version);
        }

        var loggerFactory = LoggerFactory.Create(configure =>
        {
            if (!parameters.DoSilently)
            {
                configure.AddConsoleFormatter<CustomConsoleLoggerFormatter, ConsoleFormatterOptions>(configure =>
                {
                    configure.IncludeScopes = false;
                });

                configure.AddConsole(configure =>
                {
                    configure.FormatterName = nameof(CustomConsoleLoggerFormatter);
                });
            }

            if (parameters.DoFileLogging)
            {
                var fileLoggerProvider = new CustomFileLoggerProvider();
                configure.AddProvider(fileLoggerProvider);
            }
        });

        ILogger logger = loggerFactory.CreateLogger<FileAnalyzerService>();

        string configFilename = $"{Path.GetFileNameWithoutExtension(parameters.ProcessPath)}.config";
        string configPath = Path.Combine(parameters.ProcessDirectory, configFilename);

        AppConfiguration? configuration = null;
        bool doCreateNewConfigFile = false;

        if (parameters.DoResetConfig)
        {
            logger.LogWarning("Resetting the configuration file.\n");
            File.Delete(configPath);
            doCreateNewConfigFile = true;
        }
        else if (!AppConfiguration.TryLoad(configPath, out configuration))
        {
            logger.LogWarning("Configuration file "{configFilename}" not found. Creating a new one.\n", configFilename);
            doCreateNewConfigFile = true;
        }

        configuration ??= new()
        {
            FileExtensions = DefaultConfiguration.FileExtensions,
            FilesThatIndicatesMatch = DefaultConfiguration.FilesThatIndicatesMatch,
            FilesToRemove = DefaultConfiguration.FilesToRemove,
            OutputDirectories = DefaultConfiguration.DirectoryNames,
            SearchPatterns = DefaultConfiguration.SearchPatterns,
        };

        if (doCreateNewConfigFile) configuration.Save(configPath);

        if (parameters.DoResetConfig)
            Environment.Exit(0);

        if (parameters.DoCopy && parameters.DoMoveOriginal)
        {
            logger.LogCritical("You can't copy and move files at the same time!");
            Environment.Exit(1);
        }

        string? outputDirectory = parameters.OutputDirectory;

        if (string.IsNullOrEmpty(outputDirectory))
            outputDirectory = Path.Combine(parameters.ProcessDirectory, configuration.GetDirectoryPath(DirectoryType.Found));
        else if (!Path.IsPathRooted(outputDirectory))
            outputDirectory = Path.Combine(parameters.ProcessDirectory, outputDirectory);

        return (parameters with { OutputDirectory = outputDirectory }, configuration, logger);
    }
}
