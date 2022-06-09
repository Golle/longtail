namespace Longtail;

public static class BlockStoreAsyncApiExtensions
{

    // TODO: investigate how to properly make an async api for these functions
    // TODO: can TaskCompletionSource be used instead of EventWaitHandle ? Maybe we can avoid Task.Run
    public static Task<StoredBlock?> GetStoredBlockAsync(this BlockStoreApi api, ulong blockHash)
        => Task.Run(() => api.GetStoredBlock(blockHash));

    public static Task FlushAsync(this BlockStoreApi api) => Task.Run(api.Flush);
    public static Task<StoreIndex?> GetExistingContentAsync(this BlockStoreApi api, ReadOnlyMemory<ulong> chunkHashes, uint minBlockUsagePercent = 0)
        => Task.Run(() => api.GetExistingContent(chunkHashes.Span, minBlockUsagePercent));
}