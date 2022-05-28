namespace Longtail;

public unsafe class StoredBlock : IDisposable
{
    private Longtail_StoredBlock* _storedBlock;
    private readonly bool _owner;
    internal Longtail_StoredBlock* AsPointer() => _storedBlock;
    internal StoredBlock(Longtail_StoredBlock* storedBlock, bool owner = true)
    {
        _storedBlock = storedBlock;
        _owner = owner;
    }

    public void Dispose()
    {
        if (_storedBlock != null && _owner)
        {
            // NOTE(Jens): not sure if this shuold be called
            _storedBlock->Dispose(_storedBlock);
            LongtailLibrary.Longtail_Free(_storedBlock);
            _storedBlock = null;
        }
    }
}