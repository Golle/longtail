using System;
using System.IO;
using System.Linq;
using Longtail;

MemTracer.Init();
{
    var basePath = FindFile(AppDomain.CurrentDomain.BaseDirectory, "Longtail.sln", 10);
    if (basePath == null)
    {
        Console.Error.WriteLine("Failed to find the root folder");
        return -1;
    }

    const uint targetChunkSize = 32768u;
    const uint targetBlocKSize = 8388608u;
    const uint chunksPerBlock = 1024u;

    var samplePath = Path.Combine(basePath, "tmp", typeof(Program).Assembly.GetName().Name!);
    var dataPath = Path.Combine(samplePath, "data");
    var destinationPath = Path.Combine(samplePath, "store");
    var outPath = Path.Combine(samplePath, "out");

    // Clear old folders since we generate new data
    CleanupFolder(dataPath);
    CleanupFolder(destinationPath);
    CleanupFolder(outPath);

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
        API.WriteContent(dataPath, versionMissingStoreIndex, versionIndex, fsStorage, outBlockStore, jobApi, progress);
    }
    Console.WriteLine("All well");

    if (!CompareContent(dataPath, outPath))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Mismatch in content.");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine("All files are identical!");
    }
}

Console.WriteLine($"Allocations: {MemTracer.Allocations}. Deallocations: {MemTracer.Deallocations}. Memory allocated: {MemTracer.MemoryAllocated} bytes");
//Console.WriteLine(MemTracer.GetStatsSummary());
//Console.WriteLine(MemTracer.GetStatsDetailed());
MemTracer.Dispose();

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



static bool CompareContent(string path1, string path2)
{
    var files1 = Directory.GetFiles(path1, "*", SearchOption.AllDirectories);
    var files2 = Directory.GetFiles(path2, "*", SearchOption.AllDirectories);
    if (files1.Length != files2.Length)
    {
        Console.Error.WriteLine($"Mismatch in number of files: {files1.Length} != {files2.Length}");
        return false;
    }

    foreach (var file1 in files1)
    {
        var path = Path.GetRelativePath(path1, file1);
        var file2 = files2.SingleOrDefault(f => Path.GetRelativePath(path2, f) == path);
        if (file2 == null)
        {
            Console.Error.WriteLine($"File {path} could not be found in {path2}");
            return false;
        }

        using var stream1 = File.OpenRead(file1);
        using var stream2 = File.OpenRead(file2);

        if (stream1.Length != stream2.Length)
        {
            Console.Error.WriteLine($"Mismatch in size for {path}. {stream1.Length} != {stream2.Length}");
            return false;
        }

        var buffer1 = new byte[stream1.Length];
        if (stream1.Read(buffer1) != stream1.Length)
        {
            throw new InvalidOperationException("failed to read all bytes in stream1..");
        }
        var buffer2 = new byte[stream2.Length];
        if (stream2.Read(buffer2) != stream2.Length)
        {
            throw new InvalidOperationException("failed to read all bytes in stream2..");
        }

        for (var i = 0; i < buffer1.Length; ++i)
        {
            if (buffer1[i] != buffer2[i])
            {
                Console.WriteLine($"Mismatch in bytes in file {path}");
                return false;
            }
        }
    }

    return true;
}

static void CleanupFolder(string path)
{
    if (Directory.Exists(path))
    {
        Directory.Delete(path, true);
    }
    Directory.CreateDirectory(path);
}

static void CreateSomeFiles(string dataPath, int fileCount, int minSize, int maxSize)
{
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