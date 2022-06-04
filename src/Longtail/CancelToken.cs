namespace Longtail;

// NOTE(Jens): can this be replaced by CancellationToken ?
public unsafe class CancelToken : IDisposable
{
    private Longtail_CancelAPI* _cancelApi;
    private Longtail_CancelAPI_CancelToken* _token;

    internal Longtail_CancelAPI_CancelToken* AsPointer() => _token;
    public bool IsCancelled => LongtailLibrary.Longtail_CancelAPI_IsCancelled(_cancelApi, _token) != 0;

    internal CancelToken(Longtail_CancelAPI* cancelApi, Longtail_CancelAPI_CancelToken* token)
    {
        _cancelApi = cancelApi;
        _token = token;
    }

    public void Cancel() => LongtailLibrary.Longtail_CancelAPI_Cancel(_cancelApi, _token);

    public void Dispose()
    {
        if (_token != null)
        {
            LongtailLibrary.Longtail_CancelAPI_DisposeToken(_cancelApi, _token);
            _token = null;
            _cancelApi = null;
        }
    }
}