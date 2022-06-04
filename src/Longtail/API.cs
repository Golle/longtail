using Longtail.Internal;

namespace Longtail;

public static unsafe class API
{
    public static void ChangeVersion(
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


    public static void WriteContent(string assetsFolder,
        StoreIndex storeIndex,
        VersionIndex versionIndex,
        StorageApi storageApi,
        BlockStoreApi blockStoreApi,
        JobApi jobApi,
        ProgressApi? progressApi = null,
        CancelApi? cancelApi = null,
        CancelToken? cancelToken = null)
    {
        using var path = new Utf8String(assetsFolder);
        var err = LongtailLibrary.Longtail_WriteContent(
            storageApi.AsPointer(),
            blockStoreApi.AsPointer(),
            jobApi.AsPointer(),
            progressApi != null ? progressApi.AsPointer() : null,
            cancelApi != null ? cancelApi.AsPointer() : null,
            cancelToken != null ? cancelToken.AsPointer() : null,
            storeIndex.AsPointer(),
            versionIndex.AsPointer(),
            path
        );
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_WriteContent), err);
        }
    }

    public static void WriteVersion(
        string versionPath, 
        BlockStoreApi blockStoreApi, 
        StorageApi versionStorageApi, 
        StoreIndex storeIndex, 
        VersionIndex versionIndex, 
        JobApi jobApi, 
        bool retainPermissions, 
        ProgressApi? progressApi = null, 
        CancelApi? cancelApi = null, 
        CancelToken? cancelToken = null)
    {
        using var path = new Utf8String(versionPath);
        var err = LongtailLibrary.Longtail_WriteVersion(
            blockStoreApi.AsPointer(),
            versionStorageApi.AsPointer(),
            jobApi.AsPointer(),
            progressApi != null ? progressApi.AsPointer() : null,
            cancelApi != null ? cancelApi.AsPointer() : null,
            cancelToken != null ? cancelToken.AsPointer() : null,
            storeIndex.AsPointer(),
            versionIndex.AsPointer(),
            path,
            retainPermissions ? 1 : 0
        );
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_WriteVersion), err);
        }
    }
}