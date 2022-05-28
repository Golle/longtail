using NUnit.Framework;

namespace Longtail.Tests;

internal unsafe class StructSizeTests
{
    [Test]
    public void GetCancelApiSize_Always_ReturnSameSizeAsCSharp()
    {
        var size = (ulong)sizeof(Longtail_CancelAPI);

        var result = LongtailLibrary.Longtail_GetCancelAPISize();

        Assert.That(result, Is.EqualTo(size));
    }
    
    [Test]
    public void GetCompressionApiSize_Always_ReturnSameSizeAsCSharp()
    {
        var size = (ulong)sizeof(Longtail_CompressionAPI);

        var result = LongtailLibrary.Longtail_GetCompressionAPISize();

        Assert.That(result, Is.EqualTo(size));
    }

    [Test]
    public void GetJobApiSize_Always_ReturnSameSizeAsCSharp()
    {
        var size = (ulong)sizeof(Longtail_JobAPI);

        var result = LongtailLibrary.Longtail_GetJobAPISize();

        Assert.That(result, Is.EqualTo(size));
    }

    [Test]
    public void GetProgressApiSize_Always_ReturnSameSizeAsCSharp()
    {
        var size = (ulong)sizeof(Longtail_ProgressAPI);

        var result = LongtailLibrary.Longtail_GetProgressAPISize();

        Assert.That(result, Is.EqualTo(size));
    }
    
    [Test]
    public void GetChunkerApiSize_Always_ReturnSameSizeAsCSharp()
    {
        var size = (ulong)sizeof(Longtail_ChunkerAPI);

        var result = LongtailLibrary.Longtail_GetChunkerAPISize();

        Assert.That(result, Is.EqualTo(size));
    }
    
    [Test]
    public void GetHashRegistrySize_Always_ReturnSameSizeAsCSharp()
    {
        var size = (ulong)sizeof(Longtail_HashRegistryAPI);

        var result = LongtailLibrary.Longtail_GetHashRegistrySize();

        Assert.That(result, Is.EqualTo(size));
    }
    
    [Test]
    public void GetHashApiSize_Always_ReturnSameSizeAsCSharp()
    {
        var size = (ulong)sizeof(Longtail_HashAPI);

        var result = LongtailLibrary.Longtail_GetHashAPISize();

        Assert.That(size, Is.EqualTo(result));
    }
    
    [Test]
    public void GetCompressionRegistryApiSize_Always_ReturnSameSizeAsCSharp()
    {
        var size = (ulong)sizeof(Longtail_CompressionRegistryAPI);

        var result = LongtailLibrary.Longtail_GetCompressionRegistryAPISize();

        Assert.That(result, Is.EqualTo(size));
    }

    [Test]
    public void GetStorageApiSize_Always_ReturnSameSizeAsCSharp()
    {
        var size = (ulong)sizeof(Longtail_StorageAPI);

        var result = LongtailLibrary.Longtail_GetStorageAPISize();

        Assert.That(result, Is.EqualTo(size));
    }

    [Test]
    public void GetAsyncPutStoredBlockAPISize_Always_ReturnSameSizeAsCSharp()
    {
        var size = (ulong)sizeof(Longtail_AsyncPutStoredBlockAPI);

        var result = LongtailLibrary.Longtail_GetAsyncPutStoredBlockAPISize();

        Assert.That(result, Is.EqualTo(size));
    }

    [Test]
    public void GetAsyncGetStoredBlockAPISize_Always_ReturnSameSizeAsCSharp()
    {
        var size = (ulong)sizeof(Longtail_AsyncGetStoredBlockAPI);

        var result = LongtailLibrary.Longtail_GetAsyncGetStoredBlockAPISize();

        Assert.That(result, Is.EqualTo(size));
    }

    [Test]
    public void GetAsyncGetExistingContentAPISize_Always_ReturnSameSizeAsCSharp()
    {
        var size = (ulong)sizeof(Longtail_AsyncGetExistingContentAPI);

        var result = LongtailLibrary.Longtail_GetAsyncGetExistingContentAPISize();

        Assert.That(result, Is.EqualTo(size));
    }

    [Test]
    public void GetAsyncPruneBlocksAPISize_Always_ReturnSameSizeAsCSharp()
    {
        var size = (ulong)sizeof(Longtail_AsyncPruneBlocksAPI);

        var result = LongtailLibrary.Longtail_GetAsyncPruneBlocksAPISize();

        Assert.That(result, Is.EqualTo(size));
    }

    [Test]
    public void GetAsyncPreflightStartedAPISize_Always_ReturnSameSizeAsCSharp()
    {
        var size = (ulong)sizeof(Longtail_AsyncPreflightStartedAPI);

        var result = LongtailLibrary.Longtail_GetAsyncPreflightStartedAPISize();

        Assert.That(result, Is.EqualTo(size));
    }

    [Test]
    public void GetAsyncFlushAPISize_Always_ReturnSameSizeAsCSharp()
    {
        var size = (ulong)sizeof(Longtail_AsyncFlushAPI);

        var result = LongtailLibrary.Longtail_GetAsyncFlushAPISize();

        Assert.That(result, Is.EqualTo(size));
    }

    [Test]
    public void GetBlockStoreAPISize_Always_ReturnSameSizeAsCSharp()
    {
        var size = (ulong)sizeof(Longtail_BlockStoreAPI);

        var result = LongtailLibrary.Longtail_GetBlockStoreAPISize();

        Assert.That(result, Is.EqualTo(size));
    }
    
    [Test]
    public void GetPathFilterAPISize_Always_ReturnSameSizeAsCSharp()
    {
        var size = (ulong)sizeof(Longtail_PathFilterAPI);

        var result = LongtailLibrary.Longtail_GetPathFilterAPISize();

        Assert.That(result, Is.EqualTo(size));
    }
}