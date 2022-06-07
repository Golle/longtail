namespace Longtail;

public interface IAsyncBlockstore
{
    Task PutStoredBlock(StoredBlock storedBlock);
    Task PreflightGet(ReadOnlyMemory<ulong> blockHashes);
    Task<StoredBlock?> GetStoredBlock(ulong blockHash);
    Task<StoreIndex?> GetExistingContent(ReadOnlyMemory<ulong> blockHashes, uint minBlockUsagePercent);
    Task<uint> PruneBlocks(ReadOnlyMemory<ulong> blockKeepHashes);
    Task Flush();

    // Should this be async?
    BlockstoreStats? GetStats();
    void OnDispose();
}