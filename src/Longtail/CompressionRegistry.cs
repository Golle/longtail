using System.ComponentModel;

namespace Longtail;

public unsafe class CompressionRegistry : IDisposable
{
    private Longtail_CompressionRegistryAPI* _api;

    internal CompressionRegistry(Longtail_CompressionRegistryAPI* api)
    {
        _api = api;
    }

    public static ulong GetCompressionRegistryAPISize() => LongtailLibrary.Longtail_GetCompressionAPISize();
    public static CompressionRegistry? CreateDefaultCompressionRegistry() => throw new NotImplementedException("This function has not been implemented yet, it might not be needed.");
    public static CompressionRegistry? CreateFullCompressionRegistry()
    {
        var api = LongtailLibrary.Longtail_CreateFullCompressionRegistry();
        return api != null ? new CompressionRegistry(api) : null;
    }

    public static CompressionRegistry? CreateZStdCompressionRegistry()
    {
        var api = LongtailLibrary.Longtail_CreateZStdCompressionRegistry();
        return api != null ? new CompressionRegistry(api) : null;
    }

    public CompressionApi? GetCompressionAPI(uint compressionType)
    {
        Longtail_CompressionAPI* compressionApi;
        uint settingsId;
        var err = LongtailLibrary.Longtail_GetCompressionRegistry_GetCompressionAPI(_api, compressionType, &compressionApi, &settingsId);
        if (err == ErrorCodes.ENOENT)
        {
            return null;
        }
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_GetCompressionRegistry_GetCompressionAPI), err);
        }
        return compressionApi != null ? new CompressionApi(compressionApi, settingsId, false) : null;
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