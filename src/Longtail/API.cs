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
    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //public static extern int Longtail_WriteContent(
    //    Longtail_StorageAPI* source_storage_api,
    //    Longtail_BlockStoreAPI* block_store_api,
    //    Longtail_JobAPI* job_api,
    //    Longtail_ProgressAPI* progress_api,
    //    Longtail_CancelAPI* optional_cancel_api,
    //    Longtail_CancelAPI_CancelToken* optional_cancel_token,
    //    Longtail_StoreIndex* store_index,
    //    Longtail_VersionIndex* version_index,
    //    byte* assets_folder
    //);

    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //public static extern int Longtail_CreateMissingContent(
    //    Longtail_HashAPI* hash_api,
    //    Longtail_StoreIndex* store_index,
    //    Longtail_VersionIndex* version_index,
    //    uint max_block_size,
    //    uint max_chunks_per_block,
    //    Longtail_StoreIndex** out_store_index
    //);

    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //public static extern int Longtail_GetMissingChunks(
    //    Longtail_StoreIndex* store_index,
    //    uint chunk_count,
    //    ulong* chunk_hashes,
    //    uint* out_chunk_count,
    //    ulong* out_missing_chunk_hashes
    //);

    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //public static extern int Longtail_WriteVersion(
    //    Longtail_BlockStoreAPI* block_storage_api,
    //    Longtail_StorageAPI* version_storage_api,
    //    Longtail_JobAPI* job_api,
    //    Longtail_ProgressAPI* progress_api,
    //    Longtail_CancelAPI* optional_cancel_api,
    //    Longtail_CancelAPI_CancelToken* optional_cancel_token,
    //    Longtail_StoreIndex* store_index,
    //    Longtail_VersionIndex* version_index,
    //    byte* version_path,
    //    int retain_permissions
    //);
}