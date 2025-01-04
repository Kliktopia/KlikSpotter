namespace KlikSpotter.Configuration;

internal class SearchPattern
{
    public required string Alias { get; set; }
    [JsonConverter(typeof(JsonByteArrayConverter))]
    public required byte[] Data { get; set; }
    public int? Offset { get; set; }
    public bool Unicode { get; set; }
}
