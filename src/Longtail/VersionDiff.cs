namespace Longtail;

public sealed unsafe class VersionDiff : IDisposable
{
    private Longtail_VersionDiff* _versionDiff;
    internal VersionDiff(Longtail_VersionDiff* versionDiff)
    {
        _versionDiff = versionDiff;
    }

    public static VersionDiff? Create(HashApi hashApi, VersionIndex sourceVersion, VersionIndex targetVersion)
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
        return versionDiff != null ? new VersionDiff(versionDiff) : null;
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