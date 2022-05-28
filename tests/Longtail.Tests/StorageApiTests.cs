using NUnit.Framework;

namespace Longtail.Tests;

internal class StorageApiTests
{
    [Test]
    public void CreateInMemoryStorageAPI_OnSuccess_ReturnStorageApi()
    {
        using var api = StorageApi.CreateInMemoryStorageAPI();

        Assert.That(api, Is.Not.Null);
    }

    [Test]
    public void CreateFSStorageAPI_OnSuccess_ReturnStorageApi()
    {
        using var api = StorageApi.CreateFSStorageAPI();

        Assert.That(api, Is.Not.Null);
    }
}