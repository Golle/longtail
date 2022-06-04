namespace Longtail;

public unsafe class CancelApi : IDisposable
{
    private Longtail_CancelAPI* _cancelApi;
    private CancelApi(Longtail_CancelAPI* cancelApi)
    {
        _cancelApi = cancelApi;
    }
    internal Longtail_CancelAPI* AsPointer() => _cancelApi;

    public static CancelApi CreateAtomicCancelAPI()
    {
        var api = LongtailLibrary.Longtail_CreateAtomicCancelAPI();
        if (api == null)
        {
            throw new OutOfMemoryException(nameof(LongtailLibrary.Longtail_CreateAtomicCancelAPI));
        }
        return new CancelApi(api);
    }

    public CancelToken CreateToken()
    {
        Longtail_CancelAPI_CancelToken* token;
        var err = LongtailLibrary.Longtail_CancelAPI_CreateToken(_cancelApi, &token);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_CancelAPI_CreateToken), err);
        }
        return token != null ? new CancelToken(_cancelApi, token) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_CancelAPI_CreateToken)} returned a null pointer");
    }

    //public static extern Longtail_CancelAPI* Longtail_MakeCancelAPI(
    //    void* mem,
    //    delegate* unmanaged[Cdecl]<Longtail_API*, void> dispose_func,
    //    delegate* unmanaged[Cdecl]<Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken**, int> create_token_func,
    //    delegate* unmanaged[Cdecl]<Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken*, int> cancel_func,
    //    delegate* unmanaged[Cdecl]<Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken*, int> is_cancelled,
    //    delegate* unmanaged[Cdecl]<Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken*, int> dispose_token_func
    //);

    public void Dispose()
    {
        if (_cancelApi != null)
        {
            LongtailLibrary.Longtail_DisposeAPI(&_cancelApi->m_API);
            _cancelApi = null;
        }
    }
}