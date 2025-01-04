namespace KlikSpotter.Configuration;

internal class Parser
{
    public static IEnumerable<SearchPattern> LoadSearchPatternsFromJson(string filePath)
    {
        string jsonString = File.ReadAllText(filePath);
        var searchPatterns = JsonSerializer.Deserialize<IEnumerable<SearchPattern>>(jsonString, SearchPatternJsonContext.Default.IEnumerableSearchPattern);
        return searchPatterns ?? [];
    }

    public static void SaveSearchPatternsToJson(IReadOnlyDictionary<string, byte[]> patterns)
    {
        var searchPatterns = patterns.Select(static kvp => new SearchPattern { Alias = kvp.Key, Data = kvp.Value });
        string jsonString = JsonSerializer.Serialize(searchPatterns, SearchPatternJsonContext.Default.IEnumerableSearchPattern);
        File.WriteAllText(@"C:\Temp\junk.txt", jsonString);
    }
}
