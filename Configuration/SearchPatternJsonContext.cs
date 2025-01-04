namespace KlikSpotter.Configuration;

[JsonSerializable(typeof(IEnumerable<SearchPattern>))]
[JsonSourceGenerationOptions(
    Converters = [typeof(JsonByteArrayConverter)],
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
    IndentCharacter = ' ',
    IndentSize = 4,
    NewLine = "\n",
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    WriteIndented = true)]
internal partial class SearchPatternJsonContext: JsonSerializerContext
{
}
