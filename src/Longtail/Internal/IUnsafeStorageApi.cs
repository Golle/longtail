namespace Longtail.Internal;

// This is probably not needed, since anyone could just implement the function pointers
internal unsafe interface IUnsafeStorageApi : IDisposable
{
    int GetSize(Longtail_StorageAPI_OpenFile* f, ulong* out_size);
    int Read(Longtail_StorageAPI_OpenFile* f, ulong offset, ulong length, void* output);
    int OpenWriteFile(byte* path, ulong initial_size, Longtail_StorageAPI_OpenFile** out_open_file);
    int Write(Longtail_StorageAPI_OpenFile* f, ulong offset, ulong length, void* input);
    int SetSize(Longtail_StorageAPI_OpenFile* f, ulong length);
    int SetPermissions(byte* path, ushort permissions);
    int GetPermissions(byte* path, ushort* out_permissions);
    void CloseFile(Longtail_StorageAPI_OpenFile* f);
    int CreateDir(byte* path);
    int RenameFile(byte* source_path, byte* target_path);
    byte* ConcatPath(byte* root_path, byte* sub_path);
    int IsDir(byte* path);
    int IsFile(byte* path);
    int RemoveDir(byte* path);
    int RemoveFile(byte* path);
    int StartFind(byte* path, Longtail_StorageAPI_Iterator** out_iterator);
    int FindNext(Longtail_StorageAPI_Iterator* iterator);
    void CloseFind(Longtail_StorageAPI_Iterator* iterator);
    int GetEntryProperties(Longtail_StorageAPI_Iterator* iterator, Longtail_StorageAPI_EntryProperties* out_properties);
    int LockFile(byte* path, Longtail_StorageAPI_LockFile** out_lock_file);
    int UnlockFile(Longtail_StorageAPI_LockFile* file_lock);
    byte* GetParentPath(byte* path);
    int MapFile(Longtail_StorageAPI_OpenFile* f, ulong offset, ulong length, Longtail_StorageAPI_FileMap** out_file_map, void** out_data_ptr);
    void UnmapFile(Longtail_StorageAPI_FileMap* m);
}