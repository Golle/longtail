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

    internal Longtail_BlockStoreAPI* AsPointer() => _blockStore;

    // NOTE(Jens): most of these methods have an optional async API, we make it mandatory. This is not the way they should be implemented.
    // NOTE(Jens): These will most likely never (or rarely) be called by users of the lib, so we might not need them at all (make them internal and use for testing only?)
    public void PutStoredBlock(StoredBlock storedBlock)
    {
        using var storedBlockApi = new AsyncPutStoredBlockAPI();
        var err = LongtailLibrary.Longtail_BlockStore_PutStoredBlock(_blockStore, storedBlock.AsPointer(), storedBlockApi);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_BlockStore_PutStoredBlock), err);
        }
        storedBlockApi.Wait();
        if (storedBlockApi.Err != ErrorCodesEnum.SUCCESS)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_AsyncPutStoredBlock_OnComplete), storedBlockApi.Err);
        }
    }

    public void PreflightGet(ReadOnlySpan<ulong> chunkHashes)
    {
        using var preflightApi = new AsyncPreflightStartedAPI();
        fixed (ulong* pChunkHashes = chunkHashes)
        {
            var err = LongtailLibrary.Longtail_BlockStore_PreflightGet(_blockStore, (uint)chunkHashes.Length, pChunkHashes, preflightApi);
            if (err != 0)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_BlockStore_PreflightGet), err);
            }
            preflightApi.Wait();
            if (preflightApi.Err != ErrorCodesEnum.SUCCESS)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_AsyncPreflightStarted_OnComplete), preflightApi.Err);
            }
        }
    }

    // TODO: investigate how to properly make an async api for these functions
    public Task<StoredBlock?> GetStoredBlockAsync(ulong blockHash)
        => Task.Run(() => GetStoredBlock(blockHash));

    public StoredBlock? GetStoredBlock(ulong blockHash)
    {
        using var storedBlockApi = new AsyncGetStoredBlockAPI();

        var err = LongtailLibrary.Longtail_BlockStore_GetStoredBlock(_blockStore, blockHash, storedBlockApi);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_BlockStore_GetStoredBlock), err);
        }
        storedBlockApi.Wait();
        if (storedBlockApi.Err != ErrorCodesEnum.SUCCESS)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_BlockStore_GetStoredBlock), storedBlockApi.Err);
        }
        return storedBlockApi.StoredBlock != null ? new StoredBlock(storedBlockApi.StoredBlock, false) : null;
    }

    public Task<StoreIndex?> GetExistingContentAsync(ReadOnlyMemory<ulong> chunkHashes, uint minBlockUsagePercent = 0) 
        => Task.Run(() => GetExistingContent(chunkHashes.Span, minBlockUsagePercent));
    
    public StoreIndex? GetExistingContent(ReadOnlySpan<ulong> chunkHashes, uint minBlockUsagePercent = 0)
    {
        using var contentApi = new AsyncGetExistingContentAPI();
        fixed (ulong* pChunkHashes = chunkHashes)
        {
            var err = LongtailLibrary.Longtail_BlockStore_GetExistingContent(_blockStore, (uint)chunkHashes.Length, pChunkHashes, minBlockUsagePercent, contentApi);
            if (err != 0)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_BlockStore_GetExistingContent), err);
            }
            contentApi.Wait();
            if (contentApi.Err != ErrorCodesEnum.SUCCESS)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_AsyncGetExistingContent_OnComplete), contentApi.Err);
            }
            return contentApi.StoreIndex != null ? new StoreIndex(contentApi.StoreIndex, false) : null;
        }
    }

    public uint PruneBlocks(ReadOnlySpan<ulong> blockKeepHashes)
    {
        using var pruneApi = new AsyncPruneBlocksAPI();
        fixed (ulong* pBlockHashes = blockKeepHashes)
        {
            var err = LongtailLibrary.Longtail_BlockStore_PruneBlocks(_blockStore, (uint)blockKeepHashes.Length, pBlockHashes, pruneApi);
            if (err != 0)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_BlockStore_PruneBlocks), err);
            }
            pruneApi.Wait();
            if (pruneApi.Err != ErrorCodesEnum.SUCCESS)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_AsyncPruneBlocks_OnComplete), err);
            }
            return pruneApi.PrunedBlockCount;
        }
    }

    public BlockstoreStats GetStats()
    {
        Longtail_BlockStore_Stats stats;
        var err = LongtailLibrary.Longtail_BlockStore_GetStats(_blockStore, &stats);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_BlockStore_GetStats), err);
        }

        return new BlockstoreStats(stats);
    }

    public void Flush()
    {
        using var flushApi = new AsyncFlushApi();
        var err = LongtailLibrary.Longtail_BlockStore_Flush(_blockStore, flushApi);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_BlockStore_Flush), err);
        }
        flushApi.Wait();
        if (flushApi.Err != ErrorCodesEnum.SUCCESS)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_BlockStore_Flush), flushApi.Err);
        }
    }

    public static BlockStoreApi CreateFSBlockStoreApi(JobApi jobApi, StorageApi storageApi, string path, string? optionalExtension = null, bool enableFileMapping = false)
    {
        using var contentPath = new Utf8String(path);
        using var fileExtension = optionalExtension != null ? new Utf8String(optionalExtension) : default;
        var api = LongtailLibrary.Longtail_CreateFSBlockStoreAPI(jobApi.AsPointer(), storageApi.AsPointer(), contentPath, fileExtension, enableFileMapping ? 1 : 0);
        if (api == null)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_CreateFSBlockStoreAPI), ErrorCodesEnum.ENOMEM);
        }
        return new BlockStoreApi(api);
    }

    public static BlockStoreApi MakeBlockStoreApi(IBlockstore blockstore)
    {
        using var name = new Utf8String(nameof(BlockStoreApi));
        var mem = LongtailLibrary.Longtail_Alloc(name, (ulong)sizeof(BlockStoreAPIInternal));
        if (mem == null)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_Alloc), ErrorCodes.ENOMEM);
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
            throw new LongtailException(nameof(LongtailLibrary.Longtail_MakeBlockStoreAPI), ErrorCodes.ENOMEM);
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
                // NOTE(Jens): not sure if you're allowed to modify the contents of blockhashes or change the amount. if that's the case this wont work.
                if (optionalAsyncCompleteApi != null)
                {
                    LongtailLibrary.Longtail_AsyncPreflightStarted_OnComplete(optionalAsyncCompleteApi, blockCount, blockHashes, (int)err);
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
                LongtailLibrary.Longtail_AsyncGetStoredBlock_OnComplete(asyncCompleteApi, storedBlockApi, (int)err);
            });
        return (int)result;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static int GetExistingContentFunc(Longtail_BlockStoreAPI* blockStoreApi, uint chunkCount, ulong* chunkHashes, uint minBlockUsagePercent, Longtail_AsyncGetExistingContentAPI* asyncCompleteApi)
    {
        var result = GetInterface(blockStoreApi)
            .GetExistingContent(new ReadOnlySpan<ulong>(chunkHashes, (int)chunkCount), minBlockUsagePercent, (storeIndex, err) =>
            {
                var storeIndexPointer = storeIndex != null ? storeIndex.AsPointer() : null;
                LongtailLibrary.Longtail_AsyncGetExistingContent_OnComplete(asyncCompleteApi, storeIndexPointer, (int)err);
            });
        return (int)result;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static int PruneBlocksFunc(Longtail_BlockStoreAPI* blockStoreApi, uint blockKeepCount, ulong* blockKeepHashes, Longtail_AsyncPruneBlocksAPI* asyncCompleteApi)
    {
        var result = GetInterface(blockStoreApi)
            .PruneBlocks(new ReadOnlySpan<ulong>(blockKeepHashes, (int)blockKeepCount), (prunedBlockCount, err) =>
            {
                LongtailLibrary.Longtail_AsyncPruneBlocks_OnComplete(asyncCompleteApi, prunedBlockCount, (int)err);
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
            return 0;
        }
        return ErrorCodes.ENOTSUP;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static int FlushFunc(Longtail_BlockStoreAPI* blockStoreApi, Longtail_AsyncFlushAPI* asyncCompleteApi)
    {
        var result = GetInterface(blockStoreApi)
            .Flush(err => LongtailLibrary.Longtail_AsyncFlush_OnComplete(asyncCompleteApi, (int)err));
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
            (blockstoreApi->Handle.Target as IBlockstore)?.OnDispose();
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