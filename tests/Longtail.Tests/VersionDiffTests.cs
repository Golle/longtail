using NUnit.Framework;

namespace Longtail.Tests;

internal class VersionDiffTests
{
    [Test]
    public unsafe void METHOD_STATE_RESULT()
    {
        using var hashRegistry = HashRegistry.CreateFullHashRegistry()!;
        using var hashApi = hashRegistry.GetHashApi(LongtailLibrary.Longtail_GetBlake3HashType())!;
        //NOTE(Jens): to test this we need to have a version index file
        //var diff = VersionDiff.Create(hashApi, );

    }
}
