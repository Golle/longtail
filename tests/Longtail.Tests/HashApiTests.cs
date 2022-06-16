using System.Text;
using NUnit.Framework;

namespace Longtail.Tests;

internal class HashApiTests
{
    private static readonly byte[] AString = Encoding.UTF8.GetBytes("dont change this string");

    private ulong Blake2Hash = 2095886943890721232;
    private ulong Blake3Hash = 10551986767028710526;
    private ulong MeowHash = 16919672222472858428;

    [Test]
    public void CreateBlake2HashAPI_OnSuccess_ReturnHashApi()
    {
        var hashType = LongtailLibrary.Longtail_GetBlake2HashType();

        using var hashApi = HashApi.CreateBlake2HashAPI();

        Assert.That(hashApi.GetIdentifier(), Is.EqualTo(hashType));
    }

    [Test]
    public void CreateBlake3HashAPI_OnSuccess_ReturnHashApi()
    {
        var hashType = LongtailLibrary.Longtail_GetBlake3HashType();

        using var hashApi = HashApi.CreateBlake3HashAPI();

        Assert.That(hashApi.GetIdentifier(), Is.EqualTo(hashType));
    }

    [Test]
    public void CreateMeowHashAPI_OnSuccess_ReturnHashApi()
    {
        var hashType = LongtailLibrary.Longtail_GetMeowHashType();

        using var hashApi = HashApi.CreateMeowHashAPI();

        Assert.That(hashApi.GetIdentifier(), Is.EqualTo(hashType));
    }

    [Test]
    public void HashBuffer_Blake2_ReturnHash()
    {
        using var hashApi = HashApi.CreateBlake2HashAPI();

        var result = hashApi.HashBuffer(AString);

        Assert.That(result, Is.EqualTo(Blake2Hash));
    }

    [Test]
    public void HashBuffer_Blake3_ReturnHash()
    {
        using var hashApi = HashApi.CreateBlake3HashAPI();

        var result = hashApi.HashBuffer(AString);

        Assert.That(result, Is.EqualTo(Blake3Hash));
    }

    [Test]
    public void HashBuffer_Meow_ReturnHash()
    {
        using var hashApi = HashApi.CreateMeowHashAPI();

        var result = hashApi.HashBuffer(AString);

        Assert.That(result, Is.EqualTo(MeowHash));
    }

    [Test]
    public void Hash_MeowWithContext_ReturnHash()
    {
        using var hashApi = HashApi.CreateMeowHashAPI();
        using var context = hashApi.BeginContext();
        context.Hash(AString);

        var result = context.EndContext();

        Assert.That(result, Is.EqualTo(MeowHash));
    }

    [Test]
    public void Hash_Blake2WithContext_ReturnHash()
    {
        using var hashApi = HashApi.CreateBlake2HashAPI();
        using var context = hashApi.BeginContext();
        context.Hash(AString);

        var result = context.EndContext();

        Assert.That(result, Is.EqualTo(Blake2Hash));
    }

    [Test]
    public void Hash_Blake3WithContext_ReturnHash()
    {
        using var hashApi = HashApi.CreateBlake3HashAPI();
        using var context = hashApi.BeginContext();
        context.Hash(AString);

        var result = context.EndContext();

        Assert.That(result, Is.EqualTo(Blake3Hash));
    }

    [Test, Ignore($"{nameof(LongtailLibrary.Longtail_GetPathHash)} is missing LONGTAIL_EXPORT")]
    public void GetPathHash_Blake2_ReturnHash()
    {
        using var hashApi = HashApi.CreateBlake2HashAPI();

        var hash = hashApi.GetPathHash(Path.GetTempPath());

        Assert.That(hash, Is.Not.EqualTo(0));
    }

    [Test, Ignore($"{nameof(LongtailLibrary.Longtail_GetPathHash)} is missing LONGTAIL_EXPORT")]
    public void GetPathHash_Blake3_ReturnHash()
    {
        using var hashApi = HashApi.CreateBlake3HashAPI();

        var hash = hashApi.GetPathHash(Path.GetTempPath());

        Assert.That(hash, Is.Not.EqualTo(0));
    }

    [Test, Ignore($"{nameof(LongtailLibrary.Longtail_GetPathHash)} is missing LONGTAIL_EXPORT")]
    public void GetPathHash_Meow_ReturnHash()
    {
        using var hashApi = HashApi.CreateMeowHashAPI();

        var hash = hashApi.GetPathHash(Path.GetTempPath());

        Assert.That(hash, Is.Not.EqualTo(0));
    }
}