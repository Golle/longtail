namespace Longtail;

public unsafe class StorageApiOpenFile : IDisposable
{
    private Longtail_StorageAPI* _storageApi;
    private Longtail_StorageAPI_OpenFile* _openFile;

    internal StorageApiOpenFile(Longtail_StorageAPI* storageApi, Longtail_StorageAPI_OpenFile* openFile)
    {
        _storageApi = storageApi;
        _openFile = openFile;
    }

    public ulong GetSize()
    {
        ulong size;
        var err = LongtailLibrary.Longtail_Storage_GetSize(_storageApi, _openFile, &size);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_Storage_GetSize), err);
        }
        return size;
    }

    public void Read(Span<byte> buffer, ulong offset = 0)
    {
        fixed (byte* pOutput = buffer)
        {
            var err = LongtailLibrary.Longtail_Storage_Read(_storageApi, _openFile, offset, (ulong)buffer.Length, pOutput);
            if (err != 0)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_Storage_Read), err);
            }
        }
    }

    public void Write(ulong offset, ReadOnlySpan<byte> buffer)
    {
        fixed (byte* pInput = buffer)
        {
            var err = LongtailLibrary.Longtail_Storage_Write(_storageApi, _openFile, offset, (ulong)buffer.Length, pInput);
            if (err != 0)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_Storage_Write), err);
            }
        }
    }

    public void SetSize(ulong length)
    {
        var err = LongtailLibrary.Longtail_Storage_SetSize(_storageApi, _openFile, length);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_Storage_SetSize), err);
        }
    }

    public void Dispose()
    {
        if (_openFile == null)
        {
            return;
        }

        LongtailLibrary.Longtail_Storage_CloseFile(_storageApi, _openFile);
        _openFile = null;
        _storageApi = null;
    }
}