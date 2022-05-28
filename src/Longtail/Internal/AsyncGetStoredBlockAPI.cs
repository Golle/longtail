using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Longtail.Internal;

internal unsafe class AsyncGetStoredBlockAPI : IDisposable
{
    private AsyncGetStoredBlockAPIInternal* _api;
    private readonly EventWaitHandle _waitHandle;
    public ErrorCodesEnum Err { get; private set; }
    public Longtail_StoredBlock* StoredBlock { get; private set; }
    public void Wait() => _waitHandle.WaitOne();
    public static implicit operator Longtail_AsyncGetStoredBlockAPI*(AsyncGetStoredBlockAPI api) => (Longtail_AsyncGetStoredBlockAPI*)api._api;

    public AsyncGetStoredBlockAPI()
    {
        using var name = new Utf8String(nameof(AsyncGetStoredBlockAPI));
        var mem = LongtailLibrary.Longtail_Alloc(name, (ulong)sizeof(AsyncGetStoredBlockAPIInternal));
        if (mem == null)
        {
            throw new OutOfMemoryException(nameof(LongtailLibrary.Longtail_Alloc));
        }
        _api = (AsyncGetStoredBlockAPIInternal*)LongtailLibrary.Longtail_MakeAsyncGetStoredBlockAPI(mem, &DisposeFunc, &OnCompletedFunc);
        _waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        _api->Handle = GCHandle.Alloc(this);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void OnCompletedFunc(Longtail_AsyncGetStoredBlockAPI* asyncCompleteApi, Longtail_StoredBlock* storedBlock, int err)
    {
        var api = (AsyncGetStoredBlockAPIInternal*)asyncCompleteApi;
        var apiInternal = (AsyncGetStoredBlockAPI)api->Handle.Target!;
        apiInternal.Err = (ErrorCodesEnum)err;
        apiInternal.StoredBlock = storedBlock;
        apiInternal._waitHandle.Set();
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void DisposeFunc(Longtail_API* api)
    {
        var apiInternal = (AsyncGetStoredBlockAPIInternal*)api;
        if (apiInternal->Handle.IsAllocated)
        {
            apiInternal->Handle.Free();
        }
        LongtailLibrary.Longtail_Free(api);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct AsyncGetStoredBlockAPIInternal
    {
        public Longtail_AsyncGetStoredBlockAPI Api;
        public GCHandle Handle;
    }

    public void Dispose()
    {
        if (_api != null)
        {
            LongtailLibrary.Longtail_DisposeAPI(&_api->Api.m_API);
            _api = null;
            _waitHandle.Dispose();
        }
    }
}