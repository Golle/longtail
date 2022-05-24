using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Longtail.Internal;
internal unsafe class AsyncPreflightStartedAPI : IDisposable
{
    private AsyncPreflightStartedAPIInternal* _preflight;
    public ErrorCodesEnum Err { get; private set; }

    private readonly EventWaitHandle _waitHandle;

    public static implicit operator Longtail_AsyncPreflightStartedAPI*(AsyncPreflightStartedAPI api) => (Longtail_AsyncPreflightStartedAPI*)api._preflight;
    public AsyncPreflightStartedAPI()
    {
        using var name = new Utf8String(nameof(AsyncPreflightStartedAPI));
        var mem = LongtailLibrary.Longtail_Alloc(name, (ulong)sizeof(AsyncPreflightStartedAPIInternal));
        if (mem == null)
        {
            throw new OutOfMemoryException(nameof(LongtailLibrary.Longtail_Alloc));
        }
        _preflight = (AsyncPreflightStartedAPIInternal*)LongtailLibrary.Longtail_MakeAsyncPreflightStartedAPI(mem, &DisposeFunc, &OnCompletedFunc);
        _waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        _preflight->Handle = GCHandle.Alloc(this);
    }

    public void Wait() => _waitHandle.WaitOne();

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void DisposeFunc(Longtail_API* api)
    {
        var apiInternal = (AsyncPreflightStartedAPIInternal*)api;
        if (apiInternal->Handle.IsAllocated)
        {
            apiInternal->Handle.Free();
        }
        LongtailLibrary.Longtail_Free(api);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void OnCompletedFunc(Longtail_AsyncPreflightStartedAPI* api, uint blockCount, ulong* chunkHashes, int err)
    {
        var apiInternal = (AsyncPreflightStartedAPIInternal*)api;
        var preflightApi = (AsyncPreflightStartedAPI)apiInternal->Handle.Target!;
        preflightApi.Err = (ErrorCodesEnum)err;
        preflightApi._waitHandle.Set();
    }

    public void Dispose()
    {
        if (_preflight != null)
        {
            _waitHandle.Dispose();
            LongtailLibrary.Longtail_DisposeAPI(&_preflight->PreflighApi.m_API);
            _preflight = null;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct AsyncPreflightStartedAPIInternal
    {
        public Longtail_AsyncPreflightStartedAPI PreflighApi;
        public GCHandle Handle;
    }
}