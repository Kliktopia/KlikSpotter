namespace KlikSpotter.Configuration;

internal unsafe class CustomByteArrayConverter: JsonConverter<byte[]>
{
    // Misc. ASCII values used in the conversion
    private const byte ASCII_SPACE = 0x20;

    // Printable characters
    private const byte ASCII_DOUBLE_QUOTE = 0x22;
    private const byte ASCII_AMPERSAND = 0x26;
    private const byte ASCII_APOSTROPHE = 0x27;
    private const byte ASCII_PLUS = 0x2b;
    private const byte ASCII_COMMA = 0x2c;
    private const byte ASCII_ZERO = 0x30;
    private const byte ASCII_LESS_THAN_SIGN = 0x3c;
    private const byte ASCII_GREATER_THAN_SIGN = 0x3e;
    private const byte ASCII_OPEN_BRACKET = 0x5b;
    private const byte ASCII_BACKSLASH = 0x5c;
    private const byte ASCII_CLOSE_BRACKET = 0x5d;
    private const byte ASCII_GRAVE_ACCENT = 0x60;
    private const byte ASCII_SMALL_LETTER_U = 0x75;

    private const byte ASCII_MIN_VALUE = 0x20;
    private const byte ASCII_MAX_VALUE = 0x7e;

    private static readonly byte[] _hexDigits =
    [
        (byte)'0', (byte)'1', (byte)'2', (byte)'3',
        (byte)'4', (byte)'5', (byte)'6', (byte)'7',
        (byte)'8', (byte)'9', (byte)'A', (byte)'B',
        (byte)'C', (byte)'D', (byte)'E', (byte)'F',
    ];

    public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var buffer = new List<byte>();

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number when reader.TryGetByte(out byte value):
                    buffer.Add(value);
                    break;

                case JsonTokenType.String:
                    buffer.AddRange(Encoding.UTF8.GetBytes(reader.GetString()!));
                    break;

                default:
                    throw new JsonException();
            }
        }

        return [.. buffer];
    }

    public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
    {
        // Should be plenty
        Span<byte> buffer = stackalloc byte[value.Length * 16];

        int writtenBytes = 0, index = 0;
        buffer[writtenBytes++] = ASCII_OPEN_BRACKET;

        if (value.Length == 0)
        {
            buffer[writtenBytes++] = ASCII_CLOSE_BRACKET;
            writer.WriteRawValue(buffer[..writtenBytes]);

            return;
        }

        bool indexIsAscii = IndexIsAscii(value, index);

        if (indexIsAscii)
        {
            buffer[writtenBytes++] = ASCII_DOUBLE_QUOTE;
            writtenBytes += GetCharacter(buffer[writtenBytes..], value[index++]);
        }
        else
        {
            writtenBytes += WriteNumberAsBytes(buffer[writtenBytes..], value[index++]);
        }

        while (index < value.Length)
        {
            if (indexIsAscii)
            {
                while (IndexIsAscii(value, index))
                    writtenBytes += GetCharacter(buffer[writtenBytes..], value[index++]);

                buffer[writtenBytes++] = ASCII_DOUBLE_QUOTE;
                indexIsAscii = false;
                continue;
            }

            buffer[writtenBytes++] = ASCII_COMMA;
            buffer[writtenBytes++] = ASCII_SPACE;

            indexIsAscii = IndexIsAscii(value, index);

            if (indexIsAscii)
            {
                buffer[writtenBytes++] = ASCII_DOUBLE_QUOTE;
                continue;
            }

            writtenBytes += WriteNumberAsBytes(buffer[writtenBytes..], value[index++]);
        }

        buffer[writtenBytes++] = ASCII_CLOSE_BRACKET;

        writer.WriteRawValue(buffer[..writtenBytes]);

        static bool IndexIsAscii(byte[] value, int index) => index < value.Length && (value[index] is >= ASCII_MIN_VALUE and <= ASCII_MAX_VALUE);
    }

    // Get the figure at the specified place
    private static byte GetFigureAtPlace(int value, int place, int radix) 
        => _hexDigits[value / (place == 0 ? 1 : place == 1 ? radix : radix * radix) % radix];

    private static int GetCharacter(Span<byte> span, byte value)
    {
        int writtenBytes = 0;

        if (value is ASCII_BACKSLASH)
        {
            span[writtenBytes++] = ASCII_BACKSLASH;
            span[writtenBytes++] = ASCII_BACKSLASH;
        }
        else if (value
            is ASCII_DOUBLE_QUOTE
            or ASCII_AMPERSAND
            or ASCII_APOSTROPHE
            or ASCII_PLUS
            or ASCII_LESS_THAN_SIGN
            or ASCII_GREATER_THAN_SIGN
            or ASCII_GRAVE_ACCENT)
        {
            span[writtenBytes++] = ASCII_BACKSLASH;
            span[writtenBytes++] = ASCII_SMALL_LETTER_U;
            span[writtenBytes++] = ASCII_ZERO;
            span[writtenBytes++] = ASCII_ZERO;
            span[writtenBytes++] = GetFigureAtPlace(value, place: 1, radix: 16);
            span[writtenBytes++] = GetFigureAtPlace(value, place: 0, radix: 16);
        }
        else
        {
            span[writtenBytes++] = value;
        }

        return writtenBytes;
    }

    private static int WriteNumberAsBytes(Span<byte> span, byte value)
    {
        int writtenBytes = 0;

        if (value is < ASCII_MIN_VALUE or > ASCII_MAX_VALUE)
        {
            byte ones = GetFigureAtPlace(value, place: 0, 10),
                 tens = GetFigureAtPlace(value, place: 1, 10),
                 huns = GetFigureAtPlace(value, place: 2, 10);

            if (huns is not ASCII_ZERO) span[writtenBytes++] = huns;
            if (tens is not ASCII_ZERO) span[writtenBytes++] = tens;
            span[writtenBytes++] = ones;
        }
        else
        {
            span[writtenBytes++] = ASCII_DOUBLE_QUOTE;
            writtenBytes += GetCharacter(span[writtenBytes..], value);
            span[writtenBytes++] = ASCII_DOUBLE_QUOTE;
        }

        return writtenBytes;
    }
}
