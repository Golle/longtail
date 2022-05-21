using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Longtail.Internal;

namespace Longtail;

public unsafe class BlockStoreApi : IDisposable
{
    private Longtail_BlockStoreAPI* _blockStore;
    private BlockStoreApi(Longtail_BlockStoreAPI* blockstoreApi)
    {
        _blockStore = blockstoreApi;
    }

    public ErrorCodesEnum PutStoredBlock(StoredBlock storedBlock /*, Longtail_AsyncPutStoredBlockAPI* async_complete_api*/)
    {
        //LongtailLibrary.Longtail_BlockStore_PutStoredBlock(_blockStore, storedBlock.AsPointer(), )
        return ErrorCodesEnum.SUCCESS;
    }
    public ErrorCodesEnum PreflightGet(uint chunk_count, ulong* chunk_hashes/*, Longtail_AsyncPreflightStartedAPI* optional_async_complete_api*/)
    {
        return ErrorCodesEnum.SUCCESS;
    }
    public ErrorCodesEnum GetStoredBlock(ulong block_hash/*, Longtail_AsyncGetStoredBlockAPI* async_complete_api*/)
    {
        return ErrorCodesEnum.SUCCESS;
    }
    public ErrorCodesEnum GetExistingContent(uint chunk_count, ulong* chunk_hashes, uint min_block_usage_percent/*, Longtail_AsyncGetExistingContentAPI* async_complete_api*/)
    {
        return ErrorCodesEnum.SUCCESS;
    }
    public ErrorCodesEnum PruneBlocks(uint block_keep_count, ulong* block_keep_hashes/*, Longtail_AsyncPruneBlocksAPI* async_complete_api*/)
    {
        return ErrorCodesEnum.SUCCESS;
    }
    public BlockstoreStats GetStats(/*Longtail_BlockStore_Stats* out_stats*/)
    {
        Longtail_BlockStore_Stats stats;
        var err = LongtailLibrary.Longtail_BlockStore_GetStats(_blockStore, &stats);
        if (err != 0)
        {
            throw new LongtailException(nameof(BlockStoreApi), nameof(GetStats), nameof(LongtailLibrary.Longtail_BlockStore_GetStats), err);
        }
        return new BlockstoreStats(stats);
    }

    public Task<ErrorCodesEnum> Flush() =>
        Task.Run(() =>
        {
            using var flushApi = new AsyncFlushApi();
            var err = LongtailLibrary.Longtail_BlockStore_Flush(_blockStore, flushApi);
            if (err == 0)
            {
                flushApi.Wait();
                return flushApi.Err;
            }
            return (ErrorCodesEnum)err;
        });

    public static BlockStoreApi MakeBlockStoreApi(IBlockstore blockstore)
    {
        using var name = new Utf8String(nameof(BlockStoreApi));
        var mem = LongtailLibrary.Longtail_Alloc(name, (ulong)sizeof(BlockStoreAPIInternal));
        if (mem == null)
        {
            throw new LongtailException(nameof(BlockStoreApi), nameof(MakeBlockStoreApi), nameof(LongtailLibrary.Longtail_Alloc), ErrorCodes.ENOMEM);
        }
        mem = LongtailLibrary.Longtail_MakeBlockStoreAPI(
            mem,
            &DisposeFunc,
            &PutStoredBlockFunc,
            &PreflightGetFunc,
            &GetStoredBlockFunc,
            &GetExistingContentFunc,
            &PruneBlocksFunc,
            &GetStatsFunc,
            &FlushFunc
        );
        if (mem == null)
        {
            throw new LongtailException(nameof(BlockStoreApi), nameof(MakeBlockStoreApi), nameof(LongtailLibrary.Longtail_MakeBlockStoreAPI), ErrorCodes.ENOMEM);
        }

        var blockstoreApi = (BlockStoreAPIInternal*)mem;
        blockstoreApi->Handle = GCHandle.Alloc(blockstore);
        return new BlockStoreApi((Longtail_BlockStoreAPI*)blockstoreApi);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct BlockStoreAPIInternal
    {
        public Longtail_BlockStoreAPI BlockStoreApi;
        public GCHandle Handle;
    }

    //NOTE(Jens): we're currently not handling exceptions.
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static int PutStoredBlockFunc(Longtail_BlockStoreAPI* blockStoreApi, Longtail_StoredBlock* storedBlock, Longtail_AsyncPutStoredBlockAPI* asyncCompleteApi)
    {
        var result = GetInterface(blockStoreApi)
            .PutStoredBlock(new StoredBlock(storedBlock, false), err =>
            {
                if (asyncCompleteApi != null)
                {
                    asyncCompleteApi->OnComplete(asyncCompleteApi, (int)err);
                }
            });
        return (int)result;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static int PreflightGetFunc(Longtail_BlockStoreAPI* blockStoreApi, uint blockCount, ulong* blockHashes, Longtail_AsyncPreflightStartedAPI* optionalAsyncCompleteApi)
    {
        var result = GetInterface(blockStoreApi)
            .PreflightGet(new ReadOnlySpan<ulong>(blockHashes, (int)blockCount), err =>
            {
                if (optionalAsyncCompleteApi != null)
                {
                    // NOTE(Jens): not sure if you're allowed to modify the contents of blockhashes or change the amount. if that's the case this wont work.
                    optionalAsyncCompleteApi->OnComplete(optionalAsyncCompleteApi, blockCount, blockHashes, (int)err);
                }
            });
        return (int)result;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static int GetStoredBlockFunc(Longtail_BlockStoreAPI* blockStoreApi, ulong blockHash, Longtail_AsyncGetStoredBlockAPI* asyncCompleteApi)
    {
        var result = GetInterface(blockStoreApi)
            .GetStoredBlock(blockHash, (storedBlock, err) =>
            {
                var storedBlockApi = storedBlock != null ? storedBlock.AsPointer() : null;
                asyncCompleteApi->OnComplete(asyncCompleteApi, storedBlockApi, (int)err);
            });
        return (int)result;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static int GetExistingContentFunc(Longtail_BlockStoreAPI* blockStoreApi, uint chunkCount, ulong* chunkHashes, uint minBlockUsagePercent, Longtail_AsyncGetExistingContentAPI* asyncCompleteApi)
    {
        var result = GetInterface(blockStoreApi)
            .GetExistingContentFunc(new ReadOnlySpan<ulong>(chunkHashes, (int)chunkCount), minBlockUsagePercent, (storeIndex, err) =>
            {
                var storeIndexPointer = storeIndex != null ? storeIndex.AsPointer() : null;
                asyncCompleteApi->OnComplete(asyncCompleteApi, storeIndexPointer, (int)err);
            });
        return (int)result;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static int PruneBlocksFunc(Longtail_BlockStoreAPI* blockStoreApi, uint blockKeepCount, ulong* blockKeepHashes, Longtail_AsyncPruneBlocksAPI* asyncCompleteApi)
    {
        var result = GetInterface(blockStoreApi)
            .PruneBlocks(new ReadOnlySpan<ulong>(blockKeepHashes, (int)blockKeepHashes), (prunedBlockCount, err) =>
            {
                asyncCompleteApi->OnComplete(asyncCompleteApi, prunedBlockCount, (int)err);
            });
        return (int)result;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static int GetStatsFunc(Longtail_BlockStoreAPI* blockStoreApi, Longtail_BlockStore_Stats* outStats)
    {
        var result = GetInterface(blockStoreApi)
            .GetStats();
        if (result != null)
        {
            *outStats = result.Internal;
        }
        return 0;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static int FlushFunc(Longtail_BlockStoreAPI* blockStoreApi, Longtail_AsyncFlushAPI* asyncCompleteApi)
    {
        var result = GetInterface(blockStoreApi)
            .Flush(err => asyncCompleteApi->OnComplete(asyncCompleteApi, (int)err));
        return (int)result;
    }

    private static IBlockstore GetInterface(Longtail_BlockStoreAPI* api)
    {
        var blockstoreApi = (BlockStoreAPIInternal*)api;
        // NOTE(Jens): if blockstore is null this will crash.
        return (IBlockstore)blockstoreApi->Handle.Target!;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void DisposeFunc(Longtail_API* api)
    {
        var blockstoreApi = (BlockStoreAPIInternal*)api;
        if (blockstoreApi->Handle.IsAllocated)
        {
            blockstoreApi->Handle.Free();
        }
        LongtailLibrary.Longtail_Free(api);
    }

    public void Dispose()
    {
        if (_blockStore != null)
        {
            LongtailLibrary.Longtail_DisposeAPI(&_blockStore->m_API);
            _blockStore = null;
        }
    }
}