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
        return api != null ? new StorageApi(api) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_CreateFSStorageAPI)} returned a null pointer"); ;
    }

    public static StorageApi CreateInMemoryStorageAPI()
    {
        var api = LongtailLibrary.Longtail_CreateInMemStorageAPI();
        return api != null ? new StorageApi(api) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_CreateInMemStorageAPI)} returned a null pointer"); ;
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