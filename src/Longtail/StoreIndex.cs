using Longtail.Internal;

namespace Longtail;

public sealed unsafe class StoreIndex : IDisposable
{
    private Longtail_StoreIndex* _storeIndex;
    private readonly bool _owner;
    internal Longtail_StoreIndex* AsPointer() => _storeIndex;

    internal StoreIndex(Longtail_StoreIndex* storeIndex, bool owner = true)
    {
        _storeIndex = storeIndex;
        _owner = owner;
    }

    public uint GetVersion() => LongtailLibrary.Longtail_StoreIndex_GetVersion(_storeIndex);
    public uint GetHashIdentifier() => LongtailLibrary.Longtail_StoreIndex_GetHashIdentifier(_storeIndex);
    public uint GetBlockCount() => LongtailLibrary.Longtail_StoreIndex_GetBlockCount(_storeIndex);
    public uint GetChunkCount() => LongtailLibrary.Longtail_StoreIndex_GetChunkCount(_storeIndex);
    // NOTE(Jens): these does not have set size, they depend on other factors. Not sure what to return? maybe just a struct with an indexer that wont check sizes?
    public ulong* GetBlockHashes() => throw new NotImplementedException("No fixed size for this, so we'll have to support unsafe code. TBD");
    public ulong* GetChunkHashes() => throw new NotImplementedException("No fixed size for this, so we'll have to support unsafe code. TBD");
    public ReadOnlySpan<uint> GetBlockChunksOffsets() => new(LongtailLibrary.Longtail_StoreIndex_GetBlockChunksOffsets(_storeIndex), (int)GetBlockCount());
    public ReadOnlySpan<uint> GetBlockChunkCounts() => new(LongtailLibrary.Longtail_StoreIndex_GetBlockChunkCounts(_storeIndex), (int)GetBlockCount());
    public ReadOnlySpan<uint> GetBlockTags() => new(LongtailLibrary.Longtail_StoreIndex_GetBlockTags(_storeIndex), (int)GetBlockCount());
    public uint* GetChunkSizes() => throw new NotImplementedException("No fixed size for this, so we'll have to support unsafe code. TBD");


    public StoreIndex? PruneStoreIndex(ReadOnlySpan<ulong> keepBlockhashes)
    {
        Longtail_StoreIndex* prunedStoreIndex;

        fixed (ulong* pKeepBlockHashes = keepBlockhashes)
        {
            var err = LongtailLibrary.Longtail_PruneStoreIndex(_storeIndex, (uint)keepBlockhashes.Length, pKeepBlockHashes, &prunedStoreIndex);
            if (err != 0)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_PruneStoreIndex), err);
            }
        }
        return prunedStoreIndex != null ? new StoreIndex(prunedStoreIndex) : null;
    }

    // TODO(Jens): not sure if this is the correct way to do this. it can return other error codes.
    public bool ValidateStore(VersionIndex versionIndex)
        => LongtailLibrary.Longtail_ValidateStore(_storeIndex, versionIndex.AsPointer()) == 0;

    public StoreIndex? CopyStoreIndex()
    {
        var storeIndex = LongtailLibrary.Longtail_CopyStoreIndex(_storeIndex);
        return storeIndex != null ? new StoreIndex(storeIndex) : null;
    }

    public LongtailBuffer WriteStoreIndexToBuffer()
    {
        void* buffer;
        ulong size;
        var err = LongtailLibrary.Longtail_WriteStoreIndexToBuffer(_storeIndex, &buffer, &size);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_WriteStoreIndexToBuffer), err);
        }

        return new LongtailBuffer(buffer, size);
    }
    public void WriteStoreIndex(string path, StorageApi storageApi)
    {
        using var utf8Path = new Utf8String(path);
        var err = LongtailLibrary.Longtail_WriteStoreIndex(storageApi.AsPointer(), _storeIndex, utf8Path);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_WriteStoreIndex), err);
        }
    }

    public static StoreIndex? ReadStoreIndexFromBuffer(ReadOnlySpan<byte> buffer)
    {
        Longtail_StoreIndex* storeindex;
        fixed (byte* pBuffer = buffer)
        {
            var err = LongtailLibrary.Longtail_ReadStoreIndexFromBuffer(pBuffer, (ulong)buffer.Length, &storeindex);
            if (err != 0)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_ReadStoreIndexFromBuffer), err);
            }
        }
        return storeindex != null ? new StoreIndex(storeindex) : null;
    }

    public static StoreIndex? ReadStoreIndex(string path, StorageApi storageApi)
    {
        using var utf8Path = new Utf8String(path);
        Longtail_StoreIndex* storeindex;
        var err = LongtailLibrary.Longtail_ReadStoreIndex(storageApi.AsPointer(), utf8Path, &storeindex);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_ReadStoreIndex), err);
        }
        return storeindex != null ? new StoreIndex(storeindex) : null;
    }

    public static StoreIndex CreateMissingContent(HashApi hashApi, StoreIndex storeIndex, VersionIndex versionIndex, uint minBlockSize, uint maxChunksPerBlock)
    {
        Longtail_StoreIndex* outStoreIndex;
        var err = LongtailLibrary.Longtail_CreateMissingContent(hashApi.AsPointer(), storeIndex.AsPointer(), versionIndex.AsPointer(), minBlockSize, maxChunksPerBlock, &outStoreIndex);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_CreateMissingContent), err);
        }
        if (outStoreIndex == null)
        {
            // TODO: add proper error for null pointers.
            throw new LongtailException(nameof(LongtailLibrary.Longtail_CreateMissingContent), ErrorCodesEnum.EINVAL);
        }
        return new StoreIndex(outStoreIndex);
    }

    public void Dispose()
    {
        if (_storeIndex != null && _owner)
        {
            LongtailLibrary.Longtail_Free(_storeIndex);
            _storeIndex = null;
        }
    }
}