namespace KlikSpotter.Logging;

internal class CustomConsoleLoggerFormatter: ConsoleFormatter
{
    private readonly bool _hadAnsiColors;

    public CustomConsoleLoggerFormatter()
        : base(nameof(CustomConsoleLoggerFormatter))
    {
        _hadAnsiColors = Win32Helpers.HasAnsiColors;
        if (!_hadAnsiColors) Win32Helpers.ToggleAnsiColors();
    }

    ~CustomConsoleLoggerFormatter()
    {
        if (!_hadAnsiColors) Win32Helpers.ToggleAnsiColors();
    }

    public override void Write<TState>(
        in LogEntry<TState> logEntry,
        IExternalScopeProvider? scopeProvider,
        TextWriter textWriter)
    {
        string? message = logEntry.Formatter?.Invoke(logEntry.State, logEntry.Exception);
        if (message is null) return;

        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = GetLogLevelColor(logEntry.LogLevel);
        textWriter.WriteLine(message);
        Console.ForegroundColor = originalColor;

        static ConsoleColor GetLogLevelColor(LogLevel logLevel) => logLevel switch
        {
            LogLevel.Trace => ConsoleColor.Gray,
            LogLevel.Debug => ConsoleColor.Gray,
            LogLevel.Information => ConsoleColor.White,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Magenta,
            LogLevel.Critical => ConsoleColor.Red,
            _ => ConsoleColor.White,
        };
    }
}
