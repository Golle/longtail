using NUnit.Framework;

namespace Longtail.Tests;

internal class CompressionRegistryTests
{
    [Test]
    public void CreateFullCompressionRegistry_OnSuccess_CreateRegistry()
    {
        using var result = CompressionRegistry.CreateFullCompressionRegistry();

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void CreateZStdCompressionRegistry_OnSuccess_CreateRegistry()
    {
        using var result = CompressionRegistry.CreateZStdCompressionRegistry();

        Assert.That(result, Is.Not.Null);
    }
}