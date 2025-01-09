namespace KlikSpotter.Logging;

public class CustomFileLogger(StreamWriter logFileWriter): ILogger
{
    private readonly StreamWriter _logFileWriter = logFileWriter;

    public IDisposable BeginScope<TState>(TState state)
        => new NullDisposable();

    public bool IsEnabled(LogLevel logLevel)
        => logLevel >= LogLevel.Information;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        string message = formatter(state, exception);
        message = StripAnsiColors(message);

        if (!string.IsNullOrEmpty(message))
            _logFileWriter.Write($"[{logLevel}] {message}\n");
        else
            _logFileWriter.Write('\n');

        _logFileWriter.Flush();
    }

    [return: NotNullIfNotNull(nameof(message))]
    private static string? StripAnsiColors(string? message)
    {
        if (string.IsNullOrEmpty(message)) return message;

        int start, end;
        while ((start = message.IndexOf('\x1b')) != -1
            && start + 1 < message.Length
            && (end = message.IndexOf('m', start + 1)) != -1)
            message = message[..start] + message[(end + 1)..];

        return message;
    }
}

internal class NullDisposable: IDisposable
{
    public void Dispose()
    { }
}
