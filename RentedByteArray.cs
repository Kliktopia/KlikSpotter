namespace KlikSpotter;

internal class RentedByteArray: IReadOnlyList<byte>, IDisposable
{
    private static readonly Stack<byte[]> _arrayStack = new();
    private readonly byte[] _array;
    private bool _disposed;

    private int _start = 0;
    private int _length = 0;

    public int Start => _start;
    public int Count => _length;

    public RentedByteArray(int length)
    {
        _array = ArrayPool<byte>.Shared.Rent(length);
        _start = 0;
        _length = length;
        _arrayStack.Push(_array);
    }

    public Span<byte> AsSpan() => _array.AsSpan().Slice(_start, _length);

    public static implicit operator Span<byte>(RentedByteArray array) => array.AsSpan();

    public int IndexOf(ReadOnlySpan<byte> span, int index = 0) => Slice(index).IndexOf(span);

    public int IndexOf(ReadOnlySpan<byte> span, int index, int length) => Slice(index, length).IndexOf(span);

    public byte this[int index] => AsSpan()[_start + index];

    public Span<byte> Slice(int start) => AsSpan().Slice(start);

    public Span<byte> Slice(int start, int length) => AsSpan().Slice(start, length);

    public ReadOnlySpan<byte> this[Range range]
    {
        get
        {
            var (start, length) = range.GetOffsetAndLength(_length);
            return Slice(start, length);
        }
    }

    public RentedByteArray Trim(int start, int length)
    {
        if (start < 0 || start >= _length)
            throw new ArgumentOutOfRangeException(nameof(start));

        if (length < 0 || start + length > _length)
            throw new ArgumentOutOfRangeException(nameof(length));

        _start += start;
        _length = length;

        return this;
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        var bytes = _arrayStack.Pop();
        ArrayPool<byte>.Shared.Return(bytes, false);
        _disposed = true;

        GC.SuppressFinalize(this);
    }

    public IEnumerator<byte> GetEnumerator()
        => ((IEnumerable<byte>)_array).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
