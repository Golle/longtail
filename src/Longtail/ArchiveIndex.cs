using Longtail.Internal;

namespace Longtail;

//TODO(Jens): Add tests for these when we can create StoreIndex in code
public unsafe class ArchiveIndex : IDisposable
{
    private Longtail_ArchiveIndex* _archiveIndex;
    internal Longtail_ArchiveIndex* AsPointer() => _archiveIndex;

    internal ArchiveIndex(Longtail_ArchiveIndex* archiveIndex)
    {
        _archiveIndex = archiveIndex;
    }

    public static ArchiveIndex? CreateArchiveIndex(StoreIndex storeIndex, VersionIndex versionIndex)
    {
        Longtail_ArchiveIndex* archiveIndex;
        var err = LongtailLibrary.Longtail_CreateArchiveIndex(storeIndex.AsPointer(), versionIndex.AsPointer(), &archiveIndex);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_CreateArchiveIndex), err);
        }
        return archiveIndex != null ? new ArchiveIndex(archiveIndex) : null;
    }

    public static ArchiveIndex? ReadArchiveIndex(string path, StorageApi storageApi)
    {
        using var utf8Path = new Utf8String(path);
        Longtail_ArchiveIndex* archiveIndex;
        var err = LongtailLibrary.Longtail_ReadArchiveIndex(storageApi.AsPointer(), utf8Path, &archiveIndex);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_ReadArchiveIndex), err);
        }
        return archiveIndex != null ? new ArchiveIndex(archiveIndex) : null;
    }

    public void Dispose()
    {
        if (_archiveIndex != null)
        {
            LongtailLibrary.Longtail_Free(_archiveIndex);
            _archiveIndex = null;
        }
    }
}