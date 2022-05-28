using NUnit.Framework;

namespace Longtail.Tests;

internal class LongtailLogTests
{
    [TestCase(LongtailLogLevel.Debug)]
    [TestCase(LongtailLogLevel.Info)]
    [TestCase(LongtailLogLevel.Warning)]
    [TestCase(LongtailLogLevel.Error)]
    [TestCase(LongtailLogLevel.Off)]
    public void GetLogLevel_Always_ReturnLogLevel(LongtailLogLevel level)
    {
        LongtailLog.SetLogLevel(level);

        var result = LongtailLog.GetLogLevel();

        Assert.That(result, Is.EqualTo(level));
    }

    [Test]
    public void SetLogCallback_DebugLevel_CallLogCallback()
    {
        // TODO(Jens): Add better tests for logging if needed. This just makes sure we get a callback and it converts it correctly.
        LongtailLogContext result = null;
        LongtailLog.SetLogLevel(LongtailLogLevel.Debug);
        LongtailLog.SetLogCallback((context, s) => { result = context; });
        using var storage = StorageApi.CreateFSStorageAPI();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Fields, Is.Not.Empty);
    }
}