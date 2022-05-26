namespace Longtail;

public sealed unsafe class StoreIndex : IDisposable
{
    private Longtail_StoreIndex* _storeIndex;
    private readonly bool _owner;
    internal Longtail_StoreIndex* AsPointer() => _storeIndex;

    internal StoreIndex(Longtail_StoreIndex* storeIndex, bool owner = true)
    {
        _storeIndex = storeIndex;
        _owner = owner;
    }

    public uint GetVersion() => LongtailLibrary.Longtail_StoreIndex_GetVersion(_storeIndex);
    public uint GetHashIdentifier() => LongtailLibrary.Longtail_StoreIndex_GetHashIdentifier(_storeIndex);
    public uint GetBlockCount() => LongtailLibrary.Longtail_StoreIndex_GetBlockCount(_storeIndex);
    public uint GetChunkCount() => LongtailLibrary.Longtail_StoreIndex_GetChunkCount(_storeIndex);
    // NOTE(Jens): these does not have set size, they depend on other factors. Not sure what to return? maybe just a struct with an indexer that wont check sizes?
    public ulong* GetBlockHashes() => throw new NotImplementedException("No fixed size for this, so we'll have to support unsafe code. TBD");
    public ulong* GetChunkHashes() => throw new NotImplementedException("No fixed size for this, so we'll have to support unsafe code. TBD");
    public ReadOnlySpan<uint> GetBlockChunksOffsets() => new(LongtailLibrary.Longtail_StoreIndex_GetBlockChunksOffsets(_storeIndex), (int)GetBlockCount());
    public ReadOnlySpan<uint> GetBlockChunkCounts() => new(LongtailLibrary.Longtail_StoreIndex_GetBlockChunkCounts(_storeIndex), (int)GetBlockCount());
    public ReadOnlySpan<uint> GetBlockTags() => new(LongtailLibrary.Longtail_StoreIndex_GetBlockTags(_storeIndex), (int)GetBlockCount());
    public uint* GetChunkSizes() => throw new NotImplementedException("No fixed size for this, so we'll have to support unsafe code. TBD");

    public void Dispose()
    {
        if (_storeIndex != null && _owner)
        {
            LongtailLibrary.Longtail_Free(_storeIndex);
            _storeIndex = null;
        }
    }
}