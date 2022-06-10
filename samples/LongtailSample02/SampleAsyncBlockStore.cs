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

    public async Task PutStoredBlock(StoredBlock storedBlock)
    {
        var blockHash = storedBlock.BlockIndex.BlockHash;
        var path = Path.Combine(_basePath, $"{blockHash:X16}.{_fileExtension}");
        await using var file = File.OpenWrite(path);
        await file.WriteAsync(storedBlock.BlockData.ToArray(), CancellationToken.None);
    }

    public Task PreflightGet(ReadOnlyMemory<ulong> blockHashes)
    {
        throw new NotImplementedException();
    }

    public Task<StoredBlock?> GetStoredBlock(ulong blockHash)
    {
        throw new NotImplementedException();
    }

    public async Task<StoreIndex?> GetExistingContent(ReadOnlyMemory<ulong> blockHashes, uint minBlockUsagePercent)
    {
        //using var fileStorage = StorageApi.CreateFSStorageAPI();
        using var fileInfos = FileInfos.GetFilesRecursively(_basePath, _storageApi);
        if (fileInfos.GetCount() == 0)
        {
            return StoreIndex.CreateStoreIndexFromBlocks(ReadOnlySpan<BlockIndex>.Empty);
        }
        StoreIndex? storeIndex = null;
        try
        {
            foreach (var file in Directory.EnumerateFiles(_basePath, $"*.{_fileExtension}", SearchOption.AllDirectories))
            {
                var contents = await File.ReadAllBytesAsync(file);
                using var tmpStoreIndex = StoreIndex.ReadStoreIndexFromBuffer(contents);
                storeIndex ??= new StoreIndex(); // Create an empty storeindex if there's none.
                var mergedStoreIndex = storeIndex.MergeStoreIndex(tmpStoreIndex);
                storeIndex.Dispose();
                storeIndex = mergedStoreIndex;
            }
        }
        catch
        {
            storeIndex?.Dispose();
            throw;
        }
        
        return storeIndex;
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