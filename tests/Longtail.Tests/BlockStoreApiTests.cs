using NSubstitute;
using NUnit.Framework;

namespace Longtail.Tests;

internal class BlockStoreApiTests
{
    private IBlockstore _blockStore;

    [SetUp]
    public void SetUp()
    {
        _blockStore = Substitute.For<IBlockstore>();
    }

    [Test]
    public void Dispose_Always_CallOnDispose()
    {
        var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        api.Dispose();

        _blockStore.Received(1).OnDispose();
    }

    [Test]
    public void Flush_OnSuccess_CallFlushCallback()
    {
        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        _blockStore
            .When(blockStore => blockStore.Flush(Arg.Any<Action<ErrorCodesEnum>>()))
            .Do(a => a.Arg<Action<ErrorCodesEnum>>().Invoke(ErrorCodesEnum.SUCCESS));

        var result = api.Flush();

        Assert.That(result, Is.EqualTo(ErrorCodesEnum.SUCCESS));
    }

    [Test]
    public void Flush_OnError_ReturnError()
    {
        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        _blockStore
            .When(blockStore => blockStore.Flush(Arg.Any<Action<ErrorCodesEnum>>()))
            .Do(a => a.Arg<Action<ErrorCodesEnum>>().Invoke(ErrorCodesEnum.ENOMEM));

        var result = api.Flush();

        Assert.That(result, Is.EqualTo(ErrorCodesEnum.ENOMEM));
    }
}