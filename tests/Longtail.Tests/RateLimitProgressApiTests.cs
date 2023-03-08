using NUnit.Framework;

namespace Longtail.Tests;

internal class RateLimitProgressApiTests
{
    [Test]
    public void CreateRateLimitedProgress_CreateAndDispose_DontThrowException()
    {
        Assert.DoesNotThrow(() =>
        {
            using var progressApi = ProgressApi.CreateRateLimitedProgress(_ => { }, 1);
        });
    }

    [TestCase(2u)]
    [TestCase(3u)]
    [TestCase(uint.MaxValue)]
    public void OnProgress_DoneCountHigherThanLimit_CallbackWithDoneCount(uint doneCount)
    {
        var result = 0u;
        using var progressApi = ProgressApi.CreateRateLimitedProgress(tuple => result = tuple.DoneCount, percentRateLimit: 1);

        progressApi.OnProgress(100, doneCount);

        Assert.That(result, Is.EqualTo(doneCount));
    }

    [TestCase(2u)]
    [TestCase(3u)]
    [TestCase(4u)]
    public void OnProgress_DoneCountLowerThanLimit_DontCallCallback(uint doneCount)
    {
        var result = 0u;
        using var progressApi = ProgressApi.CreateRateLimitedProgress(tuple => result = tuple.DoneCount, percentRateLimit: 5);

        progressApi.OnProgress(100, 0); // first call will not be limited
        progressApi.OnProgress(100, doneCount);

        Assert.That(result, Is.EqualTo(uint.MinValue));
    }

    [TestCase(0u)]
    [TestCase(6u)]
    [TestCase(uint.MaxValue)]
    public void OnProgress_TotalCount_CallbackWithTotalCount(uint totalCount)
    {
        var result = 0u;
        using var progressApi = ProgressApi.CreateRateLimitedProgress(tuple => result = tuple.TotalCount, percentRateLimit: 1);

        progressApi.OnProgress(totalCount, 0);

        Assert.That(result, Is.EqualTo(totalCount));
    }
}