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

    public StorageApiOpenFile? OpenWriteFile(string path, ulong initialState)
    {
        using var utf8Path = new Utf8String(path);
        Longtail_StorageAPI_OpenFile* file;
        var err = LongtailLibrary.Longtail_Storage_OpenWriteFile(_storageApi, utf8Path, initialState, &file);
        if (err == ErrorCodes.ENOENT)
        {
            return null;
        }

        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_Storage_OpenWriteFile), err);
        }
        return file != null ? new StorageApiOpenFile(_storageApi, file) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_Storage_OpenWriteFile)} returned a null pointer");
    }

    public void SetPermissions(string path, ushort permissions)
    {
        using var utf8Path = new Utf8String(path);
        var err = LongtailLibrary.Longtail_Storage_SetPermissions(_storageApi, utf8Path, permissions);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_Storage_SetPermissions), err);
        }
    }

    public ushort GetPermissions(string path)
    {
        using var utf8Path = new Utf8String(path);
        ushort permissions;
        var err = LongtailLibrary.Longtail_Storage_GetPermissions(_storageApi, utf8Path, &permissions);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_Storage_GetPermissions), err);
        }
        return permissions;
    }

    public void CreateDir(string path)
    {
        using var utf8Path = new Utf8String(path);
        var err = LongtailLibrary.Longtail_Storage_CreateDir(_storageApi, utf8Path);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_Storage_CreateDir), err);
        }
    }

    public void RenameFile(string sourcePath, string targetPath)
    {
        using var source = new Utf8String(sourcePath);
        using var target = new Utf8String(targetPath);
        var err = LongtailLibrary.Longtail_Storage_RenameFile(_storageApi, source, target);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_Storage_RenameFile), err);
        }
    }

    public string ConcatPath(string rootPath, string subPath)
    {
        using var root = new Utf8String(rootPath);
        using var sub = new Utf8String(subPath);
        var result = LongtailLibrary.Longtail_Storage_ConcatPath(_storageApi, root, sub);
        return Utf8String.GetString(result);
    }

    public string GetParentPath(string path)
    {
        using var utf8Path = new Utf8String(path);
        var parentPath = LongtailLibrary.Longtail_Storage_GetParentPath(_storageApi, utf8Path);
        return Utf8String.GetString(parentPath);
    }

    public bool IsDir(string path)
    {
        using var utf8Path = new Utf8String(path);
        return LongtailLibrary.Longtail_Storage_IsDir(_storageApi, utf8Path) != 0;
    }

    public bool IsFile(string path)
    {
        using var utf8Path = new Utf8String(path);
        return LongtailLibrary.Longtail_Storage_IsFile(_storageApi, utf8Path) != 0;
    }

    public void RemoveDir(string path)
    {
        using var utf8Path = new Utf8String(path);
        var err = LongtailLibrary.Longtail_Storage_RemoveDir(_storageApi, utf8Path);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_Storage_RemoveDir), err);
        }
    }

    public void RemoveFile(string path)
    {
        using var utf8Path = new Utf8String(path);
        var err = LongtailLibrary.Longtail_Storage_RemoveFile(_storageApi, utf8Path);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_Storage_RemoveFile), err);
        }
    }

    public void Dispose()
    {
        if (_storageApi != null)
        {
            LongtailLibrary.Longtail_DisposeAPI(&_storageApi->m_API);
            _storageApi = null;
        }
    }
}