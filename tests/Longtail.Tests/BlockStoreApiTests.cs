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
    public async Task Flush_OnSuccess_CallFlushCallback()
    {
        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        _blockStore
            .When(blockStore => blockStore.Flush(Arg.Any<Action<ErrorCodesEnum>>()))
            .Do(a => a.Arg<Action<ErrorCodesEnum>>().Invoke(ErrorCodesEnum.SUCCESS));

        await api.Flush();
    }

    [Test]
    public void Flush_OnError_ThrowException()
    {
        using var api = BlockStoreApi.MakeBlockStoreApi(_blockStore);

        _blockStore
            .When(blockStore => blockStore.Flush(Arg.Any<Action<ErrorCodesEnum>>()))
            .Do(a => a.Arg<Action<ErrorCodesEnum>>().Invoke(ErrorCodesEnum.ENOMEM));

        var result = Assert.CatchAsync<LongtailException>(async () => await api.Flush());

        Assert.That(result.Err, Is.EqualTo((int)ErrorCodesEnum.ENOMEM));
    }
}