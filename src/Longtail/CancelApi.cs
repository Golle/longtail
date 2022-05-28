namespace Longtail;

public unsafe class CancelApi : IDisposable
{
    private Longtail_CancelAPI* _cancelApi;
    private CancelApi(Longtail_CancelAPI* cancelApi)
    {
        _cancelApi = cancelApi;
    }

    public static CancelApi? CreateAtomicCancelAPI()
    {
        var api = LongtailLibrary.Longtail_CreateAtomicCancelAPI();
        return api != null ? new CancelApi(api) : null;
    }

    internal Longtail_CancelAPI* AsPointer() => _cancelApi;
    public void Dispose()
    {
        if (_cancelApi != null)
        {
            LongtailLibrary.Longtail_DisposeAPI(&_cancelApi->m_API);
            _cancelApi = null;
        }
    }
}