using NUnit.Framework;

namespace Longtail.Tests;

internal class BlockStoreApiTests
{
    private BlockStoreMock _blockStore;

    [SetUp]
    public void SetUp()
    {
        // NOTE(Jens): NSubsitite does not work well with Span and ReadOnlySpans, we can't mock those methods.
        _blockStore = new BlockStoreMock();
    }

    [Test]
    public void Dispose_Always_CallOnDispose()
    {
        var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        api.Dispose();

        Assert.That(_blockStore.Disposed, Is.True);
    }

    [Test]
    public void Flush_OnSuccess_CallFlushCallback()
    {
        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        api.Flush();

        Assert.That(_blockStore.Flushed, Is.True);
    }

    [Test]
    public void Flush_OnError_ThrowException()
    {
        _blockStore.Err = ErrorCodesEnum.ENOMEM;
        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        var result = Assert.Catch<LongtailException>(() => api.Flush())!;

        Assert.That(result.Err, Is.EqualTo(ErrorCodes.ENOMEM));
    }

    [Test]
    public void GetStats_NullStats_ThrowsException()
    {
        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);
        _blockStore.Stats = null;

        var result = Assert.Throws<LongtailException>(() => api.GetStats())!;

        Assert.That(result.Err, Is.EqualTo(ErrorCodes.ENOTSUP));
    }


    [TestCase(BlockStoreApiStats.Flush_Count)]
    [TestCase(BlockStoreApiStats.GetExistingContent_RetryCount)]
    [TestCase(BlockStoreApiStats.GetStoredBlock_FailCount)]
    [TestCase(BlockStoreApiStats.PutStoredBlock_Chunk_Count)]
    public void GetStats_WithStatus_ReturnStats(BlockStoreApiStats index)
    {
        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);
        _blockStore.Stats = new BlockstoreStats
        {
            [index] = 4
        };

        var result = api.GetStats();

        Assert.That(result.Get(index), Is.EqualTo(4));
    }

    [Test]
    public void PruneBlocks_OnSuccess_ReturnPrunedBlocksCount()
    {
        _blockStore.PrunedBlocks = 1;
        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        var result = api.PruneBlocks(ReadOnlySpan<ulong>.Empty);

        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void PruneBlocks_WithBlocks_PassKeepHashesToBlockStore()
    {
        var blocks = new ulong[] { 1, 2, 3 };
        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        api.PruneBlocks(blocks);

        Assert.That(_blockStore.BlockKeepHashes, Is.EqualTo(blocks));
    }

    [Test]
    public void PruneBlocks_OnError_ThrowException()
    {
        _blockStore.Err = ErrorCodesEnum.ENOMEM;
        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        var result = Assert.Catch<LongtailException>(() => api.PruneBlocks(ReadOnlySpan<ulong>.Empty))!;

        Assert.That(result.Err, Is.EqualTo(ErrorCodes.ENOMEM));
    }


    [Test]
    public unsafe void GetExistingContent_OnSuccess_ReturnStoreIndex()
    {
        _blockStore.StoredIndex = new StoreIndex((Longtail_StoreIndex*)10);
        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        var storeIndex = api.GetExistingContent(ReadOnlySpan<ulong>.Empty, 0);

        Assert.That(storeIndex, Is.Not.Null);
    }

    [Test]
    public unsafe void GetExistingContent_OnSuccess_PassMinBlockUsagePercent()
    {
        _blockStore.StoredIndex = new StoreIndex((Longtail_StoreIndex*)10);
        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        api.GetExistingContent(ReadOnlySpan<ulong>.Empty, 1);

        Assert.That(_blockStore.MinBlockUsagePercent, Is.EqualTo(1));
    }

    [Test]
    public unsafe void GetExistingContent_OnSuccess_PassBlockHashes()
    {
        var blockHashes = new ulong[] { 1, 2, 3 };
        _blockStore.StoredIndex = new StoreIndex((Longtail_StoreIndex*)10);

        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        api.GetExistingContent(blockHashes, 1);

        Assert.That(_blockStore.BlockHashes, Is.EqualTo(blockHashes));
    }

    [Test]
    public void GetExistingContent_OnErrro_ThrowException()
    {
        _blockStore.Err = ErrorCodesEnum.ENOMEM;
        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        var result = Assert.Catch<LongtailException>(() => api.GetExistingContent(ReadOnlySpan<ulong>.Empty, 0))!;

        Assert.That(result.Err, Is.EqualTo(ErrorCodes.ENOMEM));
    }


    [Test]
    public unsafe void GetStoredBlock_OnSuccess_ReturnStoredBlock()
    {
        _blockStore.StoredBlock = new StoredBlock((Longtail_StoredBlock*)10);
        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        var storedBlock = api.GetStoredBlock(0);

        Assert.That(storedBlock, Is.Not.Null);
    }

    [Test]
    public void PreflightGet_OnSuccess_PassBlockHashes()
    {
        var blockHashes = new ulong[] { 1, 2, 3 };
        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        api.PreflightGet(blockHashes);

        Assert.That(_blockStore.BlockHashes, Is.EqualTo(blockHashes));
    }

    [Test]
    public void PreflightGet_OnError_ThrowException()
    {
        _blockStore.Err = ErrorCodesEnum.ENOMEM;
        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        var result = Assert.Catch<LongtailException>(() => api.PreflightGet(ReadOnlySpan<ulong>.Empty))!;

        Assert.That(result.Err, Is.EqualTo(ErrorCodes.ENOMEM));
    }

    [Test]
    public unsafe void PutStoredBlock_OnSuccess_SetStoredBlock()
    {
        var storedBlock = new StoredBlock((Longtail_StoredBlock*)10);
        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        api.PutStoredBlock(storedBlock);

        Assert.That((nuint)_blockStore.StoredBlock.AsPointer(), Is.EqualTo((nuint)storedBlock.AsPointer()));
    }

    [Test]
    public unsafe void PutStoredBlock_OnError_ThrowException()
    {
        var storedBlock = new StoredBlock((Longtail_StoredBlock*)10);
        _blockStore.Err = ErrorCodesEnum.ENOMEM;
        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        var result = Assert.Catch<LongtailException>(() => api.PutStoredBlock(storedBlock));

        Assert.That(result.Err, Is.EqualTo(ErrorCodes.ENOMEM));
    }

    [Test]
    public void CreateFSStorageApi_OnSuccess_ReturnStorageApi()
    {
        using var jobApi = JobApi.CreateBikeshedJobAPI(1);
        using var storageApi = StorageApi.CreateFSStorageAPI();

        using var api = BlockStoreApi.CreateFSBlockStoreApi(jobApi, storageApi, Path.GetTempPath(), null, false);

        Assert.That(api, Is.Not.Null);
    }
}


internal class BlockStoreMock : IBlockstore
{
    public uint PrunedBlocks { get; set; }
    public ErrorCodesEnum Err { get; set; }
    public ulong[] BlockKeepHashes { get; set; }
    public BlockstoreStats Stats { get; set; }
    public StoredBlock StoredBlock { get; set; }
    public StoreIndex StoredIndex { get; set; }
    public ulong[] BlockHashes { get; private set; }
    public uint MinBlockUsagePercent { get; private set; }
    public ulong BlockHash { get; private set; }
    public bool Disposed { get; private set; }
    public bool Flushed { get; private set; }


    public ErrorCodesEnum PutStoredBlock(StoredBlock storedBlock, Action<ErrorCodesEnum> onCompleteCallback)
    {
        StoredBlock = storedBlock;
        onCompleteCallback(Err);
        return Err;
    }

    public ErrorCodesEnum PreflightGet(ReadOnlySpan<ulong> blockHashes, Action<ErrorCodesEnum> optionalOnComplete)
    {
        BlockHashes = blockHashes.ToArray();
        optionalOnComplete(Err);
        return Err;
    }

    public ErrorCodesEnum GetStoredBlock(ulong blockHash, Action<StoredBlock, ErrorCodesEnum> onComplete)
    {
        BlockHash = blockHash;
        onComplete(StoredBlock, Err);
        return Err;
    }

    public ErrorCodesEnum GetExistingContent(ReadOnlySpan<ulong> blockHashes, uint minBlockUsagePercent, Action<StoreIndex, ErrorCodesEnum> asyncCompleteApi)
    {
        BlockHashes = blockHashes.ToArray();
        MinBlockUsagePercent = minBlockUsagePercent;
        asyncCompleteApi(StoredIndex, Err);
        return Err;
    }

    public ErrorCodesEnum PruneBlocks(ReadOnlySpan<ulong> blockKeepHashes, Action<uint, ErrorCodesEnum> asyncCompleteApi)
    {
        BlockKeepHashes = blockKeepHashes.ToArray();
        asyncCompleteApi(PrunedBlocks, Err);
        return Err;
    }

    public BlockstoreStats GetStats() => Stats;
    public ErrorCodesEnum Flush(Action<ErrorCodesEnum> asyncCompleteApi)
    {
        Flushed = true;
        asyncCompleteApi(Err);
        return Err;
    }
    public void OnDispose() => Disposed = true;
}