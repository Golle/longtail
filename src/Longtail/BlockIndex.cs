namespace Longtail;

public readonly unsafe ref struct BlockIndex
{
    private readonly Longtail_BlockIndex* _index;
    public ulong BlockHash => *_index->m_BlockHash;
    public uint HashIdentifier => *_index->m_HashIdentifier;
    public uint ChunkCount => *_index->m_ChunkCount;
    public uint Tag => *_index->m_Tag;
    public ReadOnlySpan<ulong> ChunkHashes => new(_index->m_ChunkHashes, (int)ChunkCount);
    public ReadOnlySpan<uint> ChunkSizes => new(_index->m_ChunkSizes, (int)ChunkCount);
    internal BlockIndex(Longtail_BlockIndex* index)
    {
        _index = index;
    }
}