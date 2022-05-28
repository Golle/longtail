using NUnit.Framework;

namespace Longtail.Tests;

internal class CancelApiTests
{
    [Test]
    public void CreateAtomicCancelAPI_OnSuccess_ReturnApi()
    {
        using var api = CancelApi.CreateAtomicCancelAPI();

        Assert.That(api, Is.Not.Null);
    }
}