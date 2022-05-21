namespace Longtail;

public unsafe class StoreIndex : IDisposable
{
    private Longtail_StoreIndex* _storeIndex;
    private readonly bool _owner;
    internal Longtail_StoreIndex* AsPointer() => _storeIndex;
    private StoreIndex(Longtail_StoreIndex* storeIndex, bool owner = true)
    {
        _storeIndex = storeIndex;
        _owner = owner;
    }

    public void Dispose()
    {
        if (_storeIndex != null && _owner)
        {
            LongtailLibrary.Longtail_Free(_storeIndex);
            _storeIndex = null;
        }
    }
}