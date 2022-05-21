using static Longtail.Longtail_BlockStoreAPI_StatU64_Enum;

namespace Longtail;

public enum BlockStoreApiStats
{
    GetStoredBlock_Count = Longtail_BlockStoreAPI_StatU64_GetStoredBlock_Count,
    GetStoredBlock_RetryCount = Longtail_BlockStoreAPI_StatU64_GetStoredBlock_RetryCount,
    GetStoredBlock_FailCount = Longtail_BlockStoreAPI_StatU64_GetStoredBlock_FailCount,
    GetStoredBlock_Chunk_Count = Longtail_BlockStoreAPI_StatU64_GetStoredBlock_Chunk_Count,
    GetStoredBlock_Byte_Count = Longtail_BlockStoreAPI_StatU64_GetStoredBlock_Byte_Count,
    PutStoredBlock_Count = Longtail_BlockStoreAPI_StatU64_PutStoredBlock_Count,
    PutStoredBlock_RetryCount = Longtail_BlockStoreAPI_StatU64_PutStoredBlock_RetryCount,
    PutStoredBlock_FailCount = Longtail_BlockStoreAPI_StatU64_PutStoredBlock_FailCount,
    PutStoredBlock_Chunk_Count = Longtail_BlockStoreAPI_StatU64_PutStoredBlock_Chunk_Count,
    PutStoredBlock_Byte_Count = Longtail_BlockStoreAPI_StatU64_PutStoredBlock_Byte_Count,
    GetExistingContent_Count = Longtail_BlockStoreAPI_StatU64_GetExistingContent_Count,
    GetExistingContent_RetryCount = Longtail_BlockStoreAPI_StatU64_GetExistingContent_RetryCount,
    GetExistingContent_FailCount = Longtail_BlockStoreAPI_StatU64_GetExistingContent_FailCount,
    PruneBlocks_Count = Longtail_BlockStoreAPI_StatU64_PruneBlocks_Count,
    PruneBlocks_RetryCount = Longtail_BlockStoreAPI_StatU64_PruneBlocks_RetryCount,
    PruneBlocks_FailCount = Longtail_BlockStoreAPI_StatU64_PruneBlocks_FailCount,
    PreflightGet_Count = Longtail_BlockStoreAPI_StatU64_PreflightGet_Count,
    PreflightGet_RetryCount = Longtail_BlockStoreAPI_StatU64_PreflightGet_RetryCount,
    PreflightGet_FailCount = Longtail_BlockStoreAPI_StatU64_PreflightGet_FailCount,
    Flush_Count = Longtail_BlockStoreAPI_StatU64_Flush_Count,
    Flush_FailCount = Longtail_BlockStoreAPI_StatU64_Flush_FailCount,
    GetStats_Count = Longtail_BlockStoreAPI_StatU64_GetStats_Count,
    Count = Longtail_BlockStoreAPI_StatU64_Count,
}