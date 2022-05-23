namespace Longtail;

public unsafe class StorageApi : IDisposable
{
    private Longtail_StorageAPI* _api;

    private StorageApi(Longtail_StorageAPI* api)
    {
        _api = api;
    }

    internal Longtail_StorageAPI* AsPointer() => _api;

    public static StorageApi CreateFSStorageAPI()
    {
        var api = LongtailLibrary.Longtail_CreateFSStorageAPI();
        if (api == null)
        {
            throw new OutOfMemoryException();
        }
        return new(api);
    }

    public void Dispose()
    {
        if (_api != null)
        {
            LongtailLibrary.Longtail_DisposeAPI(&_api->m_API);
            _api = null;
        }
    }
}