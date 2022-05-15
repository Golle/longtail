using NUnit.Framework;

namespace Longtail.Tests;

internal class HashRegistryTests
{
    [Test]
    public void HashRegistry_Blake3_CreateAndDispose()
    {
        using var hashRegistry = HashRegistry.CreateBlake3HashRegistry();

        Assert.That(hashRegistry, Is.Not.Null);
    }

    [Test]
    public void HashRegistry_Full_CreateAndDispose()
    {
        using var hashRegistry = HashRegistry.CreateFullHashRegistry();

        Assert.That(hashRegistry, Is.Not.Null);
    }

    [Test]
    public void GetHashApi_Blake2_ReturnHashApi()
    {
        using var hashRegistry = HashRegistry.CreateFullHashRegistry();
        var hashType = LongtailLibrary.Longtail_GetBlake2HashType();
        using var hashApi = hashRegistry?.GetHashApi(hashType);

        Assert.That(hashApi, Is.Not.Null);
    }

    [Test]
    public void GetHashApi_Blake3_ReturnHashApi()
    {
        using var hashRegistry = HashRegistry.CreateFullHashRegistry();

        var type = LongtailLibrary.Longtail_GetBlake3HashType();
        using var hashApi = hashRegistry?.GetHashApi(type);

        Assert.That(hashApi, Is.Not.Null);
    }

    [Test]
    public void GetHashApi_Meow_ReturnHashApi()
    {
        using var hashRegistry = HashRegistry.CreateFullHashRegistry();
        var hashType = LongtailLibrary.Longtail_GetMeowHashType();
        using var hashApi = hashRegistry?.GetHashApi(hashType);

        Assert.That(hashApi, Is.Not.Null);
    }
}