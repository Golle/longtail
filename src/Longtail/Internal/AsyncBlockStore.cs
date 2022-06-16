namespace Longtail.Internal;

internal class AsyncBlockStore : IBlockstore
{
    private readonly IAsyncBlockstore _blockstore;
    public AsyncBlockStore(IAsyncBlockstore blockstore)
    {
        _blockstore = blockstore;
    }

    public ErrorCodesEnum PutStoredBlock(StoredBlock storedBlock, Action<ErrorCodesEnum> onCompleteCallback)
    {
        // NOTE(Jens): Should revisit this  and see if there's a better way to do this.
        Task.Run(async () =>
        {
            var err = ErrorCodesEnum.SUCCESS;
            try
            {
                await _blockstore.PutStoredBlock(storedBlock);
            }
            catch (Exception e)
            {
                err = ExceptionToErrorCode.Translate(e);
            }
            finally
            {
                onCompleteCallback(err);
            }
        });
        return 0;
    }

    public ErrorCodesEnum PreflightGet(ReadOnlySpan<ulong> blockHashes, Action<ErrorCodesEnum> optionalOnComplete)
    {
        var hashes = blockHashes.ToArray();

        Task.Run(async () =>
        {
            var err = ErrorCodesEnum.SUCCESS;
            try
            {
                await _blockstore.PreflightGet(hashes);
            }
            catch (Exception e)
            {
                err = ExceptionToErrorCode.Translate(e);
            }
            finally
            {
                optionalOnComplete(err);
            }
        });
        return 0;
    }

    public ErrorCodesEnum GetStoredBlock(ulong blockHash, Action<StoredBlock?, ErrorCodesEnum> onComplete)
    {
        Task.Run(async () =>
        {
            StoredBlock? storedBlock = null;
            var err = ErrorCodesEnum.SUCCESS;
            try
            {
                storedBlock = await _blockstore.GetStoredBlock(blockHash);
            }
            catch (Exception e)
            {
                err = ExceptionToErrorCode.Translate(e);
            }
            finally
            {
                onComplete(storedBlock, err);
            }
        });
        return 0;
    }

    public ErrorCodesEnum GetExistingContent(ReadOnlySpan<ulong> blockHashes, uint minBlockUsagePercent, Action<StoreIndex?, ErrorCodesEnum> asyncCompleteApi)
    {
        var hashes = blockHashes.ToArray();
        Task.Run(async () =>
        {
            StoreIndex? storeIndex = null;
            var err = ErrorCodesEnum.SUCCESS;
            try
            {
                storeIndex = await _blockstore.GetExistingContent(hashes, minBlockUsagePercent);
            }
            catch (Exception e)
            {
                err = ExceptionToErrorCode.Translate(e);
            }
            finally
            {
                asyncCompleteApi(storeIndex, err);
            }
        });
        return 0;
    }

    public ErrorCodesEnum PruneBlocks(ReadOnlySpan<ulong> blockKeepHashes, Action<uint, ErrorCodesEnum> asyncCompleteApi)
    {
        var hashes = blockKeepHashes.ToArray();
        Task.Run(async () =>
        {
            var prunedBlocks = 0u;
            var err = ErrorCodesEnum.SUCCESS;
            try
            {
                prunedBlocks = await _blockstore.PruneBlocks(hashes);
            }
            catch (Exception e)
            {
                err = ExceptionToErrorCode.Translate(e);
            }
            finally
            {
                asyncCompleteApi(prunedBlocks, err);
            }
        });
        return 0;
    }

    public BlockstoreStats? GetStats() => _blockstore.GetStats();

    public ErrorCodesEnum Flush(Action<ErrorCodesEnum> asyncCompleteApi)
    {
        Task.Run(async () =>
        {
            var err = ErrorCodesEnum.SUCCESS;
            try
            {
                await _blockstore.Flush();
            }
            catch (Exception e)
            {
                err = ExceptionToErrorCode.Translate(e);
            }
            finally
            {
                asyncCompleteApi(err);
            }
        });
        return 0;
    }

    public void OnDispose() => _blockstore.OnDispose();
}