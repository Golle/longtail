using NUnit.Framework;

namespace Longtail.Tests;

internal class FileInfosTests
{
    [Test]
    public void GetFilesRecursively_OnSuccess_ReturnFileInfos()
    {
        using var fileStorageApi = StorageApi.CreateFSStorageAPI();
        using var fileInfos = FileInfos.GetFilesRecursively(Directory.GetCurrentDirectory(), fileStorageApi);

        Assert.That(fileInfos, Is.Not.Null);
    }

    [Test]
    public void GetPath_OnSuccess_ReturnPath()
    {
        // NOTE(Jens): when we can implement a mock for the storage API we can replace this to a fake FS.
        using var fileStorageApi = StorageApi.CreateFSStorageAPI();
        using var fileInfos = FileInfos.GetFilesRecursively(Directory.GetCurrentDirectory(), fileStorageApi)!;
        
        var result = fileInfos.GetPath(0);

        Assert.That(result, Is.Not.Null.Or.Empty);
    }

    [Test]
    public void GetCount_OnSuccess_ReturnCount()
    {
        // NOTE(Jens): when we can implement a mock for the storage API we can replace this to a fake FS.
        using var fileStorageApi = StorageApi.CreateFSStorageAPI();
        using var fileInfos = FileInfos.GetFilesRecursively(Directory.GetCurrentDirectory(), fileStorageApi)!;

        var result = fileInfos.GetCount();

        Assert.That(result, Is.GreaterThan(0));
    }

    [Test]
    public void GetSize_OnSuccess_ReturnSize()
    {
        // NOTE(Jens): when we can implement a mock for the storage API we can replace this to a fake FS.
        using var fileStorageApi = StorageApi.CreateFSStorageAPI();
        using var fileInfos = FileInfos.GetFilesRecursively(Directory.GetCurrentDirectory(), fileStorageApi)!;

        Assert.That(Directory.GetCurrentDirectory(), Is.EqualTo("nope"));
        var result = fileInfos.GetSize(0);

        Assert.That(result, Is.GreaterThan(0));
    }

    [Test]
    public void GetPermissions_OnSuccess_ReturnPermissions()
    {
        // NOTE(Jens): when we can implement a mock for the storage API we can replace this to a fake FS.
        using var fileStorageApi = StorageApi.CreateFSStorageAPI();
        using var fileInfos = FileInfos.GetFilesRecursively(Directory.GetCurrentDirectory(), fileStorageApi)!;

        var result = fileInfos.GetPermissions(0);

        Assert.That(result, Is.Not.EqualTo(0));
    }
}