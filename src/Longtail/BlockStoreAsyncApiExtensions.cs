namespace Longtail;

public static class BlockStoreAsyncApiExtensions
{
    public static Task FlushAsync(this BlockStoreApi api) => Task.Run(api.Flush);
}