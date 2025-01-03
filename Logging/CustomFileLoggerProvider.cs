namespace KlikSpotter.Logging;

internal class CustomFileLoggerProvider: ILoggerProvider
{
    private readonly StreamWriter _logFileWriter;

    public CustomFileLoggerProvider()
    {
        _logFileWriter = new StreamWriter("KlikSpotter.log");
    }

    public ILogger CreateLogger(string _)
        => new CustomFileLogger(_logFileWriter);

    public void Dispose()
    {
        _logFileWriter.Flush();
        _logFileWriter.Dispose();
    }
}
