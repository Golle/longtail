using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Longtail.Internal;

internal unsafe class AsyncFlushApi : IDisposable
{
    private AsyncFlushAPIInternal* _flushApi;
    public ErrorCodesEnum Err { get; private set; }
    
    private readonly EventWaitHandle _waitHandle;

    public static implicit operator Longtail_AsyncFlushAPI*(AsyncFlushApi api) => (Longtail_AsyncFlushAPI*)api._flushApi;
    public AsyncFlushApi()
    {
        using var name = new Utf8String(nameof(AsyncFlushApi));
        var mem = LongtailLibrary.Longtail_Alloc(name, (ulong)sizeof(AsyncFlushAPIInternal));
        if (mem == null)
        {
            throw new OutOfMemoryException(nameof(LongtailLibrary.Longtail_Alloc));
        }
        _flushApi = (AsyncFlushAPIInternal*)LongtailLibrary.Longtail_MakeAsyncFlushAPI(mem, &DisposeFunc, &OnCompletedFunc);
        _waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        _flushApi->Handle = GCHandle.Alloc(this);
    }

    public void Wait() => _waitHandle.WaitOne();

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void DisposeFunc(Longtail_API* api)
    {
        var flushApiInternal = (AsyncFlushAPIInternal*)api;
        if (flushApiInternal->Handle.IsAllocated)
        {
            flushApiInternal->Handle.Free();
        }
        LongtailLibrary.Longtail_Free(api);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void OnCompletedFunc(Longtail_AsyncFlushAPI* api, int err)
    {
        var flushApiInternal = (AsyncFlushAPIInternal*)api;
        var flushApi = (AsyncFlushApi)flushApiInternal->Handle.Target!;
        flushApi.Err = (ErrorCodesEnum)err;
        flushApi._waitHandle.Set();
    }

    public void Dispose()
    {
        if (_flushApi != null)
        {
            LongtailLibrary.Longtail_DisposeAPI(&_flushApi->FlushApi.m_API);
            _flushApi = null;
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    private struct AsyncFlushAPIInternal
    {
        public Longtail_AsyncFlushAPI FlushApi;
        public GCHandle Handle;
    }
}