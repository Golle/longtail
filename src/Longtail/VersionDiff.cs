namespace Longtail;

public sealed unsafe class VersionDiff : IDisposable
{
    private Longtail_VersionDiff* _versionDiff;
    internal Longtail_VersionDiff* AsPointer() => _versionDiff;

    public uint SourceRemovedCount => *_versionDiff->m_SourceRemovedCount;
    public uint TargetAddedCount=> *_versionDiff->m_TargetAddedCount;
    public uint ModifiedContentCount=> *_versionDiff->m_ModifiedContentCount;
    public uint ModifiedPermissionsCount=> *_versionDiff->m_ModifiedPermissionsCount;
    public uint SourceRemovedAssetIndexes=> *_versionDiff->m_SourceRemovedAssetIndexes;
    public uint TargetAddedAssetIndexes=> *_versionDiff->m_TargetAddedAssetIndexes;
    public uint SourceContentModifiedAssetIndexes=> *_versionDiff->m_SourceContentModifiedAssetIndexes;
    public uint TargetContentModifiedAssetIndexes=> *_versionDiff->m_TargetContentModifiedAssetIndexes;
    public uint SourcePermissionsModifiedAssetIndexes=> *_versionDiff->m_SourcePermissionsModifiedAssetIndexes;
    public uint TargetPermissionsModifiedAssetIndexes => *_versionDiff->m_TargetPermissionsModifiedAssetIndexes;

    internal VersionDiff(Longtail_VersionDiff* versionDiff)
    {
        if (versionDiff == null)
        {
            throw new ArgumentNullException(nameof(versionDiff));
        }
        _versionDiff = versionDiff;
    }
    
    public static VersionDiff Create(HashApi hashApi, VersionIndex sourceVersion, VersionIndex targetVersion)
    {
        Longtail_VersionDiff* versionDiff = default;
        var err = LongtailLibrary.Longtail_CreateVersionDiff(
            hashApi.AsPointer(),
            sourceVersion.AsPointer(),
            targetVersion.AsPointer(),
            &versionDiff
        );
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_CreateVersionDiff), err);
        }
        return versionDiff != null ? new VersionDiff(versionDiff) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_CreateVersionDiff)} returned a null pointer");
    }

    public void Dispose()
    {
        if (_versionDiff != null)
        {
            LongtailLibrary.Longtail_Free(_versionDiff);
            _versionDiff = null;
        }
    }
}