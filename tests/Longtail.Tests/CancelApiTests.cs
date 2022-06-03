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

    [Test]
    public void CreateToken_OnSuccess_ReturnToken()
    {
        using var api = CancelApi.CreateAtomicCancelAPI();
        
        using var token = api.CreateToken();

        Assert.That(token, Is.Not.Null);
    }

    [Test]
    public void IsCancelled_NotCancelled_ReturnFalse()
    {
        using var api = CancelApi.CreateAtomicCancelAPI();
        using var token = api.CreateToken();

        var result = token.IsCancelled;

        Assert.That(result, Is.False);
    }

    [Test]
    public void IsCancelled_Cancelled_ReturnTrue()
    {
        using var api = CancelApi.CreateAtomicCancelAPI();
        using var token = api.CreateToken();
        token.Cancel();

        var result = token.IsCancelled;

        Assert.That(result, Is.True);
    }
}