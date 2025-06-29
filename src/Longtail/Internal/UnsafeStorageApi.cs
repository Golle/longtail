namespace Longtail.Internal;

internal unsafe class UnsafeStorageApi : IUnsafeStorageApi
{
    private Longtail_StorageAPI* _storageApi;
    public int GetSize(Longtail_StorageAPI_OpenFile* f, ulong* out_size) => _storageApi->GetSize(_storageApi, f, out_size);
    public int Read(Longtail_StorageAPI_OpenFile* f, ulong offset, ulong length, void* output) => _storageApi->Read(_storageApi, f, offset, length, output);
    public int OpenWriteFile(byte* path, ulong initial_size, Longtail_StorageAPI_OpenFile** out_open_file) => _storageApi->OpenWriteFile(_storageApi, path, initial_size, out_open_file);
    public int Write(Longtail_StorageAPI_OpenFile* f, ulong offset, ulong length, void* input) => _storageApi->Write(_storageApi, f, offset, length, input);
    public int SetSize(Longtail_StorageAPI_OpenFile* f, ulong length) => _storageApi->SetSize(_storageApi, f, length);
    public int SetPermissions(byte* path, ushort permissions) => _storageApi->SetPermissions(_storageApi, path, permissions);
    public int GetPermissions(byte* path, ushort* out_permissions) => _storageApi->GetPermissions(_storageApi, path, out_permissions);
    public void CloseFile(Longtail_StorageAPI_OpenFile* f) => _storageApi->CloseFile(_storageApi, f);
    public int CreateDir(byte* path) => _storageApi->CreateDir(_storageApi, path);
    public int RenameFile(byte* source_path, byte* target_path) => _storageApi->RenameFile(_storageApi, source_path, target_path);
    public byte* ConcatPath(byte* root_path, byte* sub_path) => _storageApi->ConcatPath(_storageApi, root_path, sub_path);
    public int IsDir(byte* path) => _storageApi->IsDir(_storageApi, path);
    public int IsFile(byte* path) => _storageApi->IsFile(_storageApi, path);
    public int RemoveDir(byte* path) => _storageApi->RemoveDir(_storageApi, path);
    public int RemoveFile(byte* path) => _storageApi->RemoveFile(_storageApi, path);
    public int StartFind(byte* path, Longtail_StorageAPI_Iterator** out_iterator) => _storageApi->StartFind(_storageApi, path, out_iterator);
    public int FindNext(Longtail_StorageAPI_Iterator* iterator) => _storageApi->FindNext(_storageApi, iterator);
    public void CloseFind(Longtail_StorageAPI_Iterator* iterator) => _storageApi->CloseFind(_storageApi, iterator);
    public int GetEntryProperties(Longtail_StorageAPI_Iterator* iterator, Longtail_StorageAPI_EntryProperties* out_properties) => _storageApi->GetEntryProperties(_storageApi, iterator, out_properties);
    public int LockFile(byte* path, Longtail_StorageAPI_LockFile** out_lock_file) => _storageApi->LockFile(_storageApi, path, out_lock_file);
    public int UnlockFile(Longtail_StorageAPI_LockFile* file_lock) => _storageApi->UnlockFile(_storageApi, file_lock);
    public byte* GetParentPath(byte* path) => _storageApi->GetParentPath(_storageApi, path);
    public int MapFile(Longtail_StorageAPI_OpenFile* f, ulong offset, ulong length, Longtail_StorageAPI_FileMap** out_file_map, void** out_data_ptr) => _storageApi->MapFile(_storageApi, f, offset, length, out_file_map, out_data_ptr);
    public void UnmapFile(Longtail_StorageAPI_FileMap* m) => _storageApi->UnMapFile(_storageApi, m);
    public int OpenAppendFile(byte* path, Longtail_StorageAPI_OpenFile** out_open_file) => _storageApi->OpenAppendFile(_storageApi, path, out_open_file);

    public void Dispose()
    {
        if (_storageApi != null)
        {
            LongtailLibrary.Longtail_DisposeAPI(&_storageApi->m_API);
            _storageApi = null;
        }
    }
}