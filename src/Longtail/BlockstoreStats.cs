namespace Longtail;

public class BlockstoreStats
{
    private Longtail_BlockStore_Stats _blockStoreStats;
    internal ref Longtail_BlockStore_Stats Internal => ref _blockStoreStats;
    internal BlockstoreStats(in Longtail_BlockStore_Stats stats)
    {
        _blockStoreStats = stats;
    }

    public void Set(BlockStoreApiStats index, ulong value) => throw new InvalidOperationException($"{nameof(Longtail_BlockStore_Stats)} has not been generated correctly");
    public ulong Get(BlockStoreApiStats index) => throw new InvalidOperationException($"{nameof(Longtail_BlockStore_Stats)} has not been generated correctly");
    public ref ulong this[BlockStoreApiStats index] => throw new InvalidOperationException($"{nameof(Longtail_BlockStore_Stats)} has not been generated correctly");
}