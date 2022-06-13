namespace Longtail;

public sealed unsafe class HashApiContext : IDisposable
{
    private readonly Longtail_HashAPI* _hashApi;
    private Longtail_HashAPI_Context* _context;

    internal HashApiContext(Longtail_HashAPI* hashApi, Longtail_HashAPI_Context* context)
    {
        _hashApi = hashApi;
        _context = context;
    }

    public ulong EndContext()
    {
        if (_context != null)
        {
            var hash = LongtailLibrary.Longtail_Hash_EndContext(_hashApi, _context);
            _context = null;

            return hash;
        }
        throw new ObjectDisposedException(nameof(HashApiContext), "The hash api context as already been disposed");
    }

    public void Hash(ReadOnlySpan<byte> data)
    {
        fixed (byte* pData = data)
        {
            LongtailLibrary.Longtail_Hash_Hash(_hashApi, _context, (uint)data.Length, pData);
        }
    }

    public void Dispose()
    {
        if (_context != null)
        {
            EndContext();
        }
    }
}