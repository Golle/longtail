using Longtail.Internal;

namespace Longtail;

public unsafe class API
{
    public void ChangeVersion(
        string versionPath,
        BlockStoreApi blockStoreApi,
        StorageApi versionStorageApi,
        HashApi hashApi,
        JobApi jobApi,
        StoreIndex storeIndex,
        VersionIndex sourceVersion,
        VersionIndex targetVersion,
        VersionDiff versionDiff,
        bool retainPermissions = false,
        CancelApi? cancelApi = null,
        CancelToken? cancelToken = null,
        ProgressApi? progressApi = null)
    {
        using var path = new Utf8String(versionPath);
        var err = LongtailLibrary.Longtail_ChangeVersion(
            blockStoreApi.AsPointer(),
            versionStorageApi.AsPointer(),
            hashApi.AsPointer(),
            jobApi.AsPointer(),
            progressApi != null ? progressApi.AsPointer() : null,
            cancelApi != null ? cancelApi.AsPointer() : null,
            cancelToken != null ? cancelToken.AsPointer() : null,
            storeIndex.AsPointer(),
            sourceVersion.AsPointer(),
            targetVersion.AsPointer(),
            versionDiff.AsPointer(),
            path,
            retainPermissions ? 1 : 0
        );
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_ChangeVersion), err);
        }
    }
}