using NUnit.Framework;

namespace Longtail.Tests;

internal class HashApiTests
{
    [Test]
    public void CreateBlake2HashAPI_OnSuccess_ReturnHashApi()
    {
        var hashType = LongtailLibrary.Longtail_GetBlake2HashType();
        
        using var hashApi = HashApi.CreateBlake2HashAPI()!;

        Assert.That(hashApi.GetIdentifier(), Is.EqualTo(hashType));
    }

    [Test]
    public void CreateBlake3HashAPI_OnSuccess_ReturnHashApi()
    {
        var hashType = LongtailLibrary.Longtail_GetBlake3HashType();

        using var hashApi = HashApi.CreateBlake3HashAPI()!;

        Assert.That(hashApi.GetIdentifier(), Is.EqualTo(hashType));
    }
    
    [Test]
    public void CreateMeowHashAPI_OnSuccess_ReturnHashApi()
    {
        var hashType = LongtailLibrary.Longtail_GetMeowHashType();

        using var hashApi = HashApi.CreateMeowHashAPI()!;

        Assert.That(hashApi.GetIdentifier(), Is.EqualTo(hashType));
    }
}