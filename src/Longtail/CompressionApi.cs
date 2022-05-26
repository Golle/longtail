namespace Longtail;

public unsafe class CompressionApi : IDisposable
{
    public uint SettingsId { get; }
    private Longtail_CompressionAPI* _api;
    private readonly bool _owner;

    internal CompressionApi(Longtail_CompressionAPI* api, uint settingsId, bool owner = true)
    {
        SettingsId = settingsId;
        _api = api;
        _owner = owner;
    }

    public static CompressionApi? CreateForBrotli(uint compressionType)
    {
        uint settings;
        var api = LongtailLibrary.Longtail_CompressionRegistry_CreateForBrotli(compressionType, &settings);
        return api != null ? new CompressionApi(api, settings) : null;
    }

    public static CompressionApi? CreateForLZ4(uint compressionType)
    {
        uint settings;
        var api = LongtailLibrary.Longtail_CompressionRegistry_CreateForLZ4(compressionType, &settings);
        return api != null ? new CompressionApi(api, settings) : null;
    }

    public static CompressionApi? CreateForZstd(uint compressionType)
    {
        uint settings;
        var api = LongtailLibrary.Longtail_CompressionRegistry_CreateForZstd(compressionType, &settings);
        return api != null ? new CompressionApi(api, settings) : null;
    }

    public void Dispose()
    {
        if (_api != null && _owner)
        {
            LongtailLibrary.Longtail_DisposeAPI(&_api->m_API);
            _api = null;
        }
    }
}