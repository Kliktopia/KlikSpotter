namespace KlikSpotter.Helpers;

internal static class ConsoleHelper
{
    public  static void WriteToConsole(string line, AnsiConsoleColor? color)
    {
        if (!color.HasValue || !Win32Helper.HasAnsiColors)
        {
            Console.Write(line);
            return;
        }

        Console.Write($"{color.Value}{line}{AnsiConsoleColor.Reset}");
    }

    [DoesNotReturn]
    public static void WriteVersionToConsole(FileVersionInfo version)
    {
        foreach (var (line, color) in GetLogo(version))
            WriteToConsole(line, color);

        Environment.Exit(0);
    }

    private static IEnumerable<(string, AnsiConsoleColor?)> GetLogo(FileVersionInfo version)
    {
        yield return (@" _  ___ _ _     _____             _   _" + '\n', new(0xe8, 0x14, 0x16));
        yield return (@"| |/ / (_) |   / ____|           | | | |" + '\n', new(0xe8, 0x14, 0x16));
        yield return (@"| ' /| |_| | _| (___  _ __   ___ | |_| |_ ___ _ __" + '\n', new(0xff, 0xa5, 0x00));
        yield return (@"|  < | | | |/ /\___ \| '_ \ / _ \| __| __/ _ \ '__|" + '\n', new(0xfa, 0xeb, 0x36));
        yield return (@"| . \| | |   < ____) | |_) | (_) | |_| ||  __/ |" + '\n', new(0x79, 0xc3, 0x14));
        yield return (@"|_|\_\_|_|_|\_\_____/| .__/ \___/ \__|\__\___|_|" + '\n', new(0x48, 0x7d, 0xe7));
        yield return (@"                     | |" + '\n', new(0x4b, 0x36, 0x9d));
        yield return (@"                     |_|  ", new(0x70, 0x36, 0x9d));
        yield return (GetVersionString(version), null);
        yield return ("\n", null);
        yield return ("By Rikard Bengtsson (", null);
        yield return ("yxkalle", AnsiConsoleColor.DarkYellow);
        yield return (")\n", null);

        static string GetVersionString(FileVersionInfo version)
        {
            string versionString = $"{version.FileMajorPart}.{version.FileMinorPart}";
            if (version.FileBuildPart > 0 || version.FilePrivatePart > 0) versionString += $".{version.FileBuildPart}";
            if (version.FilePrivatePart > 0) versionString += $".{version.FilePrivatePart}";

            return $"VERSION {versionString}\n";
        }
    }
}
