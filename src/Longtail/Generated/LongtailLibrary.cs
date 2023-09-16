using System.Runtime.InteropServices;

namespace Longtail;

internal unsafe partial class LongtailLibrary
{
    private const string DllName = "longtail_csharp_bindings";

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_DisposeAPI(
        Longtail_API* api
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetCancelAPISize();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_CancelAPI* Longtail_MakeCancelAPI(
        void* mem,
        delegate* unmanaged[Cdecl]<Longtail_API*, void> dispose_func,
        delegate* unmanaged[Cdecl]<Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken**, int> create_token_func,
        delegate* unmanaged[Cdecl]<Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken*, int> cancel_func,
        delegate* unmanaged[Cdecl]<Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken*, int> is_cancelled,
        delegate* unmanaged[Cdecl]<Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken*, int> dispose_token_func
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_CancelAPI_CreateToken(
        Longtail_CancelAPI* cancel_api,
        Longtail_CancelAPI_CancelToken** out_token
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_CancelAPI_Cancel(
        Longtail_CancelAPI* cancel_api,
        Longtail_CancelAPI_CancelToken* token
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_CancelAPI_DisposeToken(
        Longtail_CancelAPI* cancel_api,
        Longtail_CancelAPI_CancelToken* token
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_CancelAPI_IsCancelled(
        Longtail_CancelAPI* cancel_api,
        Longtail_CancelAPI_CancelToken* token
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetPathFilterAPISize();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_PathFilterAPI* Longtail_MakePathFilterAPI(
        void* mem,
        delegate* unmanaged[Cdecl]<Longtail_API*, void> dispose_func,
        delegate* unmanaged[Cdecl]<Longtail_PathFilterAPI*, byte*, byte*, byte*, int, ulong, ushort, int> include_filter_func
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_PathFilter_Include(
        Longtail_PathFilterAPI* path_filter_api,
        byte* root_path,
        byte* asset_path,
        byte* asset_name,
        int is_dir,
        ulong size,
        ushort permissions
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetHashAPISize();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_HashAPI* Longtail_MakeHashAPI(
        void* mem,
        delegate* unmanaged[Cdecl]<Longtail_API*, void> dispose_func,
        delegate* unmanaged[Cdecl]<Longtail_HashAPI*, uint> get_identifier_func,
        delegate* unmanaged[Cdecl]<Longtail_HashAPI*, Longtail_HashAPI_Context**, int> begin_context_func,
        delegate* unmanaged[Cdecl]<Longtail_HashAPI*, Longtail_HashAPI_Context*, uint, void*, void> hash_func,
        delegate* unmanaged[Cdecl]<Longtail_HashAPI*, Longtail_HashAPI_Context*, ulong> end_context_func,
        delegate* unmanaged[Cdecl]<Longtail_HashAPI*, uint, void*, ulong*, int> hash_buffer_func
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_Hash_GetIdentifier(
        Longtail_HashAPI* hash_api
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Hash_BeginContext(
        Longtail_HashAPI* hash_api,
        Longtail_HashAPI_Context** out_context
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_Hash_Hash(
        Longtail_HashAPI* hash_api,
        Longtail_HashAPI_Context* context,
        uint length,
        void* data
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_Hash_EndContext(
        Longtail_HashAPI* hash_api,
        Longtail_HashAPI_Context* context
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Hash_HashBuffer(
        Longtail_HashAPI* hash_api,
        uint length,
        void* data,
        ulong* out_hash
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetHashRegistrySize();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_HashRegistryAPI* Longtail_MakeHashRegistryAPI(
        void* mem,
        delegate* unmanaged[Cdecl]<Longtail_API*, void> dispose_func,
        delegate* unmanaged[Cdecl]<Longtail_HashRegistryAPI*, uint, Longtail_HashAPI**, int> get_hash_api_func
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_GetHashRegistry_GetHashAPI(
        Longtail_HashRegistryAPI* hash_registry,
        uint hash_type,
        Longtail_HashAPI** out_compression_api
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetCompressionAPISize();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_CompressionAPI* Longtail_MakeCompressionAPI(
        void* mem,
        delegate* unmanaged[Cdecl]<Longtail_API*, void> dispose_func,
        delegate* unmanaged[Cdecl]<Longtail_CompressionAPI*, uint, ulong, ulong> get_max_compressed_size_func,
        delegate* unmanaged[Cdecl]<Longtail_CompressionAPI*, uint, byte*, byte*, ulong, ulong, ulong*, int> compress_func,
        delegate* unmanaged[Cdecl]<Longtail_CompressionAPI*, byte*, byte*, ulong, ulong, ulong*, int> decompress_func
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetCompressionRegistryAPISize();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_CompressionRegistryAPI* Longtail_MakeCompressionRegistryAPI(
        void* mem,
        delegate* unmanaged[Cdecl]<Longtail_API*, void> dispose_func,
        delegate* unmanaged[Cdecl]<Longtail_CompressionRegistryAPI*, uint, Longtail_CompressionAPI**, uint*, int> get_compression_api_func
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_GetCompressionRegistry_GetCompressionAPI(
        Longtail_CompressionRegistryAPI* compression_registry,
        uint compression_type,
        Longtail_CompressionAPI** out_compression_api,
        uint* out_settings_id
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetStorageAPISize();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_StorageAPI* Longtail_MakeStorageAPI(
        void* mem,
        delegate* unmanaged[Cdecl]<Longtail_API*, void> dispose_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, Longtail_StorageAPI_OpenFile**, int> open_read_file_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_OpenFile*, ulong*, int> get_size_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_OpenFile*, ulong, ulong, void*, int> read_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, ulong, Longtail_StorageAPI_OpenFile**, int> open_write_file_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_OpenFile*, ulong, ulong, void*, int> write_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_OpenFile*, ulong, int> set_size_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, ushort, int> set_permissions_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, ushort*, int> get_permissions_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_OpenFile*, void> close_file_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, int> create_dir_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, byte*, int> rename_file_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, byte*, byte*> concat_path_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, int> is_dir_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, int> is_file_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, int> remove_dir_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, int> remove_file_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, Longtail_StorageAPI_Iterator**, int> start_find_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_Iterator*, int> find_next_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_Iterator*, void> close_find_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_Iterator*, Longtail_StorageAPI_EntryProperties*, int> get_entry_properties_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, Longtail_StorageAPI_LockFile**, int> lock_file_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_LockFile*, int> unlock_file_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, byte*> get_parent_path_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_OpenFile*, ulong, ulong, Longtail_StorageAPI_FileMap**, void**, int> map_file_func,
        delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_FileMap*, void> unmap_file_func
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_OpenReadFile(
        Longtail_StorageAPI* storage_api,
        byte* path,
        Longtail_StorageAPI_OpenFile** out_open_file
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_GetSize(
        Longtail_StorageAPI* storage_api,
        Longtail_StorageAPI_OpenFile* f,
        ulong* out_size
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_Read(
        Longtail_StorageAPI* storage_api,
        Longtail_StorageAPI_OpenFile* f,
        ulong offset,
        ulong length,
        void* output
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_OpenWriteFile(
        Longtail_StorageAPI* storage_api,
        byte* path,
        ulong initial_size,
        Longtail_StorageAPI_OpenFile** out_open_file
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_Write(
        Longtail_StorageAPI* storage_api,
        Longtail_StorageAPI_OpenFile* f,
        ulong offset,
        ulong length,
        void* input
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_SetSize(
        Longtail_StorageAPI* storage_api,
        Longtail_StorageAPI_OpenFile* f,
        ulong length
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_SetPermissions(
        Longtail_StorageAPI* storage_api,
        byte* path,
        ushort permissions
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_GetPermissions(
        Longtail_StorageAPI* storage_api,
        byte* path,
        ushort* out_permissions
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_Storage_CloseFile(
        Longtail_StorageAPI* storage_api,
        Longtail_StorageAPI_OpenFile* f
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_CreateDir(
        Longtail_StorageAPI* storage_api,
        byte* path
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_RenameFile(
        Longtail_StorageAPI* storage_api,
        byte* source_path,
        byte* target_path
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* Longtail_Storage_ConcatPath(
        Longtail_StorageAPI* storage_api,
        byte* root_path,
        byte* sub_path
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_IsDir(
        Longtail_StorageAPI* storage_api,
        byte* path
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_IsFile(
        Longtail_StorageAPI* storage_api,
        byte* path
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_RemoveDir(
        Longtail_StorageAPI* storage_api,
        byte* path
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_RemoveFile(
        Longtail_StorageAPI* storage_api,
        byte* path
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_StartFind(
        Longtail_StorageAPI* storage_api,
        byte* path,
        Longtail_StorageAPI_Iterator** out_iterator
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_FindNext(
        Longtail_StorageAPI* storage_api,
        Longtail_StorageAPI_Iterator* iterator
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_Storage_CloseFind(
        Longtail_StorageAPI* storage_api,
        Longtail_StorageAPI_Iterator* iterator
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_GetEntryProperties(
        Longtail_StorageAPI* storage_api,
        Longtail_StorageAPI_Iterator* iterator,
        Longtail_StorageAPI_EntryProperties* out_properties
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_LockFile(
        Longtail_StorageAPI* storage_api,
        byte* path,
        Longtail_StorageAPI_LockFile** out_lock_file
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_UnlockFile(
        Longtail_StorageAPI* storage_api,
        Longtail_StorageAPI_LockFile* lock_file
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* Longtail_Storage_GetParentPath(
        Longtail_StorageAPI* storage_api,
        byte* path
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Storage_MapFile(
        Longtail_StorageAPI* storage_api,
        Longtail_StorageAPI_OpenFile* f,
        ulong offset,
        ulong length,
        Longtail_StorageAPI_FileMap** out_file_map,
        void** out_data_ptr
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_Storage_UnmapFile(
        Longtail_StorageAPI* storage_api,
        Longtail_StorageAPI_FileMap* m
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetProgressAPISize();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_ProgressAPI* Longtail_MakeProgressAPI(
        void* mem,
        delegate* unmanaged[Cdecl]<Longtail_API*, void> dispose_func,
        delegate* unmanaged[Cdecl]<Longtail_ProgressAPI*, uint, uint, void> on_progress_func
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_Progress_OnProgress(
        Longtail_ProgressAPI* progressAPI,
        uint total_count,
        uint done_count
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetJobAPISize();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_JobAPI* Longtail_MakeJobAPI(
        void* mem,
        delegate* unmanaged[Cdecl]<Longtail_API*, void> dispose_func,
        delegate* unmanaged[Cdecl]<Longtail_JobAPI*, uint> get_worker_count_func,
        delegate* unmanaged[Cdecl]<Longtail_JobAPI*, uint, void**, int> reserve_jobs_func,
        delegate* unmanaged[Cdecl]<Longtail_JobAPI*, void*, Longtail_ProgressAPI*, Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken*, uint, delegate* unmanaged[Cdecl]<void*, uint, int, int>*, void**, byte, void**, int> create_jobs_func,
        delegate* unmanaged[Cdecl]<Longtail_JobAPI*, uint, void*, uint, void*, int> add_dependecies_func,
        delegate* unmanaged[Cdecl]<Longtail_JobAPI*, uint, void*, int> ready_jobs_func,
        delegate* unmanaged[Cdecl]<Longtail_JobAPI*, void*, Longtail_ProgressAPI*, Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken*, int> wait_for_all_jobs_func,
        delegate* unmanaged[Cdecl]<Longtail_JobAPI*, uint, int> resume_job_func,
        delegate* unmanaged[Cdecl]<Longtail_JobAPI*, uint*, uint*, int> get_max_batch_count_func
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_Job_GetWorkerCount(
        Longtail_JobAPI* job_api
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Job_ReserveJobs(
        Longtail_JobAPI* job_api,
        uint job_count,
        void** out_job_group
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Job_CreateJobs(
        Longtail_JobAPI* job_api,
        void* job_group,
        Longtail_ProgressAPI* progressAPI,
        Longtail_CancelAPI* optional_cancel_api,
        Longtail_CancelAPI_CancelToken* optional_cancel_token,
        uint job_count,
        delegate* unmanaged[Cdecl]<void*, uint, int, int>* job_funcs,
        void** job_contexts,
        byte job_channel,
        void** out_jobs
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Job_AddDependecies(
        Longtail_JobAPI* job_api,
        uint job_count,
        void* jobs,
        uint dependency_job_count,
        void* dependency_jobs
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Job_ReadyJobs(
        Longtail_JobAPI* job_api,
        uint job_count,
        void* jobs
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Job_WaitForAllJobs(
        Longtail_JobAPI* job_api,
        void* job_group,
        Longtail_ProgressAPI* progressAPI,
        Longtail_CancelAPI* optional_cancel_api,
        Longtail_CancelAPI_CancelToken* optional_cancel_token
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Job_ResumeJob(
        Longtail_JobAPI* job_api,
        uint job_id
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Job_GetMaxBatchCount(
        Longtail_JobAPI* job_api,
        uint* out_max_job_batch_count,
        uint* out_max_dependency_batch_count
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetChunkerAPISize();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_ChunkerAPI* Longtail_MakeChunkerAPI(
        void* mem,
        delegate* unmanaged[Cdecl]<Longtail_API*, void> dispose_func,
        delegate* unmanaged[Cdecl]<Longtail_ChunkerAPI*, uint*, int> get_min_chunk_size_func,
        delegate* unmanaged[Cdecl]<Longtail_ChunkerAPI*, uint, uint, uint, Longtail_ChunkerAPI_Chunker**, int> create_chunker_func,
        delegate* unmanaged[Cdecl]<Longtail_ChunkerAPI*, Longtail_ChunkerAPI_Chunker*, delegate* unmanaged[Cdecl]<void*, Longtail_ChunkerAPI_Chunker*, uint, byte*, uint*, int>, void*, Longtail_Chunker_ChunkRange*, int> next_chunk_func,
        delegate* unmanaged[Cdecl]<Longtail_ChunkerAPI*, Longtail_ChunkerAPI_Chunker*, int> dispose_chunker_func,
        delegate* unmanaged[Cdecl]<Longtail_ChunkerAPI*, Longtail_ChunkerAPI_Chunker*, void*, ulong, void**, int> next_chunk_from_buffer
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Chunker_GetMinChunkSize(
        Longtail_ChunkerAPI* chunker_api,
        uint* out_min_chunk_size
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Chunker_CreateChunker(
        Longtail_ChunkerAPI* chunker_api,
        uint min_chunk_size,
        uint avg_chunk_size,
        uint max_chunk_size,
        Longtail_ChunkerAPI_Chunker** out_chunker
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Chunker_NextChunk(
        Longtail_ChunkerAPI* chunker_api,
        Longtail_ChunkerAPI_Chunker* chunker,
        delegate* unmanaged[Cdecl]<void*, Longtail_ChunkerAPI_Chunker*, uint, byte*, uint*, int> feeder,
        void* feeder_context,
        Longtail_Chunker_ChunkRange* out_chunk_range
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Chunker_DisposeChunker(
        Longtail_ChunkerAPI* chunker_api,
        Longtail_ChunkerAPI_Chunker* chunker
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_Chunker_NextChunkFromBuffer(
        Longtail_ChunkerAPI* chunker_api,
        Longtail_ChunkerAPI_Chunker* chunker,
        void* buffer,
        ulong buffer_size,
        void** out_next_chunk_start
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetAsyncPutStoredBlockAPISize();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_AsyncPutStoredBlockAPI* Longtail_MakeAsyncPutStoredBlockAPI(
        void* mem,
        delegate* unmanaged[Cdecl]<Longtail_API*, void> dispose_func,
        delegate* unmanaged[Cdecl]<Longtail_AsyncPutStoredBlockAPI*, int, void> on_complete_func
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_AsyncPutStoredBlock_OnComplete(
        Longtail_AsyncPutStoredBlockAPI* async_complete_api,
        int err
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetAsyncGetStoredBlockAPISize();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_AsyncGetStoredBlockAPI* Longtail_MakeAsyncGetStoredBlockAPI(
        void* mem,
        delegate* unmanaged[Cdecl]<Longtail_API*, void> dispose_func,
        delegate* unmanaged[Cdecl]<Longtail_AsyncGetStoredBlockAPI*, Longtail_StoredBlock*, int, void> on_complete_func
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_AsyncGetStoredBlock_OnComplete(
        Longtail_AsyncGetStoredBlockAPI* async_complete_api,
        Longtail_StoredBlock* stored_block,
        int err
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetAsyncGetExistingContentAPISize();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_AsyncGetExistingContentAPI* Longtail_MakeAsyncGetExistingContentAPI(
        void* mem,
        delegate* unmanaged[Cdecl]<Longtail_API*, void> dispose_func,
        delegate* unmanaged[Cdecl]<Longtail_AsyncGetExistingContentAPI*, Longtail_StoreIndex*, int, void> on_complete_func
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_AsyncGetExistingContent_OnComplete(
        Longtail_AsyncGetExistingContentAPI* async_complete_api,
        Longtail_StoreIndex* store_index,
        int err
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetAsyncPruneBlocksAPISize();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_AsyncPruneBlocksAPI* Longtail_MakeAsyncPruneBlocksAPI(
        void* mem,
        delegate* unmanaged[Cdecl]<Longtail_API*, void> dispose_func,
        delegate* unmanaged[Cdecl]<Longtail_AsyncPruneBlocksAPI*, uint, int, void> on_complete_func
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_AsyncPruneBlocks_OnComplete(
        Longtail_AsyncPruneBlocksAPI* async_complete_api,
        uint pruned_block_count,
        int err
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetAsyncPreflightStartedAPISize();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_AsyncPreflightStartedAPI* Longtail_MakeAsyncPreflightStartedAPI(
        void* mem,
        delegate* unmanaged[Cdecl]<Longtail_API*, void> dispose_func,
        delegate* unmanaged[Cdecl]<Longtail_AsyncPreflightStartedAPI*, uint, ulong*, int, void> on_complete_func
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_AsyncPreflightStarted_OnComplete(
        Longtail_AsyncPreflightStartedAPI* async_complete_api,
        uint block_count,
        ulong* block_hashes,
        int err
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetAsyncFlushAPISize();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_AsyncFlushAPI* Longtail_MakeAsyncFlushAPI(
        void* mem,
        delegate* unmanaged[Cdecl]<Longtail_API*, void> dispose_func,
        delegate* unmanaged[Cdecl]<Longtail_AsyncFlushAPI*, int, void> on_complete_func
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_AsyncFlush_OnComplete(
        Longtail_AsyncFlushAPI* async_complete_api,
        int err
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetBlockStoreAPISize();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_BlockStoreAPI* Longtail_MakeBlockStoreAPI(
        void* mem,
        delegate* unmanaged[Cdecl]<Longtail_API*, void> dispose_func,
        delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, Longtail_StoredBlock*, Longtail_AsyncPutStoredBlockAPI*, int> put_stored_block_func,
        delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, uint, ulong*, Longtail_AsyncPreflightStartedAPI*, int> preflight_get_func,
        delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, ulong, Longtail_AsyncGetStoredBlockAPI*, int> get_stored_block_func,
        delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, uint, ulong*, uint, Longtail_AsyncGetExistingContentAPI*, int> get_existing_content_func,
        delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, uint, ulong*, Longtail_AsyncPruneBlocksAPI*, int> prune_blocks_func,
        delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, Longtail_BlockStore_Stats*, int> get_stats_func,
        delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, Longtail_AsyncFlushAPI*, int> flush_func
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_BlockStore_PutStoredBlock(
        Longtail_BlockStoreAPI* block_store_api,
        Longtail_StoredBlock* stored_block,
        Longtail_AsyncPutStoredBlockAPI* async_complete_api
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_BlockStore_PreflightGet(
        Longtail_BlockStoreAPI* block_store_api,
        uint chunk_count,
        ulong* chunk_hashes,
        Longtail_AsyncPreflightStartedAPI* optional_async_complete_api
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_BlockStore_GetStoredBlock(
        Longtail_BlockStoreAPI* block_store_api,
        ulong block_hash,
        Longtail_AsyncGetStoredBlockAPI* async_complete_api
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_BlockStore_GetExistingContent(
        Longtail_BlockStoreAPI* block_store_api,
        uint chunk_count,
        ulong* chunk_hashes,
        uint min_block_usage_percent,
        Longtail_AsyncGetExistingContentAPI* async_complete_api
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_BlockStore_PruneBlocks(
        Longtail_BlockStoreAPI* block_store_api,
        uint block_keep_count,
        ulong* block_keep_hashes,
        Longtail_AsyncPruneBlocksAPI* async_complete_api
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_BlockStore_GetStats(
        Longtail_BlockStoreAPI* block_store_api,
        Longtail_BlockStore_Stats* out_stats
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_BlockStore_Flush(
        Longtail_BlockStoreAPI* block_store_api,
        Longtail_AsyncFlushAPI* async_complete_api
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_SetAssert(
        delegate* unmanaged[Cdecl]<byte*, byte*, int, void> assert_func
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_SetLog(
        delegate* unmanaged[Cdecl]<Longtail_LogContext*, byte*, void> log_func,
        void* context
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_SetLogLevel(
        int level
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_GetLogLevel();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_SetAllocAndFree(
        delegate* unmanaged[Cdecl]<byte*, ulong, void*> alloc,
        delegate* unmanaged[Cdecl]<void*, void> free
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void* Longtail_Alloc(
        byte* context,
        ulong s
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_Free(
        void* p
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int EnsureParentPathExists(
        Longtail_StorageAPI* storage_api,
        byte* path
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* Longtail_Strdup(
        byte* str
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_GetFilesRecursively(
        Longtail_StorageAPI* storage_api,
        Longtail_PathFilterAPI* path_filter_api,
        Longtail_CancelAPI* optional_cancel_api,
        Longtail_CancelAPI_CancelToken* optional_cancel_token,
        byte* root_path,
        Longtail_FileInfos** out_file_infos
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetVersionIndexSize(
        uint asset_count,
        uint chunk_count,
        uint asset_chunk_index_count,
        uint path_data_size
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_BuildVersionIndex(
        void* mem,
        ulong mem_size,
        Longtail_FileInfos* file_infos,
        ulong* path_hashes,
        ulong* content_hashes,
        uint* asset_chunk_index_starts,
        uint* asset_chunk_counts,
        uint asset_chunk_index_count,
        uint* asset_chunk_indexes,
        uint chunk_count,
        uint* chunk_sizes,
        ulong* chunk_hashes,
        uint* optional_chunk_tags,
        uint hash_api_identifier,
        uint target_chunk_size,
        Longtail_VersionIndex** out_version_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_CreateVersionIndex(
        Longtail_StorageAPI* storage_api,
        Longtail_HashAPI* hash_api,
        Longtail_ChunkerAPI* chunker_api,
        Longtail_JobAPI* job_api,
        Longtail_ProgressAPI* progress_api,
        Longtail_CancelAPI* optional_cancel_api,
        Longtail_CancelAPI_CancelToken* optional_cancel_token,
        byte* root_path,
        Longtail_FileInfos* file_infos,
        uint* optional_asset_tags,
        uint target_chunk_size,
        int enable_file_map,
        Longtail_VersionIndex** out_version_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_MergeVersionIndex(
        Longtail_VersionIndex* base_version_index,
        Longtail_VersionIndex* overlay_version_index,
        Longtail_VersionIndex** out_version_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_WriteVersionIndexToBuffer(
        Longtail_VersionIndex* version_index,
        void** out_buffer,
        ulong* out_size
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_ReadVersionIndexFromBuffer(
        void* buffer,
        ulong size,
        Longtail_VersionIndex** out_version_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_WriteVersionIndex(
        Longtail_StorageAPI* storage_api,
        Longtail_VersionIndex* version_index,
        byte* path
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_ReadVersionIndex(
        Longtail_StorageAPI* storage_api,
        byte* path,
        Longtail_VersionIndex** out_version_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_GetRequiredChunkHashes(
        Longtail_VersionIndex* version_index,
        Longtail_VersionDiff* version_diff,
        uint* out_chunk_count,
        ulong* out_chunk_hashes
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_WriteContent(
        Longtail_StorageAPI* source_storage_api,
        Longtail_BlockStoreAPI* block_store_api,
        Longtail_JobAPI* job_api,
        Longtail_ProgressAPI* progress_api,
        Longtail_CancelAPI* optional_cancel_api,
        Longtail_CancelAPI_CancelToken* optional_cancel_token,
        Longtail_StoreIndex* store_index,
        Longtail_VersionIndex* version_index,
        byte* assets_folder
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_CreateMissingContent(
        Longtail_HashAPI* hash_api,
        Longtail_StoreIndex* store_index,
        Longtail_VersionIndex* version_index,
        uint max_block_size,
        uint max_chunks_per_block,
        Longtail_StoreIndex** out_store_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_GetMissingChunks(
        Longtail_StoreIndex* store_index,
        uint chunk_count,
        ulong* chunk_hashes,
        uint* out_chunk_count,
        ulong* out_missing_chunk_hashes
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_WriteVersion(
        Longtail_BlockStoreAPI* block_storage_api,
        Longtail_StorageAPI* version_storage_api,
        Longtail_JobAPI* job_api,
        Longtail_ProgressAPI* progress_api,
        Longtail_CancelAPI* optional_cancel_api,
        Longtail_CancelAPI_CancelToken* optional_cancel_token,
        Longtail_StoreIndex* store_index,
        Longtail_VersionIndex* version_index,
        byte* version_path,
        int retain_permissions
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_CreateVersionDiff(
        Longtail_HashAPI* hash_api,
        Longtail_VersionIndex* source_version,
        Longtail_VersionIndex* target_version,
        Longtail_VersionDiff** out_version_diff
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_ChangeVersion(
        Longtail_BlockStoreAPI* block_store_api,
        Longtail_StorageAPI* version_storage_api,
        Longtail_HashAPI* hash_api,
        Longtail_JobAPI* job_api,
        Longtail_ProgressAPI* progress_api,
        Longtail_CancelAPI* optional_cancel_api,
        Longtail_CancelAPI_CancelToken* optional_cancel_token,
        Longtail_StoreIndex* store_index,
        Longtail_VersionIndex* source_version,
        Longtail_VersionIndex* target_version,
        Longtail_VersionDiff* version_diff,
        byte* version_path,
        int retain_permissions
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetBlockIndexDataSize(
        uint chunk_count
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetBlockIndexSize(
        uint chunk_count
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_BlockIndex* Longtail_InitBlockIndex(
        void* mem,
        uint chunk_count
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_BlockIndex* Longtail_CopyBlockIndex(
        Longtail_BlockIndex* block_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_InitBlockIndexFromData(
        Longtail_BlockIndex* block_index,
        void* data,
        ulong data_size
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_CreateBlockIndex(
        Longtail_HashAPI* hash_api,
        uint tag,
        uint chunk_count,
        uint* chunk_indexes,
        ulong* chunk_hashes,
        uint* chunk_sizes,
        Longtail_BlockIndex** out_block_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_WriteBlockIndexToBuffer(
        Longtail_BlockIndex* block_index,
        void** out_buffer,
        ulong* out_size
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_ReadBlockIndexFromBuffer(
        void* buffer,
        ulong size,
        Longtail_BlockIndex** out_block_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_WriteBlockIndex(
        Longtail_StorageAPI* storage_api,
        Longtail_BlockIndex* block_index,
        byte* path
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_ReadBlockIndex(
        Longtail_StorageAPI* storage_api,
        byte* path,
        Longtail_BlockIndex** out_block_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetStoredBlockSize(
        ulong block_data_size
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_InitStoredBlockFromData(
        Longtail_StoredBlock* stored_block,
        void* block_data,
        ulong block_data_size
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_CreateStoredBlock(
        ulong block_hash,
        uint hash_identifier,
        uint chunk_count,
        uint tag,
        ulong* chunk_hashes,
        uint* chunk_sizes,
        uint block_data_size,
        Longtail_StoredBlock** out_stored_block
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_WriteStoredBlockToBuffer(
        Longtail_StoredBlock* stored_block,
        void** out_buffer,
        ulong* out_size
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_ReadStoredBlockFromBuffer(
        void* buffer,
        ulong size,
        Longtail_StoredBlock** out_stored_block
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_WriteStoredBlock(
        Longtail_StorageAPI* storage_api,
        Longtail_StoredBlock* stored_block,
        byte* path
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_ReadStoredBlock(
        Longtail_StorageAPI* storage_api,
        byte* path,
        Longtail_StoredBlock** out_stored_block
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_BlockIndex_GetChunkCount(
        Longtail_BlockIndex* block_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint* Longtail_BlockIndex_GetChunkTag(
        Longtail_BlockIndex* block_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong* Longtail_BlockIndex_GetChunkHashes(
        Longtail_BlockIndex* block_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint* Longtail_BlockIndex_GetChunkSizes(
        Longtail_BlockIndex* block_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_StoredBlock_Dispose(
        Longtail_StoredBlock* stored_block
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_BlockIndex* Longtail_StoredBlock_GetBlockIndex(
        Longtail_StoredBlock* stored_block
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void* Longtail_BlockIndex_BlockData(
        Longtail_StoredBlock* stored_block
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_BlockIndex_GetBlockChunksDataSize(
        Longtail_StoredBlock* stored_block
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_FileInfos_GetCount(
        Longtail_FileInfos* file_infos
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* Longtail_FileInfos_GetPath(
        Longtail_FileInfos* file_infos,
        uint index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_FileInfos_GetSize(
        Longtail_FileInfos* file_infos,
        uint index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ushort* Longtail_FileInfos_GetPermissions(
        Longtail_FileInfos* file_infos,
        uint index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_StoreIndex_GetVersion(
        Longtail_StoreIndex* store_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_StoreIndex_GetHashIdentifier(
        Longtail_StoreIndex* store_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_StoreIndex_GetBlockCount(
        Longtail_StoreIndex* store_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_StoreIndex_GetChunkCount(
        Longtail_StoreIndex* store_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong* Longtail_StoreIndex_GetBlockHashes(
        Longtail_StoreIndex* store_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong* Longtail_StoreIndex_GetChunkHashes(
        Longtail_StoreIndex* store_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint* Longtail_StoreIndex_GetBlockChunksOffsets(
        Longtail_StoreIndex* store_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint* Longtail_StoreIndex_GetBlockChunkCounts(
        Longtail_StoreIndex* store_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint* Longtail_StoreIndex_GetBlockTags(
        Longtail_StoreIndex* store_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint* Longtail_StoreIndex_GetChunkSizes(
        Longtail_StoreIndex* store_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Longtail_GetStoreIndexSize(
        uint block_count,
        uint chunk_count
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_CreateStoreIndex(
        Longtail_HashAPI* hash_api,
        uint chunk_count,
        ulong* chunk_hashes,
        uint* chunk_sizes,
        uint* optional_chunk_tags,
        uint max_block_size,
        uint max_chunks_per_block,
        Longtail_StoreIndex** out_store_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_CreateStoreIndexFromBlocks(
        uint block_count,
        Longtail_BlockIndex** block_indexes,
        Longtail_StoreIndex** out_store_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_MergeStoreIndex(
        Longtail_StoreIndex* local_store_index,
        Longtail_StoreIndex* remote_store_index,
        Longtail_StoreIndex** out_store_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_MakeBlockIndex(
        Longtail_StoreIndex* store_index,
        uint block_index,
        Longtail_BlockIndex* out_block_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_GetExistingStoreIndex(
        Longtail_StoreIndex* store_index,
        uint chunk_count,
        ulong* chunks,
        uint min_block_usage_percent,
        Longtail_StoreIndex** out_store_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_PruneStoreIndex(
        Longtail_StoreIndex* source_store_index,
        uint keep_block_count,
        ulong* keep_block_hashes,
        Longtail_StoreIndex** out_store_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_ValidateStore(
        Longtail_StoreIndex* store_index,
        Longtail_VersionIndex* version_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_StoreIndex* Longtail_CopyStoreIndex(
        Longtail_StoreIndex* store_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_SplitStoreIndex(
        Longtail_StoreIndex* store_index,
        ulong split_size,
        Longtail_StoreIndex*** out_store_indexes,
        ulong* out_count
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_WriteStoreIndexToBuffer(
        Longtail_StoreIndex* store_index,
        void** out_buffer,
        ulong* out_size
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_ReadStoreIndexFromBuffer(
        void* buffer,
        ulong size,
        Longtail_StoreIndex** out_store_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_WriteStoreIndex(
        Longtail_StorageAPI* storage_api,
        Longtail_StoreIndex* store_index,
        byte* path
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_ReadStoreIndex(
        Longtail_StorageAPI* storage_api,
        byte* path,
        Longtail_StoreIndex** out_store_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_CreateArchiveIndex(
        Longtail_StoreIndex* store_index,
        Longtail_VersionIndex* version_index,
        Longtail_ArchiveIndex** out_archive_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_ReadArchiveIndex(
        Longtail_StorageAPI* storage_api,
        byte* path,
        Longtail_ArchiveIndex** out_archive_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_VersionIndex_GetVersion(
        Longtail_VersionIndex* version_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_VersionIndex_GetHashAPI(
        Longtail_VersionIndex* version_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_VersionIndex_GetAssetCount(
        Longtail_VersionIndex* version_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_VersionIndex_GetChunkCount(
        Longtail_VersionIndex* version_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong* Longtail_VersionIndex_GetChunkHashes(
        Longtail_VersionIndex* version_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint* Longtail_VersionIndex_GetChunkSizes(
        Longtail_VersionIndex* version_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint* Longtail_VersionIndex_GetChunkTags(
        Longtail_VersionIndex* version_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int LongtailPrivate_GetPathHash(
        Longtail_HashAPI* hash_api,
        byte* path,
        ulong* out_hash
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong LongtailPrivate_LookupTable_GetSize(
        uint capacity
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_LookupTable* LongtailPrivate_LookupTable_Create(
        void* mem,
        uint capacity,
        Longtail_LookupTable* optional_source_entries
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int LongtailPrivate_LookupTable_Put(
        Longtail_LookupTable* lut,
        ulong key,
        uint value
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint* LongtailPrivate_LookupTable_PutUnique(
        Longtail_LookupTable* lut,
        ulong key,
        uint value
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint* LongtailPrivate_LookupTable_Get(
        Longtail_LookupTable* lut,
        ulong key
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint LongtailPrivate_LookupTable_GetSpaceLeft(
        Longtail_LookupTable* lut
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int LongtailPrivate_MakeFileInfos(
        uint path_count,
        byte** path_names,
        ulong* file_sizes,
        ushort* file_permissions,
        Longtail_FileInfos** out_file_infos
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_BlockStoreAPI* Longtail_CreateArchiveBlockStore(
        Longtail_StorageAPI* storage_api,
        byte* archive_path,
        Longtail_ArchiveIndex* archive_index,
        int enable_write,
        int enable_mmap_reading
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_CancelAPI* Longtail_CreateAtomicCancelAPI();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_JobAPI* Longtail_CreateBikeshedJobAPI(
        uint worker_count,
        int worker_priority
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_HashAPI* Longtail_CreateBlake2HashAPI();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_GetBlake2HashType();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_HashAPI* Longtail_CreateBlake3HashAPI();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_GetBlake3HashType();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_StorageAPI* Longtail_CreateBlockStoreStorageAPI(
        Longtail_HashAPI* hash_api,
        Longtail_JobAPI* job_api,
        Longtail_BlockStoreAPI* block_store,
        Longtail_StoreIndex* store_index,
        Longtail_VersionIndex* version_index
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_CompressionAPI* Longtail_CreateBrotliCompressionAPI();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_GetBrotliGenericMinQuality();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_GetBrotliGenericDefaultQuality();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_GetBrotliGenericMaxQuality();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_GetBrotliTextMinQuality();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_GetBrotliTextDefaultQuality();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_GetBrotliTextMaxQuality();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_CompressionAPI* Longtail_CompressionRegistry_CreateForBrotli(
        uint compression_type,
        uint* out_settings
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_BlockStoreAPI* Longtail_CreateCacheBlockStoreAPI(
        Longtail_JobAPI* job_api,
        Longtail_BlockStoreAPI* local_block_store,
        Longtail_BlockStoreAPI* remote_block_store
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_BlockStoreAPI* Longtail_CreateCompressBlockStoreAPI(
        Longtail_BlockStoreAPI* backing_block_store,
        Longtail_CompressionRegistryAPI* compression_registry
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_CompressionRegistryAPI* Longtail_CreateDefaultCompressionRegistry(
        uint compression_api_count,
        delegate* unmanaged[Cdecl]<uint, uint*, Longtail_CompressionAPI*>* create_api_funcs
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_CompressionRegistryAPI* Longtail_CreateFullCompressionRegistry();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_CompressionRegistryAPI* Longtail_CreateZStdCompressionRegistry();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_StorageAPI* Longtail_CreateFSStorageAPI();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_BlockStoreAPI* Longtail_CreateFSBlockStoreAPI(
        Longtail_JobAPI* job_api,
        Longtail_StorageAPI* storage_api,
        byte* content_path,
        byte* optional_extension,
        int enable_file_mapping
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_HashRegistryAPI* Longtail_CreateBlake3HashRegistry();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_HashRegistryAPI* Longtail_CreateFullHashRegistry();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_HashRegistryAPI* Longtail_CreateDefaultHashRegistry(
        uint hash_type_count,
        uint* hash_types,
        Longtail_HashAPI** hash_apis
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_ChunkerAPI* Longtail_CreateHPCDCChunkerAPI();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_BlockStoreAPI* Longtail_CreateLRUBlockStoreAPI(
        Longtail_BlockStoreAPI* backing_block_store,
        uint max_lru_count
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_CompressionAPI* Longtail_CreateLZ4CompressionAPI();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_GetLZ4DefaultQuality();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_CompressionAPI* Longtail_CompressionRegistry_CreateForLZ4(
        uint compression_type,
        uint* out_settings
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_StorageAPI* Longtail_CreateInMemStorageAPI();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_GetMemTracerSummary();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_GetMemTracerDetailed();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_MemTracer_Init();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* Longtail_MemTracer_GetStats(
        uint log_level
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_MemTracer_Dispose();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void* Longtail_MemTracer_Alloc(
        byte* context,
        ulong s
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Longtail_MemTracer_Free(
        void* p
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Longtail_MemTracer_DumpStats(
        byte* name
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_HashAPI* Longtail_CreateMeowHashAPI();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_GetMeowHashType();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_ProgressAPI* Longtail_CreateRateLimitedProgress(
        Longtail_ProgressAPI* progress_api,
        uint percent_rate_limit
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_BlockStoreAPI* Longtail_CreateShareBlockStoreAPI(
        Longtail_BlockStoreAPI* backing_block_store
    );

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_CompressionAPI* Longtail_CreateZStdCompressionAPI();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_GetZStdMinQuality();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_GetZStdDefaultQuality();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_GetZStdMaxQuality();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_GetZStdHighQuality();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint Longtail_GetZStdLowQuality();
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Longtail_CompressionAPI* Longtail_CompressionRegistry_CreateForZstd(
        uint compression_type,
        uint* out_settings
    );

}
