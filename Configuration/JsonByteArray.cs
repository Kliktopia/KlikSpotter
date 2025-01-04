namespace KlikSpotter.Configuration;

internal class JsonByteArrayConverter: JsonConverter<byte[]>
{
    // Misc. ASCII values
    private const byte ASCII_SPACE = 0x20;
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

    public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string? stringValue = reader.GetString();
            return stringValue is not null ? Encoding.UTF8.GetBytes(stringValue) : [];
        }

        byte[] bytes = [0];
        var hexArray = new List<byte>();

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number when reader.TryGetByte(out bytes[0]):
                    hexArray.Add(bytes[0]);
                    break;
                case JsonTokenType.String:
                    string? stringValue = reader.GetString();
                    if (stringValue is null || (bytes = Encoding.UTF8.GetBytes(stringValue)).Length == 0)
                        break;
                    hexArray.AddRange(bytes);
                    break;
            }
        }

        return [.. hexArray];
    }

    public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] is not < ASCII_MIN_VALUE and not > ASCII_MAX_VALUE)
                continue;

            WriteAsNumberArray(writer, value);
            return;
        }

        writer.WriteStringValue(Encoding.ASCII.GetString(value));
    }

    private static void WriteAsNumberArray(Utf8JsonWriter writer, byte[] array)
    {
        const int MAX_TRANSFORMED_BYTE_LENGTH = 5; // 3 digits max + 1 comma + 1 space
        const int EXCESS_LENGTH = 2; // 2 brackets

        int bufferLength = array.Length * MAX_TRANSFORMED_BYTE_LENGTH + EXCESS_LENGTH;
        byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferLength);

        int position = 0;
        buffer[position++] = ASCII_OPEN_BRACKET;

        if (array.Length > 0)
        {
            int index;
            for (index = 0; index < array.Length - 1; index++)
            {
                position += WriteNumber(buffer.AsSpan(position), array[index]);

                buffer[position++] = ASCII_COMMA;
                buffer[position++] = ASCII_SPACE;
            }

            // Write the last element, without the comma and space
            position += WriteNumber(buffer.AsSpan(start: position), array[index]);
        }

        buffer[position++] = ASCII_CLOSE_BRACKET;
        writer.WriteRawValue(buffer.AsSpan(0, position));

        ArrayPool<byte>.Shared.Return(buffer);

        return;

        // Local function, returns the number of bytes written
        static int WriteNumber(Span<byte> span, byte value)
        {
            const string HEX_FIGURES = "0123456789ABCDEF";

            int offset = 0;

            if (value is < ASCII_MIN_VALUE or > ASCII_MAX_VALUE)
            {
                byte[] fig = [GetDecChar(value, 0), GetDecChar(value, 1), GetDecChar(value, 2)];

                if (fig[2] != ASCII_ZERO) span[offset++] = fig[2];
                if (fig[1] != ASCII_ZERO) span[offset++] = fig[1];
                span[offset++] = fig[0];

                return offset;
            }

            span[offset++] = ASCII_DOUBLE_QUOTE;

            if (value is ASCII_BACKSLASH)
            {
                span[offset++] = ASCII_BACKSLASH;
                span[offset++] = ASCII_BACKSLASH;
            }
            else if (value is ASCII_DOUBLE_QUOTE
                or ASCII_AMPERSAND
                or ASCII_APOSTROPHE
                or ASCII_PLUS
                or ASCII_LESS_THAN_SIGN
                or ASCII_GREATER_THAN_SIGN
                or ASCII_GRAVE_ACCENT)
            {
                span[offset++] = ASCII_BACKSLASH;
                span[offset++] = ASCII_SMALL_LETTER_U;
                span[offset++] = ASCII_ZERO;
                span[offset++] = ASCII_ZERO;
                span[offset++] = GetHexChar(value, 1);
                span[offset++] = GetHexChar(value, 0);
            }
            else
            {
                span[offset++] = value;
            }

            span[offset++] = ASCII_DOUBLE_QUOTE;

            return offset;

            // Local local functions ;-)
            static byte GetDecChar(int value, int place) => (byte)HEX_FIGURES[value / (place == 0 ? 1 : place == 1 ? 10 : 100) % 10];
            static byte GetHexChar(int value, int place) => (byte)HEX_FIGURES[value / (1 << (place * 4)) & 0xf];
        }
    }
}
