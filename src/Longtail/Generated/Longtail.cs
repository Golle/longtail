namespace Longtail;

internal unsafe struct Longtail_API
{
    /// <summary>
    /// void Longtail_DisposeFunc(struct Longtail_API* api)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_API*, void> Dispose;
}
internal unsafe struct Longtail_CancelAPI
{
    public Longtail_API m_API;
    /// <summary>
    /// int Longtail_CancelAPI_CreateTokenFunc(struct Longtail_CancelAPI* cancel_api, struct Longtail_CancelAPI_CancelToken** out_token)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken**, int> CreateToken;
    /// <summary>
    /// int Longtail_CancelAPI_CancelFunc(struct Longtail_CancelAPI* cancel_api, struct Longtail_CancelAPI_CancelToken* token)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken*, int> Cancel;
    /// <summary>
    /// int Longtail_CancelAPI_IsCancelledFunc(struct Longtail_CancelAPI* cancel_api, struct Longtail_CancelAPI_CancelToken* token)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken*, int> IsCancelled;
    /// <summary>
    /// int Longtail_CancelAPI_DisposeTokenFunc(struct Longtail_CancelAPI* cancel_api, struct Longtail_CancelAPI_CancelToken* token)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken*, int> DisposeToken;
}
internal unsafe struct Longtail_PathFilterAPI
{
    public Longtail_API m_API;
    /// <summary>
    /// int Longtail_PathFilter_IncludeFunc(struct Longtail_PathFilterAPI* path_filter_api, const char* root_path, const char* asset_path, const char* asset_name, int is_dir, unsigned long long int size, unsigned short int permissions)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_PathFilterAPI*, byte*, byte*, byte*, int, ulong, ushort, int> Include;
}
internal unsafe struct Longtail_HashAPI
{
    public Longtail_API m_API;
    /// <summary>
    /// unsigned int Longtail_Hash_GetIdentifierFunc(struct Longtail_HashAPI* hash_api)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_HashAPI*, uint> GetIdentifier;
    /// <summary>
    /// int Longtail_Hash_BeginContextFunc(struct Longtail_HashAPI* hash_api, struct Longtail_HashAPI_Context** out_context)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_HashAPI*, Longtail_HashAPI_Context**, int> BeginContext;
    /// <summary>
    /// void Longtail_Hash_HashFunc(struct Longtail_HashAPI* hash_api, struct Longtail_HashAPI_Context* context, unsigned int length, const void* data)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_HashAPI*, Longtail_HashAPI_Context*, uint, void*, void> Hash;
    /// <summary>
    /// unsigned long long int Longtail_Hash_EndContextFunc(struct Longtail_HashAPI* hash_api, struct Longtail_HashAPI_Context* context)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_HashAPI*, Longtail_HashAPI_Context*, ulong> EndContext;
    /// <summary>
    /// int Longtail_Hash_HashBufferFunc(struct Longtail_HashAPI* hash_api, unsigned int length, const void* data, unsigned long long int* out_hash)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_HashAPI*, uint, void*, ulong*, int> HashBuffer;
}
internal unsafe struct Longtail_HashRegistryAPI
{
    public Longtail_API m_API;
    /// <summary>
    /// int Longtail_HashRegistry_GetHashAPIFunc(struct Longtail_HashRegistryAPI* hash_registry, unsigned int hash_type, struct Longtail_HashAPI** out_hash_api)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_HashRegistryAPI*, uint, Longtail_HashAPI**, int> GetHashAPI;
}
internal unsafe struct Longtail_CompressionAPI
{
    public Longtail_API m_API;
    /// <summary>
    /// unsigned long long int Longtail_CompressionAPI_GetMaxCompressedSizeFunc(struct Longtail_CompressionAPI* compression_api, unsigned int settings_id, unsigned long long int size)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_CompressionAPI*, uint, ulong, ulong> GetMaxCompressedSize;
    /// <summary>
    /// int Longtail_CompressionAPI_CompressFunc(struct Longtail_CompressionAPI* compression_api, unsigned int settings_id, const char* uncompressed, char* compressed, unsigned long long int uncompressed_size, unsigned long long int max_compressed_size, unsigned long long int* out_compressed_size)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_CompressionAPI*, uint, byte*, byte*, ulong, ulong, ulong*, int> Compress;
    /// <summary>
    /// int Longtail_CompressionAPI_DecompressFunc(struct Longtail_CompressionAPI* compression_api, const char* compressed, char* uncompressed, unsigned long long int compressed_size, unsigned long long int max_uncompressed_size, unsigned long long int* out_uncompressed_size)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_CompressionAPI*, byte*, byte*, ulong, ulong, ulong*, int> Decompress;
}
internal unsafe struct Longtail_CompressionRegistryAPI
{
    public Longtail_API m_API;
    /// <summary>
    /// int Longtail_CompressionRegistry_GetCompressionAPIFunc(struct Longtail_CompressionRegistryAPI* compression_registry, unsigned int compression_type, struct Longtail_CompressionAPI** out_compression_api, unsigned int* out_settings_id)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_CompressionRegistryAPI*, uint, Longtail_CompressionAPI**, uint*, int> GetCompressionAPI;
}
internal enum Longtail_StorageAPI_Enum
{
    Longtail_StorageAPI_OtherExecuteAccess = 0001,
    Longtail_StorageAPI_OtherWriteAccess = 0002,
    Longtail_StorageAPI_OtherReadAccess = 0004,
    Longtail_StorageAPI_GroupExecuteAccess = 0010,
    Longtail_StorageAPI_GroupWriteAccess = 0020,
    Longtail_StorageAPI_GroupReadAccess = 0040,
    Longtail_StorageAPI_UserExecuteAccess = 0100,
    Longtail_StorageAPI_UserWriteAccess = 0200,
    Longtail_StorageAPI_UserReadAccess = 0400,
}
internal unsafe struct Longtail_StorageAPI_EntryProperties
{
    public byte* m_Name;
    public ulong m_Size;
    public ushort m_Permissions;
    public int m_IsDir;
}
internal unsafe struct Longtail_StorageAPI
{
    public Longtail_API m_API;
    /// <summary>
    /// int Longtail_Storage_OpenReadFileFunc(struct Longtail_StorageAPI* storage_api, const char* path, struct Longtail_StorageAPI_OpenFile** out_open_file)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, Longtail_StorageAPI_OpenFile**, int> OpenReadFile;
    /// <summary>
    /// int Longtail_Storage_GetSizeFunc(struct Longtail_StorageAPI* storage_api, struct Longtail_StorageAPI_OpenFile* f, unsigned long long int* out_size)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_OpenFile*, ulong*, int> GetSize;
    /// <summary>
    /// int Longtail_Storage_ReadFunc(struct Longtail_StorageAPI* storage_api, struct Longtail_StorageAPI_OpenFile* f, unsigned long long int offset, unsigned long long int length, void* output)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_OpenFile*, ulong, ulong, void*, int> Read;
    /// <summary>
    /// int Longtail_Storage_OpenWriteFileFunc(struct Longtail_StorageAPI* storage_api, const char* path, unsigned long long int initial_size, struct Longtail_StorageAPI_OpenFile** out_open_file)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, ulong, Longtail_StorageAPI_OpenFile**, int> OpenWriteFile;
    /// <summary>
    /// int Longtail_Storage_WriteFunc(struct Longtail_StorageAPI* storage_api, struct Longtail_StorageAPI_OpenFile* f, unsigned long long int offset, unsigned long long int length, const void* input)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_OpenFile*, ulong, ulong, void*, int> Write;
    /// <summary>
    /// int Longtail_Storage_SetSizeFunc(struct Longtail_StorageAPI* storage_api, struct Longtail_StorageAPI_OpenFile* f, unsigned long long int length)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_OpenFile*, ulong, int> SetSize;
    /// <summary>
    /// int Longtail_Storage_SetPermissionsFunc(struct Longtail_StorageAPI* storage_api, const char* path, unsigned short int permissions)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, ushort, int> SetPermissions;
    /// <summary>
    /// int Longtail_Storage_GetPermissionsFunc(struct Longtail_StorageAPI* storage_api, const char* path, unsigned short int* out_permissions)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, ushort*, int> GetPermissions;
    /// <summary>
    /// void Longtail_Storage_CloseFileFunc(struct Longtail_StorageAPI* storage_api, struct Longtail_StorageAPI_OpenFile* f)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_OpenFile*, void> CloseFile;
    /// <summary>
    /// int Longtail_Storage_CreateDirFunc(struct Longtail_StorageAPI* storage_api, const char* path)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, int> CreateDir;
    /// <summary>
    /// int Longtail_Storage_RenameFileFunc(struct Longtail_StorageAPI* storage_api, const char* source_path, const char* target_path)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, byte*, int> RenameFile;
    /// <summary>
    /// char* Longtail_Storage_ConcatPathFunc(struct Longtail_StorageAPI* storage_api, const char* root_path, const char* sub_path)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, byte*, byte*> ConcatPath;
    /// <summary>
    /// int Longtail_Storage_IsDirFunc(struct Longtail_StorageAPI* storage_api, const char* path)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, int> IsDir;
    /// <summary>
    /// int Longtail_Storage_IsFileFunc(struct Longtail_StorageAPI* storage_api, const char* path)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, int> IsFile;
    /// <summary>
    /// int Longtail_Storage_RemoveDirFunc(struct Longtail_StorageAPI* storage_api, const char* path)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, int> RemoveDir;
    /// <summary>
    /// int Longtail_Storage_RemoveFileFunc(struct Longtail_StorageAPI* storage_api, const char* path)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, int> RemoveFile;
    /// <summary>
    /// int Longtail_Storage_StartFindFunc(struct Longtail_StorageAPI* storage_api, const char* path, struct Longtail_StorageAPI_Iterator** out_iterator)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, Longtail_StorageAPI_Iterator**, int> StartFind;
    /// <summary>
    /// int Longtail_Storage_FindNextFunc(struct Longtail_StorageAPI* storage_api, struct Longtail_StorageAPI_Iterator* iterator)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_Iterator*, int> FindNext;
    /// <summary>
    /// void Longtail_Storage_CloseFindFunc(struct Longtail_StorageAPI* storage_api, struct Longtail_StorageAPI_Iterator* iterator)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_Iterator*, void> CloseFind;
    /// <summary>
    /// int Longtail_Storage_GetEntryPropertiesFunc(struct Longtail_StorageAPI* storage_api, struct Longtail_StorageAPI_Iterator* iterator, struct Longtail_StorageAPI_EntryProperties* out_properties)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_Iterator*, Longtail_StorageAPI_EntryProperties*, int> GetEntryProperties;
    /// <summary>
    /// int Longtail_Storage_LockFileFunc(struct Longtail_StorageAPI* storage_api, const char* path, struct Longtail_StorageAPI_LockFile** out_lock_file)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, Longtail_StorageAPI_LockFile**, int> LockFile;
    /// <summary>
    /// int Longtail_Storage_UnlockFileFunc(struct Longtail_StorageAPI* storage_api, struct Longtail_StorageAPI_LockFile* file_lock)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_LockFile*, int> UnlockFile;
    /// <summary>
    /// char* Longtail_Storage_GetParentPathFunc(struct Longtail_StorageAPI* storage_api, const char* path)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, byte*> GetParentPath;
    /// <summary>
    /// int Longtail_Storage_MapFileFunc(struct Longtail_StorageAPI* storage_api, struct Longtail_StorageAPI_OpenFile* f, unsigned long long int offset, unsigned long long int length, struct Longtail_StorageAPI_FileMap** out_file_map, const void** out_data_ptr)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_OpenFile*, ulong, ulong, Longtail_StorageAPI_FileMap**, void**, int> MapFile;
    /// <summary>
    /// void Longtail_Storage_UnmapFileFunc(struct Longtail_StorageAPI* storage_api, struct Longtail_StorageAPI_FileMap* m)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_FileMap*, void> UnMapFile;
    /// <summary>
    /// int Longtail_Storage_OpenAppendFileFunc(struct Longtail_StorageAPI* storage_api, const char* path, struct Longtail_StorageAPI_OpenFile** out_open_file)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, Longtail_StorageAPI_OpenFile**, int> OpenAppendFile;
}
internal unsafe struct Longtail_ConcurrentChunkWriteAPI
{
    public Longtail_API m_API;
    /// <summary>
    /// int Longtail_ConcurrentChunkWrite_CreateDirFunc(struct Longtail_ConcurrentChunkWriteAPI* concurrent_file_write_api, unsigned int asset_index)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_ConcurrentChunkWriteAPI*, uint, int> CreateDir;
    /// <summary>
    /// int Longtail_ConcurrentChunkWrite_OpenFunc(struct Longtail_ConcurrentChunkWriteAPI* concurrent_file_write_api, unsigned int asset_index)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_ConcurrentChunkWriteAPI*, uint, int> Open;
    /// <summary>
    /// void Longtail_ConcurrentChunkWrite_CloseFunc(struct Longtail_ConcurrentChunkWriteAPI* concurrent_file_write_api, unsigned int asset_index)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_ConcurrentChunkWriteAPI*, uint, void> Close;
    /// <summary>
    /// int Longtail_ConcurrentChunkWrite_WriteFunc(struct Longtail_ConcurrentChunkWriteAPI* concurrent_file_write_api, unsigned int asset_index, unsigned long long int offset, unsigned int size, const void* input)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_ConcurrentChunkWriteAPI*, uint, ulong, uint, void*, int> Write;
    /// <summary>
    /// int Longtail_ConcurrentChunkWrite_FlushFunc(struct Longtail_ConcurrentChunkWriteAPI* concurrent_file_write_api)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_ConcurrentChunkWriteAPI*, int> Flush;
}
internal unsafe struct Longtail_ProgressAPI
{
    public Longtail_API m_API;
    /// <summary>
    /// void Longtail_Progress_OnProgressFunc(struct Longtail_ProgressAPI* progressAPI, unsigned int total_count, unsigned int done_count)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_ProgressAPI*, uint, uint, void> OnProgress;
}
internal unsafe struct Longtail_JobAPI
{
    public Longtail_API m_API;
    /// <summary>
    /// unsigned int Longtail_Job_GetWorkerCountFunc(struct Longtail_JobAPI* job_api)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_JobAPI*, uint> GetWorkerCount;
    /// <summary>
    /// int Longtail_Job_ReserveJobsFunc(struct Longtail_JobAPI* job_api, unsigned int job_count, void** out_job_group)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_JobAPI*, uint, void**, int> ReserveJobs;
    /// <summary>
    /// int Longtail_Job_CreateJobsFunc(struct Longtail_JobAPI* job_api, void* job_group, struct Longtail_ProgressAPI* progressAPI, struct Longtail_CancelAPI* optional_cancel_api, struct Longtail_CancelAPI_CancelToken* optional_cancel_token, unsigned int job_count, int Longtail_JobAPI_JobFunc(void* context, unsigned int job_id, int detected_error)* job_funcs, void** job_contexts, char job_channel, void** out_jobs)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_JobAPI*, void*, Longtail_ProgressAPI*, Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken*, uint, delegate* unmanaged[Cdecl]<void*, uint, int, int>*, void**, byte, void**, int> CreateJobs;
    /// <summary>
    /// int Longtail_Job_AddDependeciesFunc(struct Longtail_JobAPI* job_api, unsigned int job_count, void* jobs, unsigned int dependency_job_count, void* dependency_jobs)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_JobAPI*, uint, void*, uint, void*, int> AddDependecies;
    /// <summary>
    /// int Longtail_Job_ReadyJobsFunc(struct Longtail_JobAPI* job_api, unsigned int job_count, void* jobs)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_JobAPI*, uint, void*, int> ReadyJobs;
    /// <summary>
    /// int Longtail_Job_WaitForAllJobsFunc(struct Longtail_JobAPI* job_api, void* job_group, struct Longtail_ProgressAPI* progressAPI, struct Longtail_CancelAPI* optional_cancel_api, struct Longtail_CancelAPI_CancelToken* optional_cancel_token)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_JobAPI*, void*, Longtail_ProgressAPI*, Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken*, int> WaitForAllJobs;
    /// <summary>
    /// int Longtail_Job_ResumeJobFunc(struct Longtail_JobAPI* job_api, unsigned int job_id)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_JobAPI*, uint, int> ResumeJob;
    /// <summary>
    /// int Longtail_Job_GetMaxBatchCountFunc(struct Longtail_JobAPI* job_api, unsigned int* out_max_job_batch_count, unsigned int* out_max_dependency_batch_count)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_JobAPI*, uint*, uint*, int> GetMaxBatchCount;
}
internal unsafe struct Longtail_Chunker_ChunkRange
{
    public byte* buf;
    public ulong offset;
    public uint len;
}
internal unsafe struct Longtail_ChunkerAPI
{
    public Longtail_API m_API;
    /// <summary>
    /// int Longtail_Chunker_GetMinChunkSizeFunc(struct Longtail_ChunkerAPI* chunker_api, unsigned int* out_min_chunk_size)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_ChunkerAPI*, uint*, int> GetMinChunkSize;
    /// <summary>
    /// int Longtail_Chunker_CreateChunkerFunc(struct Longtail_ChunkerAPI* chunker_api, unsigned int min_chunk_size, unsigned int avg_chunk_size, unsigned int max_chunk_size, struct Longtail_ChunkerAPI_Chunker** out_chunker)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_ChunkerAPI*, uint, uint, uint, Longtail_ChunkerAPI_Chunker**, int> CreateChunker;
    /// <summary>
    /// int Longtail_Chunker_NextChunkFunc(struct Longtail_ChunkerAPI* chunker_api, struct Longtail_ChunkerAPI_Chunker* chunker, int Longtail_Chunker_Feeder(void* context, struct Longtail_ChunkerAPI_Chunker* chunker, unsigned int requested_size, char* buffer, unsigned int* out_size) feeder, void* feeder_context, struct Longtail_Chunker_ChunkRange* out_chunk_range)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_ChunkerAPI*, Longtail_ChunkerAPI_Chunker*, delegate* unmanaged[Cdecl]<void*, Longtail_ChunkerAPI_Chunker*, uint, byte*, uint*, int>, void*, Longtail_Chunker_ChunkRange*, int> NextChunk;
    /// <summary>
    /// int Longtail_Chunker_DisposeChunkerFunc(struct Longtail_ChunkerAPI* chunker_api, struct Longtail_ChunkerAPI_Chunker* chunker)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_ChunkerAPI*, Longtail_ChunkerAPI_Chunker*, int> DisposeChunker;
    /// <summary>
    /// int Longtail_Chunker_NextChunkFromBufferFunc(struct Longtail_ChunkerAPI* chunker_api, struct Longtail_ChunkerAPI_Chunker* chunker, const void* buffer, unsigned long long int buffer_size, const void** out_next_chunk_start)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_ChunkerAPI*, Longtail_ChunkerAPI_Chunker*, void*, ulong, void**, int> NextChunkFromBuffer;
}
internal unsafe struct Longtail_AsyncPutStoredBlockAPI
{
    public Longtail_API m_API;
    /// <summary>
    /// void Longtail_AsyncPutStoredBlock_OnCompleteFunc(struct Longtail_AsyncPutStoredBlockAPI* async_complete_api, int err)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_AsyncPutStoredBlockAPI*, int, void> OnComplete;
}
internal unsafe struct Longtail_AsyncGetStoredBlockAPI
{
    public Longtail_API m_API;
    /// <summary>
    /// void Longtail_AsyncGetStoredBlock_OnCompleteFunc(struct Longtail_AsyncGetStoredBlockAPI* async_complete_api, struct Longtail_StoredBlock* stored_block, int err)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_AsyncGetStoredBlockAPI*, Longtail_StoredBlock*, int, void> OnComplete;
}
internal unsafe struct Longtail_AsyncGetExistingContentAPI
{
    public Longtail_API m_API;
    /// <summary>
    /// void Longtail_AsyncGetExistingContent_OnCompleteFunc(struct Longtail_AsyncGetExistingContentAPI* async_complete_api, struct Longtail_StoreIndex* store_index, int err)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_AsyncGetExistingContentAPI*, Longtail_StoreIndex*, int, void> OnComplete;
}
internal unsafe struct Longtail_AsyncPruneBlocksAPI
{
    public Longtail_API m_API;
    /// <summary>
    /// void Longtail_AsyncPruneBlocks_OnCompleteFunc(struct Longtail_AsyncPruneBlocksAPI* async_complete_api, unsigned int pruned_block_count, int err)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_AsyncPruneBlocksAPI*, uint, int, void> OnComplete;
}
internal unsafe struct Longtail_AsyncPreflightStartedAPI
{
    public Longtail_API m_API;
    /// <summary>
    /// void Longtail_AsyncPreflightStarted_OnCompleteFunc(struct Longtail_AsyncPreflightStartedAPI* async_complete_api, unsigned int block_count, unsigned long long int* block_hashes, int err)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_AsyncPreflightStartedAPI*, uint, ulong*, int, void> OnComplete;
}
internal unsafe struct Longtail_AsyncFlushAPI
{
    public Longtail_API m_API;
    /// <summary>
    /// void Longtail_AsyncFlush_OnCompleteFunc(struct Longtail_AsyncFlushAPI* async_complete_api, int err)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_AsyncFlushAPI*, int, void> OnComplete;
}
internal enum Longtail_BlockStoreAPI_StatU64_Enum
{
    Longtail_BlockStoreAPI_StatU64_GetStoredBlock_Count,
    Longtail_BlockStoreAPI_StatU64_GetStoredBlock_RetryCount,
    Longtail_BlockStoreAPI_StatU64_GetStoredBlock_FailCount,
    Longtail_BlockStoreAPI_StatU64_GetStoredBlock_Chunk_Count,
    Longtail_BlockStoreAPI_StatU64_GetStoredBlock_Byte_Count,
    Longtail_BlockStoreAPI_StatU64_PutStoredBlock_Count,
    Longtail_BlockStoreAPI_StatU64_PutStoredBlock_RetryCount,
    Longtail_BlockStoreAPI_StatU64_PutStoredBlock_FailCount,
    Longtail_BlockStoreAPI_StatU64_PutStoredBlock_Chunk_Count,
    Longtail_BlockStoreAPI_StatU64_PutStoredBlock_Byte_Count,
    Longtail_BlockStoreAPI_StatU64_GetExistingContent_Count,
    Longtail_BlockStoreAPI_StatU64_GetExistingContent_RetryCount,
    Longtail_BlockStoreAPI_StatU64_GetExistingContent_FailCount,
    Longtail_BlockStoreAPI_StatU64_PruneBlocks_Count,
    Longtail_BlockStoreAPI_StatU64_PruneBlocks_RetryCount,
    Longtail_BlockStoreAPI_StatU64_PruneBlocks_FailCount,
    Longtail_BlockStoreAPI_StatU64_PreflightGet_Count,
    Longtail_BlockStoreAPI_StatU64_PreflightGet_RetryCount,
    Longtail_BlockStoreAPI_StatU64_PreflightGet_FailCount,
    Longtail_BlockStoreAPI_StatU64_Flush_Count,
    Longtail_BlockStoreAPI_StatU64_Flush_FailCount,
    Longtail_BlockStoreAPI_StatU64_GetStats_Count,
    Longtail_BlockStoreAPI_StatU64_Count,
}
internal unsafe struct Longtail_BlockStore_Stats
{
    public fixed ulong m_StatU64[(int)Longtail_BlockStoreAPI_StatU64_Enum.Longtail_BlockStoreAPI_StatU64_Count];
}
internal unsafe struct Longtail_BlockStoreAPI
{
    public Longtail_API m_API;
    /// <summary>
    /// int Longtail_BlockStore_PutStoredBlockFunc(struct Longtail_BlockStoreAPI* block_store_api, struct Longtail_StoredBlock* stored_block, struct Longtail_AsyncPutStoredBlockAPI* async_complete_api)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, Longtail_StoredBlock*, Longtail_AsyncPutStoredBlockAPI*, int> PutStoredBlock;
    /// <summary>
    /// int Longtail_BlockStore_PreflightGetFunc(struct Longtail_BlockStoreAPI* block_store_api, unsigned int block_count, const unsigned long long int* block_hashes, struct Longtail_AsyncPreflightStartedAPI* optional_async_complete_api)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, uint, ulong*, Longtail_AsyncPreflightStartedAPI*, int> PreflightGet;
    /// <summary>
    /// int Longtail_BlockStore_GetStoredBlockFunc(struct Longtail_BlockStoreAPI* block_store_api, unsigned long long int block_hash, struct Longtail_AsyncGetStoredBlockAPI* async_complete_api)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, ulong, Longtail_AsyncGetStoredBlockAPI*, int> GetStoredBlock;
    /// <summary>
    /// int Longtail_BlockStore_GetExistingContentFunc(struct Longtail_BlockStoreAPI* block_store_api, unsigned int chunk_count, const unsigned long long int* chunk_hashes, unsigned int min_block_usage_percent, struct Longtail_AsyncGetExistingContentAPI* async_complete_api)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, uint, ulong*, uint, Longtail_AsyncGetExistingContentAPI*, int> GetExistingContent;
    /// <summary>
    /// int Longtail_BlockStore_PruneBlocksFunc(struct Longtail_BlockStoreAPI* block_store_api, unsigned int block_keep_count, const unsigned long long int* block_keep_hashes, struct Longtail_AsyncPruneBlocksAPI* async_complete_api)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, uint, ulong*, Longtail_AsyncPruneBlocksAPI*, int> PruneBlocks;
    /// <summary>
    /// int Longtail_BlockStore_GetStatsFunc(struct Longtail_BlockStoreAPI* block_store_api, struct Longtail_BlockStore_Stats* out_stats)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, Longtail_BlockStore_Stats*, int> GetStats;
    /// <summary>
    /// int Longtail_BlockStore_FlushFunc(struct Longtail_BlockStoreAPI* block_store_api, struct Longtail_AsyncFlushAPI* async_complete_api)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, Longtail_AsyncFlushAPI*, int> Flush;
}
internal unsafe struct Longtail_Monitor
{
    public ulong StructSize;
    /// <summary>
    /// void Longtail_MonitorGetStoredBlockPrepare(const struct Longtail_StoreIndex* store_index, unsigned int block_index)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StoreIndex*, uint, void> BlockPrepare;
    /// <summary>
    /// void Longtail_MonitorGetStoredBlockLoad(const struct Longtail_StoreIndex* store_index, unsigned int block_index)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StoreIndex*, uint, void> BlockLoad;
    /// <summary>
    /// void Longtail_MonitorGetStoredBlockLoaded(const struct Longtail_StoreIndex* store_index, unsigned int block_index, int err)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StoreIndex*, uint, int, void> BlockLoaded;
    /// <summary>
    /// void Longtail_MonitorGetStoredBlockComplete(const struct Longtail_StoreIndex* store_index, unsigned int block_index, int err)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StoreIndex*, uint, int, void> BlockLoadComplete;
    /// <summary>
    /// void Longtail_MonitorAssetRemove(const struct Longtail_VersionIndex* source_version_index, unsigned int asset_index, int err)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_VersionIndex*, uint, int, void> AssetRemove;
    /// <summary>
    /// void Longtail_MonitorAssetOpen(const struct Longtail_VersionIndex* target_version_index, unsigned int asset_index, int err)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_VersionIndex*, uint, int, void> AssetOpen;
    /// <summary>
    /// void Longtail_MonitorAssetWrite(const struct Longtail_StoreIndex* target_store_index, const struct Longtail_VersionIndex* version_index, unsigned int asset_index, unsigned long long int write_offset, unsigned int size, unsigned int chunk_index, unsigned int chunk_index_in_block, unsigned int chunk_count_in_block, unsigned int block_index, unsigned int block_data_offset, int err)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StoreIndex*, Longtail_VersionIndex*, uint, ulong, uint, uint, uint, uint, uint, uint, int, void> AssetWrite;
    /// <summary>
    /// void Longtail_MonitorChunkRead(const struct Longtail_StoreIndex* store_index, const struct Longtail_VersionIndex* target_version_index, unsigned int block_index, unsigned int chunk_index, unsigned int chunk_index_in_block, int err)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StoreIndex*, Longtail_VersionIndex*, uint, uint, uint, int, void> ChunkRead;
    /// <summary>
    /// void Longtail_MonitorBlockCompose(const struct Longtail_StoreIndex* store_index, unsigned int block_index)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StoreIndex*, uint, void> BlockCompose;
    /// <summary>
    /// void Longtail_MonitorBlockSave(const struct Longtail_StoreIndex* store_index, unsigned int block_index, unsigned long long int block_size)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StoreIndex*, uint, ulong, void> BlockSave;
    /// <summary>
    /// void Longtail_MonitorBlockSaved(const struct Longtail_StoreIndex* store_index, unsigned int block_index, int err)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StoreIndex*, uint, int, void> BlockSaved;
    /// <summary>
    /// void Longtail_MonitorAssetClose(const struct Longtail_VersionIndex* version_index, unsigned int asset_index)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_VersionIndex*, uint, void> AssetClose;
    /// <summary>
    /// void Longtail_MonitorAssetRead(const struct Longtail_StoreIndex* store_index, const struct Longtail_VersionIndex* version_index, unsigned int asset_index, unsigned long long int read_offset, unsigned int size, unsigned long long int chunk_hash, unsigned int block_index, unsigned int block_data_offset, int err)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StoreIndex*, Longtail_VersionIndex*, uint, ulong, uint, ulong, uint, uint, int, void> AssetRead;
}
internal unsafe struct Longtail_LogField
{
    public byte* name;
    public byte* value;
}
internal unsafe struct Longtail_LogContext
{
    public void* context;
    public byte* file;
    public byte* function;
    public Longtail_LogField* fields;
    public int field_count;
    public int line;
    public int level;
}
internal unsafe struct Longtail_LogFieldFmt_Private
{
    public byte* name;
    public byte* fmt;
    public void* value;
}
internal unsafe struct Longtail_LogContextFmt_Private
{
    public Longtail_LogContextFmt_Private* parent_context;
    public Longtail_LogFieldFmt_Private* fields;
    public ulong field_count;
}
internal unsafe struct Longtail_BlockIndex
{
    public ulong* m_BlockHash;
    public uint* m_HashIdentifier;
    public uint* m_ChunkCount;
    public uint* m_Tag;
    public ulong* m_ChunkHashes;
    public uint* m_ChunkSizes;
}
internal unsafe struct Longtail_StoredBlock
{
    /// <summary>
    /// int Longtail_StoredBlock_DisposeFunc(struct Longtail_StoredBlock* stored_block)
    /// </summary>
    public delegate* unmanaged[Cdecl]<Longtail_StoredBlock*, int> Dispose;
    public Longtail_BlockIndex* m_BlockIndex;
    public void* m_BlockData;
    public uint m_BlockChunksDataSize;
}
internal unsafe struct Longtail_FileInfos
{
    public uint m_Count;
    public uint m_PathDataSize;
    public ulong* m_Sizes;
    public uint* m_PathStartOffsets;
    public ushort* m_Permissions;
    public byte* m_PathData;
}
internal unsafe struct Longtail_StoreIndex
{
    public uint* m_Version;
    public uint* m_HashIdentifier;
    public uint* m_BlockCount;
    public uint* m_ChunkCount;
    public ulong* m_BlockHashes;
    public ulong* m_ChunkHashes;
    public uint* m_BlockChunksOffsets;
    public uint* m_BlockChunkCounts;
    public uint* m_BlockTags;
    public uint* m_ChunkSizes;
}
internal unsafe struct Longtail_VersionIndex
{
    public uint* m_Version;
    public uint* m_HashIdentifier;
    public uint* m_TargetChunkSize;
    public uint* m_AssetCount;
    public uint* m_ChunkCount;
    public uint* m_AssetChunkIndexCount;
    public ulong* m_PathHashes;
    public ulong* m_ContentHashes;
    public ulong* m_AssetSizes;
    public uint* m_AssetChunkCounts;
    public uint* m_AssetChunkIndexStarts;
    public uint* m_AssetChunkIndexes;
    public ulong* m_ChunkHashes;
    public uint* m_ChunkSizes;
    public uint* m_ChunkTags;
    public uint* m_NameOffsets;
    public uint m_NameDataSize;
    public ushort* m_Permissions;
    public byte* m_NameData;
}
internal unsafe struct Longtail_ArchiveIndex
{
    public uint* m_Version;
    public uint* m_IndexDataSize;
    public Longtail_StoreIndex m_StoreIndex;
    public ulong* m_BlockStartOffets;
    public uint* m_BlockSizes;
    public Longtail_VersionIndex m_VersionIndex;
}
internal unsafe struct Longtail_VersionDiff
{
    public uint* m_SourceRemovedCount;
    public uint* m_TargetAddedCount;
    public uint* m_ModifiedContentCount;
    public uint* m_ModifiedPermissionsCount;
    public uint* m_SourceRemovedAssetIndexes;
    public uint* m_TargetAddedAssetIndexes;
    public uint* m_SourceContentModifiedAssetIndexes;
    public uint* m_TargetContentModifiedAssetIndexes;
    public uint* m_SourcePermissionsModifiedAssetIndexes;
    public uint* m_TargetPermissionsModifiedAssetIndexes;
}
internal unsafe struct Longtail_CancelAPI_CancelToken
{
}
internal unsafe struct Longtail_StorageAPI_OpenFile
{
}
internal unsafe struct Longtail_StorageAPI_Iterator
{
}
internal unsafe struct Longtail_StorageAPI_LockFile
{
}
internal unsafe struct Longtail_StorageAPI_FileMap
{
}
internal unsafe struct Longtail_ChunkerAPI_Chunker
{
}
internal unsafe struct Longtail_HashAPI_Context
{
}
internal unsafe struct Longtail_LookupTable
{
}
internal unsafe struct Longtail_Paths
{
}
