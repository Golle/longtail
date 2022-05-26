using NUnit.Framework;

namespace Longtail.Tests;

internal class CompressionApiTests
{
    [Test]
    public void CreateForBrotli_NoCompressionType_ReturnNull()
    {
        using var api = CompressionApi.CreateForBrotli(0);

        Assert.That(api, Is.Null);
    }

    [Test]
    public void CreateForZstd_NoCompressionType_ReturnNull()
    {
        using var api = CompressionApi.CreateForZstd(0);

        Assert.That(api, Is.Null);
    }
    
    [Test]
    public void CreateForLZ4_NoCompressionType_ReturnNull()
    {
        using var api = CompressionApi.CreateForLZ4(0);

        Assert.That(api, Is.Null);
    }

    [TestCaseSource(nameof(GetBrotliCompressionTypes))]
    public void CreateForBrotli_WithCompressionType_ReturnCompressionApi(uint compressionType)
    {
        using var api = CompressionApi.CreateForBrotli(compressionType);

        Assert.That(api, Is.Not.Null);
    }
    
    [TestCaseSource(nameof(GetBrotliCompressionTypes))]
    public void CreateForBrotli_WithCompressionType_SetSettingsId(uint compressionType)
    {
        using var api = CompressionApi.CreateForBrotli(compressionType)!;

        Assert.That(api.SettingsId, Is.EqualTo(compressionType));
    }

    [TestCaseSource(nameof(GetZStdCompressionTypes))]
    public void CreateForZStd_WithCompressionType_ReturnCompressionApi(uint compressionType)
    {
        using var api = CompressionApi.CreateForZstd(compressionType);

        Assert.That(api, Is.Not.Null);
    }

    [TestCaseSource(nameof(GetZStdCompressionTypes))]
    public void CreateForZStd_WithCompressionType_SetSettingsId(uint compressionType)
    {
        using var api = CompressionApi.CreateForZstd(compressionType)!;

        Assert.That(api.SettingsId, Is.EqualTo(compressionType));
    }

    [Test]
    public void CreateForLZ4_WithCompressionType_ReturnCompressionApi()
    {
        using var api = CompressionApi.CreateForLZ4(CompressionTypes.LZ4DefaultQuality);

        Assert.That(api, Is.Not.Null);
    }

    [Test]
    public void CreateForLZ4_WithCompressionType_SetSettingsId()
    {
        using var api = CompressionApi.CreateForLZ4(CompressionTypes.LZ4DefaultQuality)!;

        Assert.That(api.SettingsId, Is.EqualTo(CompressionTypes.LZ4DefaultQuality));
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
}
