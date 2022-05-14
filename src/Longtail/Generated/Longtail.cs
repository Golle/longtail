internal unsafe struct Longtail_API
{
	public delegate* unmanaged[Cdecl]<Longtail_API*, void> Dispose;
}
internal unsafe struct Longtail_CancelAPI
{
	public Longtail_API m_API;
	public delegate* unmanaged[Cdecl]<Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken**, int> CreateToken;
	public delegate* unmanaged[Cdecl]<Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken*, int> Cancel;
	public delegate* unmanaged[Cdecl]<Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken*, int> IsCancelled;
	public delegate* unmanaged[Cdecl]<Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken*, int> DisposeToken;
}
internal unsafe struct Longtail_PathFilterAPI
{
	public Longtail_API m_API;
	public delegate* unmanaged[Cdecl]<Longtail_PathFilterAPI*, byte*, byte*, byte*, int, ulong, ushort, int> Include;
}
internal unsafe struct Longtail_HashAPI
{
	public Longtail_API m_API;
	public delegate* unmanaged[Cdecl]<Longtail_HashAPI*, uint> GetIdentifier;
	public delegate* unmanaged[Cdecl]<Longtail_HashAPI*, Longtail_HashAPI_Context**, int> BeginContext;
	public delegate* unmanaged[Cdecl]<Longtail_HashAPI*, Longtail_HashAPI_Context*, uint, void*, void> Hash;
	public delegate* unmanaged[Cdecl]<Longtail_HashAPI*, Longtail_HashAPI_Context*, ulong> EndContext;
	public delegate* unmanaged[Cdecl]<Longtail_HashAPI*, uint, void*, ulong*, int> HashBuffer;
}
internal unsafe struct Longtail_HashRegistryAPI
{
	public Longtail_API m_API;
	public delegate* unmanaged[Cdecl]<Longtail_HashRegistryAPI*, uint, Longtail_HashAPI**, int> GetHashAPI;
}
internal unsafe struct Longtail_CompressionAPI
{
	public Longtail_API m_API;
	public delegate* unmanaged[Cdecl]<Longtail_CompressionAPI*, uint, ulong, ulong> GetMaxCompressedSize;
	public delegate* unmanaged[Cdecl]<Longtail_CompressionAPI*, uint, byte*, byte*, ulong, ulong, ulong*, int> Compress;
	public delegate* unmanaged[Cdecl]<Longtail_CompressionAPI*, byte*, byte*, ulong, ulong, ulong*, int> Decompress;
}
internal unsafe struct Longtail_CompressionRegistryAPI
{
	public Longtail_API m_API;
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
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, Longtail_StorageAPI_OpenFile**, int> OpenReadFile;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_OpenFile*, ulong*, int> GetSize;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_OpenFile*, ulong, ulong, void*, int> Read;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, ulong, Longtail_StorageAPI_OpenFile**, int> OpenWriteFile;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_OpenFile*, ulong, ulong, void*, int> Write;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_OpenFile*, ulong, int> SetSize;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, ushort, int> SetPermissions;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, ushort*, int> GetPermissions;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_OpenFile*, void> CloseFile;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, int> CreateDir;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, byte*, int> RenameFile;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, byte*, byte*> ConcatPath;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, int> IsDir;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, int> IsFile;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, int> RemoveDir;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, int> RemoveFile;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, Longtail_StorageAPI_Iterator**, int> StartFind;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_Iterator*, int> FindNext;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_Iterator*, void> CloseFind;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_Iterator*, Longtail_StorageAPI_EntryProperties*, int> GetEntryProperties;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, Longtail_StorageAPI_LockFile**, int> LockFile;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_LockFile*, int> UnlockFile;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, byte*, byte*> GetParentPath;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_OpenFile*, ulong, ulong, Longtail_StorageAPI_FileMap**, void**, int> MapFile;
	public delegate* unmanaged[Cdecl]<Longtail_StorageAPI*, Longtail_StorageAPI_FileMap*, void> UnMapFile;
}
internal unsafe struct Longtail_ProgressAPI
{
	public Longtail_API m_API;
	public delegate* unmanaged[Cdecl]<Longtail_ProgressAPI*, uint, uint, void> OnProgress;
}
internal unsafe struct Longtail_JobAPI
{
	public Longtail_API m_API;
	public delegate* unmanaged[Cdecl]<Longtail_JobAPI*, uint> GetWorkerCount;
	public delegate* unmanaged[Cdecl]<Longtail_JobAPI*, uint, void**, int> ReserveJobs;
	public delegate* unmanaged[Cdecl]<Longtail_JobAPI*, void*, Longtail_ProgressAPI*, Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken*, uint, delegate* unmanaged[Cdecl]<void*, uint, int, int>*, void**, void**, int> CreateJobs;
	public delegate* unmanaged[Cdecl]<Longtail_JobAPI*, uint, void*, uint, void*, int> AddDependecies;
	public delegate* unmanaged[Cdecl]<Longtail_JobAPI*, uint, void*, int> ReadyJobs;
	public delegate* unmanaged[Cdecl]<Longtail_JobAPI*, void*, Longtail_ProgressAPI*, Longtail_CancelAPI*, Longtail_CancelAPI_CancelToken*, int> WaitForAllJobs;
	public delegate* unmanaged[Cdecl]<Longtail_JobAPI*, uint, int> ResumeJob;
	public delegate* unmanaged[Cdecl]<Longtail_JobAPI*, uint*, uint*, int> GetMaxBatchCountFunc;
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
	public delegate* unmanaged[Cdecl]<Longtail_ChunkerAPI*, uint*, int> GetMinChunkSize;
	public delegate* unmanaged[Cdecl]<Longtail_ChunkerAPI*, uint, uint, uint, Longtail_ChunkerAPI_Chunker**, int> CreateChunker;
	public delegate* unmanaged[Cdecl]<Longtail_ChunkerAPI*, Longtail_ChunkerAPI_Chunker*, delegate* unmanaged[Cdecl]<void*, Longtail_ChunkerAPI_Chunker*, uint, byte*, uint*, int>, void*, Longtail_Chunker_ChunkRange*, int> NextChunk;
	public delegate* unmanaged[Cdecl]<Longtail_ChunkerAPI*, Longtail_ChunkerAPI_Chunker*, int> DisposeChunker;
	public delegate* unmanaged[Cdecl]<Longtail_ChunkerAPI*, Longtail_ChunkerAPI_Chunker*, void*, ulong, void**, int> NextChunkFromBuffer;
}
internal unsafe struct Longtail_AsyncPutStoredBlockAPI
{
	public Longtail_API m_API;
	public delegate* unmanaged[Cdecl]<Longtail_AsyncPutStoredBlockAPI*, int, void> OnComplete;
}
internal unsafe struct Longtail_AsyncGetStoredBlockAPI
{
	public Longtail_API m_API;
	public delegate* unmanaged[Cdecl]<Longtail_AsyncGetStoredBlockAPI*, Longtail_StoredBlock*, int, void> OnComplete;
}
internal unsafe struct Longtail_AsyncGetExistingContentAPI
{
	public Longtail_API m_API;
	public delegate* unmanaged[Cdecl]<Longtail_AsyncGetExistingContentAPI*, Longtail_StoreIndex*, int, void> OnComplete;
}
internal unsafe struct Longtail_AsyncPruneBlocksAPI
{
	public Longtail_API m_API;
	public delegate* unmanaged[Cdecl]<Longtail_AsyncPruneBlocksAPI*, uint, int, void> OnComplete;
}
internal unsafe struct Longtail_AsyncPreflightStartedAPI
{
	public Longtail_API m_API;
	public delegate* unmanaged[Cdecl]<Longtail_AsyncPreflightStartedAPI*, uint, ulong*, int, void> OnComplete;
}
internal unsafe struct Longtail_AsyncFlushAPI
{
	public Longtail_API m_API;
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
	public ulong m_StatU64;
}
internal unsafe struct Longtail_BlockStoreAPI
{
	public Longtail_API m_API;
	public delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, Longtail_StoredBlock*, Longtail_AsyncPutStoredBlockAPI*, int> PutStoredBlock;
	public delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, uint, ulong*, Longtail_AsyncPreflightStartedAPI*, int> PreflightGet;
	public delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, ulong, Longtail_AsyncGetStoredBlockAPI*, int> GetStoredBlock;
	public delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, uint, ulong*, uint, Longtail_AsyncGetExistingContentAPI*, int> GetExistingContent;
	public delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, uint, ulong*, Longtail_AsyncPruneBlocksAPI*, int> PruneBlocks;
	public delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, Longtail_BlockStore_Stats*, int> GetStats;
	public delegate* unmanaged[Cdecl]<Longtail_BlockStoreAPI*, Longtail_AsyncFlushAPI*, int> Flush;
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
