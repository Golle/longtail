using Longtail.Internal;

namespace Longtail;

public sealed unsafe class HashApi : IDisposable
{
    private Longtail_HashAPI* _hashApi;
    private readonly bool _owner;
    internal Longtail_HashAPI* AsPointer() => _hashApi;
    public uint GetIdentifier() => LongtailLibrary.Longtail_Hash_GetIdentifier(_hashApi);
    internal HashApi(Longtail_HashAPI* hashApi, bool owner = true)
    {
        _hashApi = hashApi;
        _owner = owner;
    }

    public static HashApi CreateBlake2HashAPI()
    {
        var api = LongtailLibrary.Longtail_CreateBlake2HashAPI();
        return api != null ? new HashApi(api) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_CreateBlake2HashAPI)} returned a null pointer");
    }

    public static HashApi CreateBlake3HashAPI()
    {
        var api = LongtailLibrary.Longtail_CreateBlake3HashAPI();
        return api != null ? new HashApi(api) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_CreateBlake2HashAPI)} returned a null pointer");
    }
    public static HashApi CreateMeowHashAPI()
    {
        var api = LongtailLibrary.Longtail_CreateMeowHashAPI();
        return api != null ? new HashApi(api) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_CreateBlake2HashAPI)} returned a null pointer");
    }

    public HashApiContext BeginContext()
    {
        Longtail_HashAPI_Context* context;
        var err = LongtailLibrary.Longtail_Hash_BeginContext(_hashApi, &context);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_Hash_BeginContext), err);
        }
        return context != null ? new HashApiContext(_hashApi, context) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_Hash_BeginContext)} returned a null pointer");
    }

    public ulong HashBuffer(ReadOnlySpan<byte> data)
    {
        fixed (byte* pdata = data)
        {
            ulong hash;
            var err = LongtailLibrary.Longtail_Hash_HashBuffer(_hashApi, (uint)data.Length, pdata, &hash);
            if (err != 0)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_Hash_HashBuffer), err);
            }
            return hash;
        }
    }

    public ulong GetPathHash(string path)
    {
        using var utf8Path = new Utf8String(path);
        ulong hash;
        var err = LongtailLibrary.Longtail_GetPathHash(_hashApi, utf8Path, &hash);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_GetPathHash), err);
        }
        return hash;
    }

    public void Dispose()
    {
        if (_hashApi != null && _owner)
        {
            LongtailLibrary.Longtail_DisposeAPI((Longtail_API*)_hashApi);
            _hashApi = null;
        }
    }
}