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

        using var hashApi = hashRegistry?.GetHashApi(HashTypes.Blake2)!;

        Assert.That(hashApi?.GetIdentifier(), Is.EqualTo(HashTypes.Blake2));
    }

    [Test]
    public void GetHashApi_Blake3_ReturnHashApi()
    {
        using var hashRegistry = HashRegistry.CreateFullHashRegistry();

        using var hashApi = hashRegistry?.GetHashApi(HashTypes.Blake3);

        Assert.That(hashApi?.GetIdentifier(), Is.EqualTo(HashTypes.Blake3));
    }

    [Test]
    public void GetHashApi_Meow_ReturnHashApi()
    {
        using var hashRegistry = HashRegistry.CreateFullHashRegistry();

        using var hashApi = hashRegistry?.GetHashApi(HashTypes.Meow);

        Assert.That(hashApi?.GetIdentifier(), Is.EqualTo(HashTypes.Meow));
    }
}