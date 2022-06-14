using Longtail.Internal;

namespace Longtail;

public unsafe class StorageApi : IDisposable
{
    private Longtail_StorageAPI* _storageApi;

    private StorageApi(Longtail_StorageAPI* storageApi)
    {
        _storageApi = storageApi;
    }

    internal Longtail_StorageAPI* AsPointer() => _storageApi;
    public static StorageApi CreateFSStorageAPI()
    {
        var api = LongtailLibrary.Longtail_CreateFSStorageAPI();
        return api != null ? new StorageApi(api) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_CreateFSStorageAPI)} returned a null pointer");
    }

    public static StorageApi CreateInMemoryStorageAPI()
    {
        var api = LongtailLibrary.Longtail_CreateInMemStorageAPI();
        return api != null ? new StorageApi(api) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_CreateInMemStorageAPI)} returned a null pointer");
    }

    public void WriteBlockIndex(string path, in BlockIndex blockIndex)
    {
        using var utf8Path = new Utf8String(path);
        var err = LongtailLibrary.Longtail_WriteBlockIndex(_storageApi, blockIndex.AsPointer(), utf8Path);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_WriteBlockIndex), err);
        }
    }

    public BlockIndex ReadBlockIndex(string path)
    {
        using var utf8Path = new Utf8String(path);
        Longtail_BlockIndex* blockIndex;
        var err = LongtailLibrary.Longtail_ReadBlockIndex(_storageApi, utf8Path, &blockIndex);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_ReadBlockIndex), err);
        }
        return blockIndex != null ? new BlockIndex(blockIndex) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_ReadBlockIndex)} returned a null pointer");
    }

    public StorageApiOpenFile? OpenReadFile(string path)
    {
        using var utf8Path = new Utf8String(path);
        Longtail_StorageAPI_OpenFile* openFile;
        var err = LongtailLibrary.Longtail_Storage_OpenReadFile(_storageApi, utf8Path, &openFile);
        if (err == ErrorCodes.ENOENT)
        {
            return null;
        }
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_Storage_OpenReadFile), err);
        }
        return openFile != null ? new StorageApiOpenFile(_storageApi, openFile) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_Storage_OpenReadFile)} returned a null pointer");

    }
    
    //public static extern int Longtail_Storage_OpenWriteFile(
    //    Longtail_StorageAPI* storage_api,
    //    byte* path,
    //    ulong initial_size,
    //    Longtail_StorageAPI_OpenFile** out_open_file
    //);

    //public static extern int Longtail_Storage_Write(
    //    Longtail_StorageAPI* storage_api,
    //    Longtail_StorageAPI_OpenFile* f,
    //    ulong offset,
    //    ulong length,
    //    void* input
    //);

    //public static extern int Longtail_Storage_SetSize(
    //    Longtail_StorageAPI* storage_api,
    //    Longtail_StorageAPI_OpenFile* f,
    //    ulong length
    //);



    public void Dispose()
    {
        if (_storageApi != null)
        {
            LongtailLibrary.Longtail_DisposeAPI(&_storageApi->m_API);
            _storageApi = null;
        }
    }
}