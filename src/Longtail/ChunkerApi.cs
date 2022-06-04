namespace Longtail;

public unsafe class ChunkerApi : IDisposable
{
    private Longtail_ChunkerAPI* _chunkerApi;

    internal ChunkerApi(Longtail_ChunkerAPI* chunkerApi)
    {
        _chunkerApi = chunkerApi;
    }

    public uint GetMinChunkSize()
    {
        uint chunkSize;
        var err = LongtailLibrary.Longtail_Chunker_GetMinChunkSize(_chunkerApi, &chunkSize);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_Chunker_GetMinChunkSize), err);
        }
        return chunkSize;
    }

    public static ChunkerApi CreateHPCDCChunkerAPI()
    {
        var api = LongtailLibrary.Longtail_CreateHPCDCChunkerAPI();
        return api != null ? new ChunkerApi(api) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_CreateHPCDCChunkerAPI)} returned a null pointer"); ;
    }

    internal Longtail_ChunkerAPI* AsPointer() => _chunkerApi;

    public void Dispose()
    {
        if (_chunkerApi != null)
        {
            LongtailLibrary.Longtail_DisposeAPI(&_chunkerApi->m_API);
            _chunkerApi = null;
        }
    }
}