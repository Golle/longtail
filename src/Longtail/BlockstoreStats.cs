namespace Longtail;

public unsafe class BlockstoreStats
{
    private Longtail_BlockStore_Stats _blockStoreStats;
    internal ref Longtail_BlockStore_Stats Internal => ref _blockStoreStats;
    public BlockstoreStats()
    {
        _blockStoreStats = default;
    }
    internal BlockstoreStats(in Longtail_BlockStore_Stats stats)
    {
        _blockStoreStats = stats;
    }

    public void Set(BlockStoreApiStats index, ulong value) => _blockStoreStats.m_StatU64[(int)index] = value;
    public ulong Get(BlockStoreApiStats index) => _blockStoreStats.m_StatU64[(int)index];
    public ref ulong this[BlockStoreApiStats index] => ref _blockStoreStats.m_StatU64[(int)index];
}