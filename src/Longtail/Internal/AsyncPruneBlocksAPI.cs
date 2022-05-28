using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Longtail.Internal;

internal unsafe class AsyncPruneBlocksAPI : IDisposable
{
    private AsyncPruneBlocksAPIInternal* _pruneApi;
    public ErrorCodesEnum Err { get; private set; }
    public uint PrunedBlockCount { get; private set; }

    private readonly EventWaitHandle _waitHandle;

    public static implicit operator Longtail_AsyncPruneBlocksAPI*(AsyncPruneBlocksAPI api) => (Longtail_AsyncPruneBlocksAPI*)api._pruneApi;
    public void Wait() => _waitHandle.WaitOne();
    public AsyncPruneBlocksAPI()
    {
        using var name = new Utf8String(nameof(AsyncPruneBlocksAPI));
        var mem = LongtailLibrary.Longtail_Alloc(name, (ulong)sizeof(AsyncPruneBlocksAPIInternal));
        if (mem == null)
        {
            throw new OutOfMemoryException(nameof(LongtailLibrary.Longtail_Alloc));
        }
        _pruneApi = (AsyncPruneBlocksAPIInternal*)LongtailLibrary.Longtail_MakeAsyncPruneBlocksAPI(mem, &DisposeFunc, &OnCompletedFunc);
        _waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        _pruneApi->Handle = GCHandle.Alloc(this);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void DisposeFunc(Longtail_API* api)
    {
        var pruneBlocksApi = (AsyncPruneBlocksAPIInternal*)api;
        if (pruneBlocksApi->Handle.IsAllocated)
        {
            pruneBlocksApi->Handle.Free();
        }
        LongtailLibrary.Longtail_Free(api);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void OnCompletedFunc(Longtail_AsyncPruneBlocksAPI* api, uint prunedBlockCount, int err)
    {
        var pruneApiInternal = (AsyncPruneBlocksAPIInternal*)api;
        var pruneApi = (AsyncPruneBlocksAPI)pruneApiInternal->Handle.Target!;
        pruneApi.Err = (ErrorCodesEnum)err;
        pruneApi.PrunedBlockCount = prunedBlockCount;
        pruneApi._waitHandle.Set();
    }

    public void Dispose()
    {
        if (_pruneApi != null)
        {
            LongtailLibrary.Longtail_DisposeAPI(&_pruneApi->PruneApi.m_API);
            _pruneApi = null;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct AsyncPruneBlocksAPIInternal
    {
        public Longtail_AsyncPruneBlocksAPI PruneApi;
        public GCHandle Handle;
    }
}