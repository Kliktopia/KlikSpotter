namespace KlikSpotter;

internal class PinnedArrayHandle<T>: SafeHandle where T : struct
{
    private GCHandle _gcHandle;

    public PinnedArrayHandle(T[] array)
        : base(IntPtr.Zero, true)
    {
        ArgumentNullException.ThrowIfNull(array);

        _gcHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
        SetHandle(_gcHandle.AddrOfPinnedObject());
    }

    public override bool IsInvalid => (handle == IntPtr.Zero);

    protected override bool ReleaseHandle()
    {
        if (_gcHandle.IsAllocated) _gcHandle.Free();
        return true;
    }
}
