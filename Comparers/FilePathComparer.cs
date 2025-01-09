namespace KlikSpotter.Comparers;

internal class FilePathComparer: IComparer<string>
{
    public int Compare(string? x, string? y)
    {
        if (x is null) return y is null ? 0 : -1;
        if (y is null) return 1;

        string[] xParts = x.Split(FileHelper.DirectorySeparators, StringSplitOptions.RemoveEmptyEntries), 
                 yParts = y.Split(FileHelper.DirectorySeparators, StringSplitOptions.RemoveEmptyEntries);

        int minLength = xParts.Length < yParts.Length ? xParts.Length : yParts.Length;

        int comparison;
        for (int i = 0; i < minLength - 1; i++)
            if ((comparison = string.Compare(xParts[i], yParts[i], StringComparison.OrdinalIgnoreCase)) != 0)
                return comparison;

        return xParts.Length.CompareTo(minLength);
    }
}
