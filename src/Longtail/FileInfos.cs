using Longtail.Internal;

namespace Longtail;

public unsafe class FileInfos : IDisposable
{
    private Longtail_FileInfos* _fileInfos;

    private FileInfos(Longtail_FileInfos* fileInfos)
    {
        _fileInfos = fileInfos;
    }

    internal Longtail_FileInfos* AsPointer() => _fileInfos;
    public uint GetCount() => LongtailLibrary.Longtail_FileInfos_GetCount(_fileInfos);
    public string GetPath(uint index)
    {
        var path = LongtailLibrary.Longtail_FileInfos_GetPath(_fileInfos, index);
        return Utf8String.GetString(path);
    }
    public ulong GetSize(uint index) => LongtailLibrary.Longtail_FileInfos_GetSize(_fileInfos, index);
    public ushort GetPermissions(uint index)
    {
        var permission = LongtailLibrary.Longtail_FileInfos_GetPermissions(_fileInfos, index);
        return permission != null ? *permission : (ushort)0;
    }

    // NOTE(Jens): this methods seems to be missing an implementation in the library
    //public static extern Longtail_Paths* Longtail_FileInfos_GetPaths(
    //    Longtail_FileInfos* file_infos
    //);

    public static FileInfos GetFilesRecursively(string rootPath, StorageApi storageApi, PathFilterApi? pathFilter = null, CancelApi? cancelApi = null, CancelToken? cancelToken = null)
    {
        using var path = new Utf8String(rootPath);

        Longtail_FileInfos* fileInfos;
        var err = LongtailLibrary.Longtail_GetFilesRecursively(
            storageApi.AsPointer(),
            pathFilter != null ? pathFilter.AsPointer() : null,
            cancelApi != null ? cancelApi.AsPointer() : null,
            cancelToken != null ? cancelToken.AsPointer() : null,
            path,
            &fileInfos
        );

        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_GetFilesRecursively), err);
        }

        return fileInfos != null ? new FileInfos(fileInfos) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_GetFilesRecursively)} returned a null pointer");
    }

    public void Dispose()
    {
        if (_fileInfos != null)
        {
            LongtailLibrary.Longtail_Free(_fileInfos);
            _fileInfos = null;
        }
    }
}