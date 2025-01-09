using System.ComponentModel;

namespace KlikSpotter.Service;

internal class SearchPattern
{
    public required string Alias { get; init; }
    [JsonConverter(typeof(CustomByteArrayConverter))]
    public required byte[] Data { get; init; }
    public int Priority { get; init; }
    public FileType FileType { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Position { get; init; }
    public IEnumerable<string>? FileExtensions { get; init; }
    public IEnumerable<MatchIndicator>? Indicators { get; init; }

    public SearchPattern()
    {
        Alias = string.Empty;
        Data = [];
        FileType = FileType.All;
    }

    [SetsRequiredMembers]
    public SearchPattern(string alias, string data, bool isUnicode, FileType fileType = FileType.All) 
        : this(
              alias,
              [.. isUnicode ? data.SelectMany(BitConverter.GetBytes) : data.Select(c => (byte)(c & 0xff))],
              isUnicode: false, 
              fileType)
    {
    }

    [SetsRequiredMembers]
    public SearchPattern(string alias, int[] data, bool isUnicode = false, FileType fileType = FileType.All)
    {
        Alias = alias;
        FileType = fileType;

        if (isUnicode)
        {
            Data = new byte[data.Length << 1];
            for (int i = 0; i < data.Length; i++)
            {
                Debug.Assert(data[i] >= 0);
                Debug.Assert(data[i] <= 0xffff);

                Data[i << 1] = (byte)(data[i] & 0xff);
                Data[i << 1 | 1] = (byte)(data[i] >> 8 & 0xff);
            }
        }
        else
        {
            Data = new byte[data.Length];
            for (int index = 0; index < data.Length; index++)
            {
                Debug.Assert(data[index] >= 0);
                Debug.Assert(data[index] <= 0xff);

                Data[index] = (byte)(data[index] & 0xff);
            }
        }
    }
}
