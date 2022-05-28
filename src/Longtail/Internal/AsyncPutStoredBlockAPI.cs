using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Longtail.Internal;
internal unsafe class AsyncPutStoredBlockAPI : IDisposable
{
    private AsyncPutStoredBlockAPIInternal* _storedBlock;
    public ErrorCodesEnum Err { get; private set; }

    private readonly EventWaitHandle _waitHandle;

    public static implicit operator Longtail_AsyncPutStoredBlockAPI*(AsyncPutStoredBlockAPI api) => (Longtail_AsyncPutStoredBlockAPI*)api._storedBlock;
    public void Wait() => _waitHandle.WaitOne();
    public AsyncPutStoredBlockAPI()
    {
        using var name = new Utf8String(nameof(AsyncPutStoredBlockAPI));
        var mem = LongtailLibrary.Longtail_Alloc(name, (ulong)sizeof(AsyncPutStoredBlockAPIInternal));
        if (mem == null)
        {
            throw new OutOfMemoryException(nameof(LongtailLibrary.Longtail_Alloc));
        }
        _storedBlock = (AsyncPutStoredBlockAPIInternal*)LongtailLibrary.Longtail_MakeAsyncPutStoredBlockAPI(mem, &DisposeFunc, &OnCompletedFunc);
        _waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        _storedBlock->Handle = GCHandle.Alloc(this);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void DisposeFunc(Longtail_API* api)
    {
        var storedBlockApi = (AsyncPutStoredBlockAPIInternal*)api;
        if (storedBlockApi->Handle.IsAllocated)
        {
            storedBlockApi->Handle.Free();
        }
        LongtailLibrary.Longtail_Free(api);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void OnCompletedFunc(Longtail_AsyncPutStoredBlockAPI* api, int err)
    {
        var apiInternal = (AsyncPutStoredBlockAPIInternal*)api;
        var storedBlockApi = (AsyncPutStoredBlockAPI)apiInternal->Handle.Target!;
        storedBlockApi.Err = (ErrorCodesEnum)err;
        storedBlockApi._waitHandle.Set();
    }

    public void Dispose()
    {
        if (_storedBlock != null)
        {
            _waitHandle.Dispose();
            LongtailLibrary.Longtail_DisposeAPI(&_storedBlock->PutStoredBlockApi.m_API);
            _storedBlock = null;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct AsyncPutStoredBlockAPIInternal
    {
        public Longtail_AsyncPutStoredBlockAPI PutStoredBlockApi;
        public GCHandle Handle;
    }
}