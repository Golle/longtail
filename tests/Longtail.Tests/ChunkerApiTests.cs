using NUnit.Framework;

namespace Longtail.Tests;

internal class ChunkerApiTests
{
    [Test]
    public void CreateHPCDCChunkerAPI_OnSuccess_ReturnChunkerApi()
    {
        using var result = ChunkerApi.CreateHPCDCChunkerAPI();

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetMinChunkSize_OnSuccess_ReturnMinChunkSize()
    {
        using var api = ChunkerApi.CreateHPCDCChunkerAPI()!;

        var result = api.GetMinChunkSize();

        Assert.That(result, Is.EqualTo(48));
    }
}