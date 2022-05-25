using Longtail.Internal;

namespace Longtail;

public unsafe class VersionIndex : IDisposable
{
    private Longtail_VersionIndex* _versionIndex;

    // Note(Jens): The implementation for these are very simple, maybe we should not do an interop call to get the values?
    public uint Version => LongtailLibrary.Longtail_VersionIndex_GetVersion(_versionIndex);
    public uint GetHashAPI => LongtailLibrary.Longtail_VersionIndex_GetVersion(_versionIndex);
    public uint AssetCount => LongtailLibrary.Longtail_VersionIndex_GetAssetCount(_versionIndex);
    public uint ChunkCount => LongtailLibrary.Longtail_VersionIndex_GetChunkCount(_versionIndex);
    public ulong* ChunkHashes => LongtailLibrary.Longtail_VersionIndex_GetChunkHashes(_versionIndex);
    public uint* ChunkSizes => LongtailLibrary.Longtail_VersionIndex_GetChunkSizes(_versionIndex);
    public uint* ChunkTags => LongtailLibrary.Longtail_VersionIndex_GetChunkTags(_versionIndex);
    internal Longtail_VersionIndex* AsPointer() => _versionIndex;

    internal VersionIndex(Longtail_VersionIndex* versionIndex)
    {
        _versionIndex = versionIndex;
    }

    public static VersionIndex? ReadFromBuffer(ReadOnlySpan<byte> buffer)
    {
        fixed (byte* pBuffer = buffer)
        {
            Longtail_VersionIndex* versionIndex;
            var err = LongtailLibrary.Longtail_ReadVersionIndexFromBuffer(pBuffer, (ulong)buffer.Length, &versionIndex);
            if (err != 0)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_ReadVersionIndexFromBuffer), err);
            }
            return versionIndex != null ? new VersionIndex(versionIndex) : null;
        }
    }

    public static VersionIndex? Read(IStorageApi unsafeStorageApi)
    {
        return null;
        //fixed (byte* pBuffer = buffer)
        //{
        //    Longtail_VersionIndex* versionIndex;
        //    var err = LongtailLibrary.Longtail_ReadVersionIndex(pBuffer, (ulong)buffer.Length, &versionIndex);
        //    if (err != 0)
        //    {
        //        throw new LongtailException(nameof(VersionIndex), nameof(ReadFromBuffer), nameof(LongtailLibrary.Longtail_ReadVersionIndexFromBuffer), err);
        //    }
        //    return versionIndex != null ? new VersionIndex(versionIndex) : null;
        //}
    }

    public static VersionIndex? Create(
        string rootPath, 
        StorageApi storageApi, 
        HashApi hashApi, 
        ChunkerApi chunkerApi, 
        JobApi jobApi, 
        FileInfos fileInfos, 
        uint targetChunkSize, 
        bool enableFileMap, 
        ProgressApi? progressApi = null, 
        CancelApi? cancelApi = null, 
        CancelToken? cancelToken = null)
    {
        using var rootPathUtf8 = new Utf8String(rootPath);

        Longtail_VersionIndex* versionIndex;
        var err = LongtailLibrary.Longtail_CreateVersionIndex(
            storageApi.AsPointer(),
            hashApi.AsPointer(),
            chunkerApi.AsPointer(),
            jobApi.AsPointer(),
            progressApi != null ? progressApi.AsPointer() : null,
            cancelApi != null ? cancelApi.AsPointer() : null,
            cancelToken != null ? cancelToken.AsPointer() : null,
            rootPathUtf8,
            fileInfos.AsPointer(),
            null,
            targetChunkSize,
            enableFileMap ? 1 : 0,
            &versionIndex
        );
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_ReadVersionIndexFromBuffer), err);
        }
        return versionIndex != null ? new VersionIndex(versionIndex) : null;
    }

    public void Dispose()
    {
        if (_versionIndex != null)
        {
            LongtailLibrary.Longtail_Free(_versionIndex);
            _versionIndex = null;
        }
    }
}

public unsafe class CancelToken
{
    private Longtail_CancelAPI_CancelToken* _token;

    // NOTE(Jens): can this be replaced by CancellationToken ?
    internal Longtail_CancelAPI_CancelToken* AsPointer() => _token;
}

public unsafe class CancelApi
{
    private Longtail_CancelAPI* _cancelApi;
    internal Longtail_CancelAPI* AsPointer() => _cancelApi;
}

public unsafe class ChunkerApi
{
    private Longtail_ChunkerAPI* _chunkerApi;
    internal Longtail_ChunkerAPI* AsPointer() => _chunkerApi;
}

public interface IStorageApi
{
    // TODO: Implement a safe version of the API
}