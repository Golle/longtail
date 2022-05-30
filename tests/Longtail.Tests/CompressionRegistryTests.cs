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

    [Test]
    public void GetCompressionAPI_NoCompressionType_ReturnNull()
    {
        using var registry = CompressionRegistry.CreateFullCompressionRegistry()!;

        using var result = registry.GetCompressionAPI(0);

        Assert.That(result, Is.Null);
    }

    [TestCaseSource(nameof(GetZStdCompressionTypes))]
    public void GetCompressionAPI_FZStdRegistryWithCompressionType_ReturnHashApi(uint compressionType)
    {
        using var registry = CompressionRegistry.CreateZStdCompressionRegistry()!;

        using var result = registry.GetCompressionAPI(compressionType)!;

        Assert.That(result.SettingsId, Is.EqualTo(compressionType));
    }


    [TestCaseSource(nameof(GetBrotliCompressionTypes))]
    [TestCaseSource(nameof(GetZStdCompressionTypes))]
    [TestCaseSource(nameof(GetLZ4CompressionTypes))]
    public void GetCompressionAPI_FullRegistryWithCompressionType_ReturnHashApi(uint compressionType)
    {
        using var registry = CompressionRegistry.CreateFullCompressionRegistry()!;

        using var result = registry.GetCompressionAPI(compressionType)!;

        Assert.That(result.SettingsId, Is.EqualTo(compressionType));
    }


    private static IEnumerable<uint> GetZStdCompressionTypes()
    {
        yield return CompressionTypes.ZStdDefaultQuality;
        yield return CompressionTypes.ZStdMinQuality;
        yield return CompressionTypes.ZStdMaxQuality;
        yield return CompressionTypes.ZStdHighQuality;
        yield return CompressionTypes.ZStdLowQuality;
    }

    private static IEnumerable<uint> GetBrotliCompressionTypes()
    {
        yield return CompressionTypes.BrotliGenericDefaultQuality;
        yield return CompressionTypes.BrotliGenericMinQuality;
        yield return CompressionTypes.BrotliGenericMaxQuality;
        yield return CompressionTypes.BrotliTextDefaultQuality;
        yield return CompressionTypes.BrotliTextMinQuality;
        yield return CompressionTypes.BrotliTextMaxQuality;
    }

    private static IEnumerable<uint> GetLZ4CompressionTypes()
    {
        yield return CompressionTypes.LZ4DefaultQuality;
    }
}