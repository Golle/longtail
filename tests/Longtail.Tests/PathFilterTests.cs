using NSubstitute;
using NUnit.Framework;

namespace Longtail.Tests;

internal class PathFilterTests
{
    private IPathFilter _pathFilter;

    [SetUp]
    public void SetUp()
    {
        _pathFilter = Substitute.For<IPathFilter>();
    }

    [Test]
    public void MakePathFilterApi_OnSuccess_ReturnPathFilter()
    {
        using var filter = PathFilterApi.MakePathFilterApi(_pathFilter);

        Assert.That(filter, Is.Not.Null);
    }

    [Test]
    public void Include_Always_PassRootPathToFilter()
    {
        using var filter = PathFilterApi.MakePathFilterApi(_pathFilter);
        
        filter.Include("a", string.Empty, string.Empty, false, 0, 0);

        _pathFilter.Received(1).Include(Arg.Is("a"), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<ulong>(), Arg.Any<ushort>());
    }

    [Test]
    public void Include_Always_PassAssetPathToFilter()
    {
        using var filter = PathFilterApi.MakePathFilterApi(_pathFilter);

        filter.Include(string.Empty, "a", string.Empty, false, 0, 0);

        _pathFilter.Received(1).Include(Arg.Any<string>(), Arg.Is("a"), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<ulong>(), Arg.Any<ushort>());
    }

    [Test]
    public void Include_Always_PassAssetNameToFilter()
    {
        using var filter = PathFilterApi.MakePathFilterApi(_pathFilter);

        filter.Include(string.Empty, string.Empty, "a", false, 0, 0);

        _pathFilter.Received(1).Include(Arg.Any<string>(), Arg.Any<string>(), Arg.Is("a"), Arg.Any<bool>(), Arg.Any<ulong>(), Arg.Any<ushort>());
    }
    
    [TestCase(true)]
    [TestCase(false)]
    public void Include_Always_PassIsDirToFilter(bool isDir)
    {
        using var filter = PathFilterApi.MakePathFilterApi(_pathFilter);

        filter.Include(string.Empty, string.Empty, string.Empty, isDir, 0, 0);

        _pathFilter.Received(1).Include(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Is(isDir), Arg.Any<ulong>(), Arg.Any<ushort>());
    }

    [Test]
    public void Include_Always_PassAssetSizeToFilter()
    {
        using var filter = PathFilterApi.MakePathFilterApi(_pathFilter);

        filter.Include(string.Empty, string.Empty, string.Empty, false, 1, 0);

        _pathFilter.Received(1).Include(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Is<ulong>(1), Arg.Any<ushort>());
    }

    [Test]
    public void Include_Always_PassPermissionsToFilter()
    {
        using var filter = PathFilterApi.MakePathFilterApi(_pathFilter);

        filter.Include(string.Empty, string.Empty, string.Empty, false, 0, 1);

        _pathFilter.Received(1).Include(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<ulong>(), Arg.Is<ushort>(1));
    }
}