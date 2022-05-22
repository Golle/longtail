namespace Longtail;

public interface IBlockstore
{
    ErrorCodesEnum PutStoredBlock(StoredBlock storedBlock, Action<ErrorCodesEnum> onCompleteCallback);
    ErrorCodesEnum PreflightGet(ReadOnlySpan<ulong> blockHashes, Action<ErrorCodesEnum> optionalOnComplete);
    ErrorCodesEnum GetStoredBlock(ulong blockHash, Action<StoredBlock?, ErrorCodesEnum> onComplete);
    ErrorCodesEnum GetExistingContentFunc(ReadOnlySpan<ulong> blockHashes, uint minBlockUsagePercent, Action<StoreIndex?, ErrorCodesEnum> asyncCompleteApi);
    ErrorCodesEnum PruneBlocks(ReadOnlySpan<ulong> blockKeepHashes, Action<uint, ErrorCodesEnum> asyncCompleteApi);
    BlockstoreStats? GetStats();
    ErrorCodesEnum Flush(Action<ErrorCodesEnum> asyncCompleteApi);
    void OnDispose();
}