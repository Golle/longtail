using Longtail;

namespace LongtailSample02;

internal class SampleAsyncBlockStore : IAsyncBlockstore
{
    private readonly string _basePath;
    private readonly string _fileExtension;
    private readonly StorageApi _storageApi;

    public SampleAsyncBlockStore(string basePath, string fileExtension, StorageApi storageApi)
    {
        _basePath = basePath;
        _fileExtension = fileExtension;
        _storageApi = storageApi;
    }

    public Task PutStoredBlock(StoredBlock storedBlock)
    {
        var blockHash = storedBlock.BlockIndex.BlockHash;
        var path = Path.Combine(_basePath, $"{blockHash:X16}.{_fileExtension}");
        storedBlock.WriteStoredBlock(path, _storageApi);
        return Task.CompletedTask;
    }

    public Task PreflightGet(ReadOnlyMemory<ulong> blockHashes)
    {
        //throw new NotImplementedException();
        return Task.CompletedTask;

    }

    public Task<StoredBlock?> GetStoredBlock(ulong blockHash)
    {
        var path = Path.Combine(_basePath, $"{blockHash:X16}.{_fileExtension}");

        var block = StoredBlock.ReadStoredBlock(path, _storageApi);
        return Task.FromResult(block)!;
    }
   
    public async Task<StoreIndex?> GetExistingContent(ReadOnlyMemory<ulong> blockHashes, uint minBlockUsagePercent)
    {
        using var fileInfos = FileInfos.GetFilesRecursively(_basePath, _storageApi);
        if (fileInfos.GetCount() == 0)
        {
            return StoreIndex.CreateStoreIndexFromBlocks(ReadOnlySpan<BlockIndex>.Empty);
        }

        var blockIndexes = new BlockIndex[(int)fileInfos.GetCount()];
        for (var i = 0u; i < fileInfos.GetCount(); ++i)
        {
            var path = fileInfos.GetPath(i);
            blockIndexes[i] = await _storageApi.ReadBlockIndexAsync(Path.Combine(_basePath, path));
        }

        try
        {
            return StoreIndex.CreateStoreIndexFromBlocks(blockIndexes);
        }
        finally
        {
            foreach (var blockIndex in blockIndexes)
            {
                blockIndex.Dispose();
            }
        }
    }

    public Task<uint> PruneBlocks(ReadOnlyMemory<ulong> blockKeepHashes)
    {
        throw new NotImplementedException();
    }

    public Task Flush()
    {
        throw new NotImplementedException();
    }

    public BlockstoreStats? GetStats()
    {
        throw new NotImplementedException();
    }

    public void OnDispose()
    {
        //throw new NotImplementedException();
    }
}