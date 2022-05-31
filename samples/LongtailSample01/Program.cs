using System;
using System.IO;
using Longtail;

var basePath = FindFile(AppDomain.CurrentDomain.BaseDirectory, "Longtail.sln", 10);
if (basePath == null)
{
    Console.Error.WriteLine("Failed to find the root folder");
    return -1;
}

const uint targetChunkSize = 32768u;
const uint targetBlocKSize = 8388608u;
const uint chunksPerBlock = 1024u;

var tmpPath = Path.Combine(basePath, "tmp", typeof(Program).Assembly.GetName().Name!);
var dataPath = Path.Combine(tmpPath, "data");
var destinationPath = Path.Combine(tmpPath, "output");

Console.WriteLine("Creating files.");
CreateSomeFiles(dataPath, 10, 4 * 1024, 10 * 1024 * 1024);
Console.WriteLine("Files created.");
using var fsStorage = StorageApi.CreateFSStorageAPI()!;
using var files = FileInfos.GetFilesRecursively(dataPath, fsStorage);

using var jobApi = JobApi.CreateBikeshedJobAPI((uint)(Environment.ProcessorCount - 1))!;

using var hashRegistry = HashRegistry.CreateFullHashRegistry()!;
using var hashApi = hashRegistry.GetHashApi(HashTypes.Blake3)!;

//using var compression = CompressionApi.CreateBrotliCompressionAPI()!;
using var chunker = ChunkerApi.CreateHPCDCChunkerAPI();

using var progress = new ProgressApi(tuple => Console.WriteLine($"{tuple.DoneCount}/{tuple.TotalCount} completed."));
using var versionIndex = VersionIndex.Create(dataPath, fsStorage, hashApi!, chunker!, jobApi, files!, targetChunkSize, false)!;

using var outBlockStore = BlockStoreApi.CreateFSBlockStoreApi(jobApi, fsStorage, destinationPath)!;

using var compressionRegistry = CompressionRegistry.CreateFullCompressionRegistry()!;
compressionRegistry.GetCompressionAPI(CompressionTypes.BrotliGenericDefaultQuality);

using var outStoreIndex = outBlockStore.GetExistingContent(versionIndex.GetChunkHashes())!;
using var versionMissingStoreIndex = StoreIndex.CreateMissingContent(hashApi, outStoreIndex, versionIndex, targetBlocKSize, chunksPerBlock);
if (versionMissingStoreIndex.GetBlockCount() > 0)
{
    
    Console.WriteLine("Misssing content found.");
}

Console.WriteLine("All well");

return 0;

static string? FindFile(string? currentPath, string name, int parents = 6)
{
    if (currentPath == null || parents == 0)
    {
        return null;
    }
    var fullPath = Path.Combine(currentPath, name);
    if (File.Exists(fullPath))
    {
        return currentPath;
    }
    return FindFile(Directory.GetParent(currentPath)?.FullName, name, parents - 1);
}



static void CreateSomeFiles(string dataPath, int fileCount, int minSize, int maxSize)
{
    if (Directory.Exists(dataPath))
    {
        Directory.Delete(dataPath, true);
    }
    Directory.CreateDirectory(dataPath);

    var random = new Random();
    for (var i = 0; i < fileCount; ++i)
    {
        var size = random.Next(minSize, maxSize);
        var filename = $"{Guid.NewGuid()}.dat";

        using var file = File.OpenWrite(Path.Combine(dataPath, filename));
        file.SetLength(size);
        var bytes = new byte[size];
        random.NextBytes(bytes);
        file.Write(bytes);
    }

}