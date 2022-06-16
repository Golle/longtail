namespace Longtail;

public static class StorageApiExtensions
{
    public static Task<BlockIndex> ReadBlockIndexAsync(this StorageApi storageApi, string path) => Task.Run(() => storageApi.ReadBlockIndex(path));
}