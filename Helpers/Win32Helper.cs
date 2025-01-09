namespace KlikSpotter.Helpers;

internal static unsafe partial class Win32Helper
{
    private const int MAX_PATH = 0x8000;
    private const uint STD_OUTPUT_HANDLE = ~10U;
    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4;
    
    public const uint FILE_ATTRIBUTE_INVALID = ~0U;

    public static void ToggleAnsiColors()
    {
        nint handle = GetStdHandle(STD_OUTPUT_HANDLE);
        GetConsoleMode(handle, out uint mode);
        SetConsoleMode(handle, mode ^ ENABLE_VIRTUAL_TERMINAL_PROCESSING);
    }

    public static bool HasAnsiColors
    {
        get
        {
            nint handle = GetStdHandle(STD_OUTPUT_HANDLE);
            GetConsoleMode(handle, out uint mode);
            return (mode & ENABLE_VIRTUAL_TERMINAL_PROCESSING) != 0;
        }
    }

    public static bool TryGetPathRelativePath(string? from, string? to, [NotNullWhen(true)] out string? relativePath)
    {
        uint attrFrom, attrTo;

        if (from is null)
        {
            relativePath = null;
            return false;
        }
        else if (to is null)
        {
            relativePath = from;
            return true;
        }

        sbyte* buffer = stackalloc sbyte[MAX_PATH];

        if ((attrFrom = GetFileAttributes($@"\\?\{from}")) == FILE_ATTRIBUTE_INVALID ||
            (attrTo = GetFileAttributes($@"\\?\{to}")) == FILE_ATTRIBUTE_INVALID ||
            (!PathRelativePathTo(buffer, from, attrFrom, to, attrTo)))
        {
            relativePath = null;
            return false;
        }

        relativePath = Marshal.PtrToStringUni((IntPtr)buffer);
        return relativePath is not null;
    }

    public static bool IsValidPath(string path)
        => GetFileAttributes($@"\\?\{path}") != FILE_ATTRIBUTE_INVALID;

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial nint GetStdHandle(uint handle);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetConsoleMode(nint handle, out uint dwMode);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetConsoleMode(nint hConsoleHandle, uint dwMode);

    [LibraryImport("kernel32.dll", EntryPoint = "GetFileAttributesW", StringMarshalling = StringMarshalling.Utf16)]
    public static partial uint GetFileAttributes([MarshalAs(UnmanagedType.LPWStr)] string lpFileName);

    [LibraryImport("shlwapi.dll", EntryPoint = "PathRelativePathToW", StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool PathRelativePathTo(
        sbyte* pszPath,
        [MarshalAs(UnmanagedType.LPWStr)] string pszFrom,
        uint dwAttrFrom,
        [MarshalAs(UnmanagedType.LPWStr)] string pszTo,
        uint dwAttrTo);
}
