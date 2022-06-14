using NUnit.Framework;

namespace Longtail.Tests;

internal class StorageApiTests
{
    [Test]
    public void CreateInMemoryStorageAPI_OnSuccess_ReturnStorageApi()
    {
        using var api = StorageApi.CreateInMemoryStorageAPI();

        Assert.That(api, Is.Not.Null);
    }

    [Test]
    public void CreateFSStorageAPI_OnSuccess_ReturnStorageApi()
    {
        using var api = StorageApi.CreateFSStorageAPI();

        Assert.That(api, Is.Not.Null);
    }

    [Test]
    public void OpeanReadFile_FileDoesNotExist_ReturnNull()
    {
        using var storage = StorageApi.CreateFSStorageAPI();

        using var file = storage.OpenReadFile("file_not_exists.txt");

        Assert.That(file, Is.Null);
    }

    [Test]
    public void GetSize_FileExists_ReturnSize()
    {
        using var testFile = new TestFile();
        using var storage = StorageApi.CreateFSStorageAPI();

        using var file = storage.OpenReadFile(testFile.Path)!;
        var size = file.GetSize();

        Assert.That(size, Is.EqualTo(testFile.Length));
    }

    [Test]
    public void Read_FileExists_ReturnContentsOfFile()
    {
        using var testFile = new TestFile();
        using var storage = StorageApi.CreateFSStorageAPI();

        using var file = storage.OpenReadFile(testFile.Path)!;
        var buffer = new byte[file.GetSize()];
        file.Read(buffer);

        Assert.That(buffer, Is.EqualTo(testFile.Bytes));
    }

    /// <summary>
    /// This will create a temporary file which will be deleted when its disposed. (Not sure if this api works on all platforms)
    /// </summary>
    public class TestFile : IDisposable
    {
        public string Path { get; } = System.IO.Path.GetTempFileName();
        public byte[] Bytes { get; }
        public int Length => Bytes.Length;

        private static readonly Random _random = new();
        public TestFile(int size = 1000)
        {
            Bytes = new byte[size];
            _random.NextBytes(Bytes);
            File.WriteAllBytes(Path, Bytes);
        }

        public void Dispose() => File.Delete(Path);
    }
}