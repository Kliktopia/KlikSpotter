using System.Drawing;

namespace KlikSpotter.Logging;

internal class CustomConsoleLoggerFormatter: ConsoleFormatter
{
    private readonly bool _hadAnsiColors;

    public CustomConsoleLoggerFormatter()
        : base(nameof(CustomConsoleLoggerFormatter))
    {
        _hadAnsiColors = Win32Helper.HasAnsiColors;
        if (!_hadAnsiColors) Win32Helper.ToggleAnsiColors();
    }

    ~CustomConsoleLoggerFormatter()
    {
        if (!_hadAnsiColors) Win32Helper.ToggleAnsiColors();
    }

    public override void Write<TState>(
        in LogEntry<TState> logEntry,
        IExternalScopeProvider? scopeProvider,
        TextWriter textWriter)
    {
        string? message = logEntry.Formatter?.Invoke(logEntry.State, logEntry.Exception);
        if (message is null) return;

        if (Win32Helper.HasAnsiColors)
        {
            var color = GetLogLevelColor(logEntry.LogLevel);
            message = $"{color}{message}{AnsiConsoleColor.Reset}";
        }
            
        textWriter.WriteLine(message);

        static AnsiConsoleColor GetLogLevelColor(LogLevel logLevel) => logLevel switch
        {
            LogLevel.Trace => AnsiConsoleColor.BrightBlack,
            LogLevel.Debug => AnsiConsoleColor.DarkCyan,
            LogLevel.Information => AnsiConsoleColor.DarkWhite,
            LogLevel.Warning => AnsiConsoleColor.BrightYellow,
            LogLevel.Error => AnsiConsoleColor.BrightMagenta,
            LogLevel.Critical => AnsiConsoleColor.BrightRed,
            _ => AnsiConsoleColor.White,
        };
    }
}
