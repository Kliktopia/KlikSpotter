namespace KlikSpotter.Configuration;

internal class FileExtension
{
    public required string Extension { get; init; }
    public bool? IsArchive { get; init; }
    public IEnumerable<MatchIndicator>? Indicators { get; init; }

    public FileExtension()
    {
    }

    [SetsRequiredMembers]
    public FileExtension(string extension, bool? isArchive = null, IEnumerable<MatchIndicator>? indicators = null)
    {
        Extension = extension;
        IsArchive = isArchive;
        Indicators = indicators;
    }
}
