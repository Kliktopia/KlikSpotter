namespace KlikSpotter.Comparers;

internal class SearchPatternComparer: IComparer<SearchPattern>
{
    public int Compare(SearchPattern? x, SearchPattern? y)
    {
        int comparison;

        if (x is null) return y is null ? 0 : -1;
        if (y is null) return 1;

        // If we've reached this point, neither x nor y are null.
        if ((comparison = x.Priority - y.Priority) != 0) return comparison;

        int minLength = x.Data.Length < y.Data.Length ? x.Data.Length : y.Data.Length, i;
        for (i = 0; i < minLength; i++)
            if ((comparison = x.Data[i] - y.Data[i]) != 0) return comparison;

        // If we've reached this point, the arrays are equal up to the minimum length.
        if ((comparison = x.Data.Length - y.Data.Length) > 0)
            return x.Data[i] * comparison;
        else if (comparison < 0)
            return y.Data[i] * comparison;

        // If we've reached this point, the arrays are equal.
        return string.Compare(x.Alias, y.Alias);
    }
}
