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
}


internal class BlockStoreMock : IBlockstore
{
    public uint PrunedBlocks { get; set; }
    public ErrorCodesEnum Err { get; set; }
    public ulong[] BlockKeepHashes { get; set; }
    public BlockstoreStats Stats { get; set; }
    public bool Disposed { get; private set; }
    public bool Flushed { get; private set; }

    public ErrorCodesEnum PutStoredBlock(StoredBlock storedBlock, Action<ErrorCodesEnum> onCompleteCallback)
    {
        throw new NotImplementedException();
    }

    public ErrorCodesEnum PreflightGet(ReadOnlySpan<ulong> blockHashes, Action<ErrorCodesEnum> optionalOnComplete)
    {
        throw new NotImplementedException();
    }

    public ErrorCodesEnum GetStoredBlock(ulong blockHash, Action<StoredBlock, ErrorCodesEnum> onComplete)
    {
        throw new NotImplementedException();
    }

    public ErrorCodesEnum GetExistingContentFunc(ReadOnlySpan<ulong> blockHashes, uint minBlockUsagePercent, Action<StoreIndex, ErrorCodesEnum> asyncCompleteApi)
    {
        throw new NotImplementedException();
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