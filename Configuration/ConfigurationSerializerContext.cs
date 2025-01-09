namespace KlikSpotter.Configuration;

[JsonSerializable(typeof(AppConfiguration))]
[JsonSourceGenerationOptions(
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
    RespectNullableAnnotations = true,
    UseStringEnumConverter = true,
    IndentCharacter = ' ',
    IndentSize = 4,
    NewLine = "\n",
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    WriteIndented = true)]
internal partial class ConfigurationSerializerContext: JsonSerializerContext
{
}
