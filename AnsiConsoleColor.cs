namespace KlikSpotter;

internal partial class FileAnalyzerService
{
    private readonly struct AnsiConsoleColor
    {
        public readonly byte R;
        public readonly byte G;
        public readonly byte B;
        private readonly int _basicColor = -1;
        private readonly bool _fullRgb;

        public AnsiConsoleColor(byte r, byte g, byte b, bool fullRgb = false)
        {
            R = r;
            G = g;
            B = b;
            _fullRgb = fullRgb;
        }

        private AnsiConsoleColor(int basicColor) => _basicColor = basicColor;

        public readonly override string ToString()
        {
            if (_basicColor != -1) 
                return $"\x1b[{_basicColor}m";

            if (_fullRgb)
                return $"\x1b[38;2;{R};{G};{B}m";

            int color = 16;
            color += B / 43;
            color += (G / 43) * 6;
            color += (R / 43) * 36;

            return $"\x1b[38;5;{color}m";
        }

        public static readonly AnsiConsoleColor Reset = new(0);

        public static readonly AnsiConsoleColor Black = new(30);
        public static readonly AnsiConsoleColor DarkRed = new(31);
        public static readonly AnsiConsoleColor DarkGreen = new(32);
        public static readonly AnsiConsoleColor DarkYellow = new(33);
        public static readonly AnsiConsoleColor DarkBlue = new(34);
        public static readonly AnsiConsoleColor DarkMagenta = new(35);
        public static readonly AnsiConsoleColor DarkCyan = new(36);
        public static readonly AnsiConsoleColor DarkWhite = new(37);
        public static readonly AnsiConsoleColor BrightBlack = new(90);
        public static readonly AnsiConsoleColor BrightRed = new(91);
        public static readonly AnsiConsoleColor BrightGreen = new(92);
        public static readonly AnsiConsoleColor BrightYellow = new(93);
        public static readonly AnsiConsoleColor BrightBlue = new(94);
        public static readonly AnsiConsoleColor BrightMagenta = new(95);
        public static readonly AnsiConsoleColor BrightCyan = new(96);
        public static readonly AnsiConsoleColor White = new(97);
    }
}
