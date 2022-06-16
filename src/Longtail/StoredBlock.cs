using Longtail.Internal;

namespace Longtail;

public unsafe class StoredBlock : IDisposable
{
    private Longtail_StoredBlock* _storedBlock;
    private readonly bool _owner;
    internal Longtail_StoredBlock* AsPointer() => _storedBlock;
    public BlockIndex BlockIndex => new(_storedBlock->m_BlockIndex, false);
    public uint BlockChunksDataSize => _storedBlock->m_BlockChunksDataSize;
    public ReadOnlySpan<byte> BlockData => new(_storedBlock->m_BlockData, (int)_storedBlock->m_BlockChunksDataSize);
    public StoredBlock()
    {
        _storedBlock = (Longtail_StoredBlock*)LongtailLibrary.Longtail_Alloc(null, (ulong)sizeof(Longtail_StoredBlock));
        _owner = true;
        if (_storedBlock == null)
        {
            throw new OutOfMemoryException(nameof(StoredBlock));
        }
    }

    internal StoredBlock(Longtail_StoredBlock* storedBlock, bool owner = true)
    {
        _storedBlock = storedBlock;
        _owner = owner;
    }

    public static ulong GetStoredBlockSize(ulong blockDataSize) => LongtailLibrary.Longtail_GetStoredBlockSize(blockDataSize);

    public void InitStoredBlockFromData(ReadOnlySpan<byte> blockData)
    {
        if (_storedBlock == null)
        {
            throw new InvalidOperationException($"Failed to {nameof(InitStoredBlockFromData)} because the stored block is null");
        }
        fixed (byte* pBlockData = blockData)
        {
            var err = LongtailLibrary.Longtail_InitStoredBlockFromData(_storedBlock, pBlockData, (ulong)blockData.Length);
            if (err != 0)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_InitStoredBlockFromData), err);
            }
        }
    }

    public static StoredBlock CreateStoredBlock(ulong blockHash, uint hashIdentifier, uint blockDataSize, uint tag, ReadOnlySpan<ulong> chunkHashes, ReadOnlySpan<uint> chunkSizes)
    {
        if (chunkHashes.Length != chunkSizes.Length)
        {
            throw new InvalidOperationException($"The length of parameteters {nameof(chunkHashes)} and {nameof(chunkSizes)} must be the same");
        }
        var chunkCount = (uint)chunkHashes.Length;

        Longtail_StoredBlock* storedBlock;
        fixed (ulong* pChunkHashes = chunkHashes)
        fixed (uint* pChunkSizes = chunkSizes)
        {
            var err = LongtailLibrary.Longtail_CreateStoredBlock(blockHash, hashIdentifier, chunkCount, tag, pChunkHashes, pChunkSizes, blockDataSize, &storedBlock);
            if (err != 0)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_CreateStoredBlock), err);
            }
        }
        return new StoredBlock(storedBlock);
    }

    public static StoredBlock ReadStoredBlockFromBuffer(ReadOnlySpan<byte> buffer)
    {
        Longtail_StoredBlock* storedBlock;
        fixed (byte* pBuffer = buffer)
        {
            var err = LongtailLibrary.Longtail_ReadStoredBlockFromBuffer(pBuffer, (ulong)buffer.Length, &storedBlock);
            if (err != 0)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_ReadStoredBlockFromBuffer), err);
            }
        }
        return new StoredBlock(storedBlock);
    }

    public void WriteStoredBlock(string path, StorageApi storageApi)
    {
        using var utf8Path = new Utf8String(path);
        var err = LongtailLibrary.Longtail_WriteStoredBlock(storageApi.AsPointer(), _storedBlock, utf8Path);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_WriteStoredBlock), err);
        }
    }

    public static StoredBlock ReadStoredBlock(string path, StorageApi storageApi)
    {
        using var utf8Path = new Utf8String(path);
        Longtail_StoredBlock* storedBlock;
        var err = LongtailLibrary.Longtail_ReadStoredBlock(storageApi.AsPointer(), utf8Path, &storedBlock);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_ReadStoredBlock), err);
        }
        return new StoredBlock(storedBlock);
    }

    public void Dispose()
    {
        if (_storedBlock != null && _owner)
        {
            // NOTE(Jens): not sure if this shuold be called
            if (_storedBlock->Dispose != null)
            {
                _storedBlock->Dispose(_storedBlock);
            }
            LongtailLibrary.Longtail_Free(_storedBlock);
            _storedBlock = null;
        }
    }
}