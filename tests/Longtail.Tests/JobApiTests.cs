using NUnit.Framework;

namespace Longtail.Tests;

internal class JobApiTests
{
    [Test]
    public void CreateBikeshedJobAPI_Success_ReturnJobApi()
    {
        using var jobApi = JobApi.CreateBikeshedJobAPI(1);

        Assert.That(jobApi, Is.Not.Null);
    }

    [TestCase(1u)]
    [TestCase(4u)]
    [TestCase(8u)]
    public void CreateBikeshedJobAPI_WithWorkerCount_ReturnCorrectWorkerCount(uint workerCount)
    {
        using var jobApi = JobApi.CreateBikeshedJobAPI(workerCount);

        var result = jobApi.GetWorkerCount();

        Assert.That(result, Is.EqualTo(workerCount));
    }
}