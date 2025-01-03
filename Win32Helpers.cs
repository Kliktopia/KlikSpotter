namespace KlikSpotter;

internal static unsafe partial class Win32Helpers
{
    private const uint STD_OUTPUT_HANDLE = ~10U;
    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4;
    private const uint FILE_ATTRIBUTE_INVALID = ~0U;
    private static readonly PinnedArrayHandle<sbyte> _pathHandle;
    private static readonly sbyte[] _pathBuffer;

    static Win32Helpers()
    {
        const uint MAX_PATH = 0x8000;

        _pathBuffer = new sbyte[MAX_PATH];
        _pathHandle = new(_pathBuffer);
    }

    public static void ToggleAnsiColors()
    {
        nint handle = GetStdHandle(STD_OUTPUT_HANDLE);
        GetConsoleMode(handle, out uint mode);
        SetConsoleMode(handle, mode ^ ENABLE_VIRTUAL_TERMINAL_PROCESSING);
    }

    public static bool HasAnsiColors()
    {
        nint handle = GetStdHandle(STD_OUTPUT_HANDLE);
        GetConsoleMode(handle, out uint mode);
        return (mode & ENABLE_VIRTUAL_TERMINAL_PROCESSING) != 0;
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

        if ((attrFrom = GetFileAttributes($@"\\?\{from}")) == FILE_ATTRIBUTE_INVALID ||
            (attrTo = GetFileAttributes($@"\\?\{to}")) == FILE_ATTRIBUTE_INVALID ||
            (!PathRelativePathTo(_pathBuffer, from, attrFrom, to, attrTo)))
        {
            relativePath = null;
            return false;
        }

        relativePath = Marshal.PtrToStringUni(_pathHandle.DangerousGetHandle());
        return relativePath is not null;
    }

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
        [Out] sbyte[] pszPath,
        [MarshalAs(UnmanagedType.LPWStr)] string pszFrom,
        uint dwAttrFrom,
        [MarshalAs(UnmanagedType.LPWStr)] string pszTo,
        uint dwAttrTo);
}
