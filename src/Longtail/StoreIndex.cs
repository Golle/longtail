using Longtail.Internal;

namespace Longtail;

public sealed unsafe class StoreIndex : IDisposable
{
    private Longtail_StoreIndex* _storeIndex;
    private readonly bool _owner;
    internal Longtail_StoreIndex* AsPointer() => _storeIndex;

    public StoreIndex()
    {
        using var name = new Utf8String(nameof(StoreIndex));
        _storeIndex = (Longtail_StoreIndex*)LongtailLibrary.Longtail_Alloc(name, (ulong)sizeof(Longtail_StoreIndex));
        *_storeIndex = default;
        _owner = true;
    }

    internal StoreIndex(Longtail_StoreIndex* storeIndex, bool owner = true)
    {
        _storeIndex = storeIndex;
        _owner = owner;
    }

    public uint Version => *_storeIndex->m_Version;
    public uint HashIdentifner => *_storeIndex->m_HashIdentifier;
    public uint BlockCount => *_storeIndex->m_BlockCount;
    public uint ChunkCount => *_storeIndex->m_ChunkCount;
    public ReadOnlySpan<ulong> BlockHashes => new(_storeIndex->m_BlockHashes, (int)_storeIndex->m_BlockCount);
    public ReadOnlySpan<uint> BlockChunksOffsets => new(_storeIndex->m_BlockChunksOffsets, (int)BlockCount);
    public ReadOnlySpan<uint> BlockChunkCounts => new(_storeIndex->m_BlockChunkCounts, (int)BlockCount);
    public ReadOnlySpan<uint> BlockTags => new(_storeIndex->m_BlockTags, (int)BlockCount);
    public ReadOnlySpan<uint> GetChunkSizes(uint index)
    {
        if (index >= ChunkCount)
        {
            throw new IndexOutOfRangeException($"The index {index} is out of range. Max allowed: {ChunkCount}");
        }
        var count = _storeIndex->m_BlockChunkCounts[index];
        var offset = _storeIndex->m_BlockChunksOffsets[index];
        return new(_storeIndex->m_ChunkSizes + offset, (int)count);
    }

    public ReadOnlySpan<ulong> GetChunkHashes(uint index)
    {
        if (index >= ChunkCount)
        {
            throw new IndexOutOfRangeException($"The index {index} is out of range. Max allowed: {ChunkCount}");
        }
        var count = _storeIndex->m_BlockChunkCounts[index];
        var offset = _storeIndex->m_BlockChunksOffsets[index];
        return new(_storeIndex->m_ChunkHashes + offset, (int)count);
    }

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

    public StoreIndex[] SplitStoreIndex(ulong splitSize)
    {
        Longtail_StoreIndex** outIndices;
        ulong outCount = 0;
        var err = LongtailLibrary.Longtail_SplitStoreIndex(_storeIndex, splitSize, &outIndices, &outCount);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_SplitStoreIndex), err);
        }
        // We don't use the array that longtail allocated for us, we use a managed array.
        LongtailLibrary.Longtail_Free(outIndices);
        if (outCount == 0)
        {
            return Array.Empty<StoreIndex>();
        }

        var indices = new StoreIndex[outCount];
        for (var i = 0ul; i < outCount; ++i)
        {
            indices[i] = new StoreIndex(outIndices[i]);
        }
        return indices;
    }

    public static StoreIndex ReadStoreIndexFromBuffer(ReadOnlySpan<byte> buffer)
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
        return storeindex != null ? new StoreIndex(storeindex) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_ReadStoreIndexFromBuffer)} returned a null pointer");
    }

    public static StoreIndex ReadStoreIndex(string path, StorageApi storageApi)
    {
        using var utf8Path = new Utf8String(path);
        Longtail_StoreIndex* storeindex;
        var err = LongtailLibrary.Longtail_ReadStoreIndex(storageApi.AsPointer(), utf8Path, &storeindex);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_ReadStoreIndex), err);
        }
        return storeindex != null ? new StoreIndex(storeindex) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_ReadStoreIndexFromBuffer)} returned a null pointer");
    }

    public static StoreIndex CreateMissingContent(HashApi hashApi, StoreIndex storeIndex, VersionIndex versionIndex, uint minBlockSize, uint maxChunksPerBlock)
    {
        Longtail_StoreIndex* outStoreIndex;
        var err = LongtailLibrary.Longtail_CreateMissingContent(hashApi.AsPointer(), storeIndex.AsPointer(), versionIndex.AsPointer(), minBlockSize, maxChunksPerBlock, &outStoreIndex);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_CreateMissingContent), err);
        }
        return outStoreIndex != null ? new StoreIndex(outStoreIndex) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_CreateMissingContent)} returned a null pointer");
    }

    public uint GetMissingContent(ReadOnlySpan<ulong> chunkHashes, Span<ulong> outMissingChunkHashes)
    {
        var outChunkCount = (uint)outMissingChunkHashes.Length;
        fixed (ulong* pChunkHashes = chunkHashes)
        fixed (ulong* pMissingChunkHashes = outMissingChunkHashes)
        {
            var err = LongtailLibrary.Longtail_GetMissingChunks(_storeIndex, (uint)chunkHashes.Length, pChunkHashes, &outChunkCount, pMissingChunkHashes);
            if (err != 0)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_GetMissingChunks), err);
            }
        }
        return outChunkCount;
    }


    //public static extern ulong Longtail_GetStoreIndexSize(
    //    uint block_count,
    //    uint chunk_count
    //);
    //public static extern int Longtail_CreateStoreIndex(
    //    Longtail_HashAPI* hash_api,
    //    uint chunk_count,
    //    ulong* chunk_hashes,
    //    uint* chunk_sizes,
    //    uint* optional_chunk_tags,
    //    uint max_block_size,
    //    uint max_chunks_per_block,
    //    Longtail_StoreIndex** out_store_index
    //);

    public static StoreIndex CreateStoreIndexFromBlocks(ReadOnlySpan<BlockIndex> blockIndexes)
    {
        var indexes = stackalloc Longtail_BlockIndex*[blockIndexes.Length];
        for (var i = 0; i < blockIndexes.Length; ++i)
        {
            indexes[i] = blockIndexes[i].AsPointer();
        }

        Longtail_StoreIndex* storeIndex;
        var err = LongtailLibrary.Longtail_CreateStoreIndexFromBlocks((uint)blockIndexes.Length, indexes, &storeIndex);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_CreateStoreIndexFromBlocks), err);
        }
        return storeIndex != null ? new StoreIndex(storeIndex) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_CreateStoreIndexFromBlocks)} returned a null pointer");
    }
    public StoreIndex MergeStoreIndex(StoreIndex remoteStoreIndex)
    {
        Longtail_StoreIndex* newStoreIndex;
        var err = LongtailLibrary.Longtail_MergeStoreIndex(_storeIndex, remoteStoreIndex.AsPointer(), &newStoreIndex);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_MergeStoreIndex), err);
        }
        return newStoreIndex != null ? new StoreIndex(newStoreIndex) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_MergeStoreIndex)} returned a null pointer");
    }

    //public static extern int Longtail_MakeBlockIndex(
    //    Longtail_StoreIndex* store_index,
    //    uint block_index,
    //    Longtail_BlockIndex* out_block_index
    //);


    public StoreIndex GetExistingStoreIndex(ReadOnlySpan<ulong> chunks, uint minBlockUsagePercent)
    {
        fixed (ulong* pChunks = chunks)
        {
            Longtail_StoreIndex* existingStoreIndex;
            var err = LongtailLibrary.Longtail_GetExistingStoreIndex(_storeIndex, (uint)chunks.Length, pChunks, minBlockUsagePercent, &existingStoreIndex);
            if (err != 0)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_GetExistingStoreIndex), err);
            }
            return existingStoreIndex != null ? new StoreIndex(existingStoreIndex) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_GetExistingStoreIndex)} returned a null pointer"); ;
        }
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
