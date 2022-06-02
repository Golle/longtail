using NUnit.Framework;

namespace Longtail.Tests;

internal class FileInfosTests
{
    private readonly string TestFolder = Path.Combine(Directory.GetCurrentDirectory(), "tmp");
    
    [OneTimeSetUp]
    public void SetUp()
    {
        if (!Directory.Exists(TestFolder))
        {
            Directory.CreateDirectory(TestFolder);
            var buffer = new byte[100 * 1024];
            new Random().NextBytes(buffer);
            File.WriteAllBytes(Path.Combine(TestFolder, "a_file.tmp"), buffer);
        }
    }
        
    [OneTimeTearDown]
    public void TearDown()
    {
        if (Directory.Exists(TestFolder))
        {
            Directory.Delete(TestFolder, true);
        }
    }

    [Test]
    public void GetFilesRecursively_OnSuccess_ReturnFileInfos()
    {
        using var fileStorageApi = StorageApi.CreateFSStorageAPI();
        using var fileInfos = FileInfos.GetFilesRecursively(TestFolder, fileStorageApi);

        Assert.That(fileInfos, Is.Not.Null);
    }

    [Test]
    public void GetPath_OnSuccess_ReturnPath()
    {
        // NOTE(Jens): when we can implement a mock for the storage API we can replace this to a fake FS.
        using var fileStorageApi = StorageApi.CreateFSStorageAPI();
        using var fileInfos = FileInfos.GetFilesRecursively(TestFolder, fileStorageApi)!;
        
        var result = fileInfos.GetPath(0);

        Assert.That(result, Is.Not.Null.Or.Empty);
    }

    [Test]
    public void GetCount_OnSuccess_ReturnCount()
    {
        // NOTE(Jens): when we can implement a mock for the storage API we can replace this to a fake FS.
        using var fileStorageApi = StorageApi.CreateFSStorageAPI();
        using var fileInfos = FileInfos.GetFilesRecursively(TestFolder, fileStorageApi)!;

        var result = fileInfos.GetCount();

        Assert.That(result, Is.GreaterThan(0));
    }

    [Test]
    public void GetSize_OnSuccess_ReturnSize()
    {
        // NOTE(Jens): when we can implement a mock for the storage API we can replace this to a fake FS.
        using var fileStorageApi = StorageApi.CreateFSStorageAPI();
        using var fileInfos = FileInfos.GetFilesRecursively(TestFolder, fileStorageApi)!;

        var result = fileInfos.GetSize(0);

        Assert.That(result, Is.GreaterThan(0));
    }

    [Test]
    public void GetPermissions_OnSuccess_ReturnPermissions()
    {
        // NOTE(Jens): when we can implement a mock for the storage API we can replace this to a fake FS.
        using var fileStorageApi = StorageApi.CreateFSStorageAPI();
        using var fileInfos = FileInfos.GetFilesRecursively(TestFolder, fileStorageApi)!;

        var result = fileInfos.GetPermissions(0);

        Assert.That(result, Is.Not.EqualTo(0));
    }
}