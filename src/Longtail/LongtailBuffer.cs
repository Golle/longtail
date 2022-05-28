namespace Longtail;

public sealed unsafe class LongtailBuffer : IDisposable
{
    private void* _buffer;
    private readonly ulong _size;

    internal LongtailBuffer(void* buffer, ulong size)
    {
        _buffer = buffer;
        _size = size;
    }
    public Span<byte> AsSpan() => _buffer != null ? new(_buffer, (int)_size) : Span<byte>.Empty;
    public ReadOnlySpan<byte> AsReadOnlySpan() => _buffer != null ? new(_buffer, (int)_size) : Span<byte>.Empty;
    public void Dispose()
    {
        if (_buffer != null)
        {
            LongtailLibrary.Longtail_Free(_buffer);
            _buffer = null;
        }
    }
}