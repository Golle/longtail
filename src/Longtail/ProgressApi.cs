using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Longtail.Internal;

namespace Longtail;

public unsafe class ProgressApi : IDisposable
{
    private readonly Action<(uint DoneCount, uint TotalCount)> _callback;
    private ProgressApiInternal* _progressApi;

    public ProgressApi(Action<(uint DoneCount, uint TotalCount)> callback)
    {
        _callback = callback;
        using var name = new Utf8String(nameof(ProgressApi));
        var mem = LongtailLibrary.Longtail_Alloc(name, (ulong)sizeof(ProgressApiInternal));
        if (mem == null)
        {
            throw new LongtailException(nameof(ProgressApi), "ctor", nameof(LongtailLibrary.Longtail_Alloc), ErrorCodes.ENOMEM);
        }
        var result = LongtailLibrary.Longtail_MakeProgressAPI(mem, &DisposeFunc, &OnProgressFunc);
        if (result == null)
        {
            throw new LongtailException(nameof(ProgressApi), "ctor", nameof(LongtailLibrary.Longtail_MakeProgressAPI), ErrorCodes.ENOMEM);
        }

        _progressApi = (ProgressApiInternal*)mem;
        _progressApi->Handle = GCHandle.Alloc(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnProgress(uint totalCount, uint doneCount)
        => LongtailLibrary.Longtail_Progress_OnProgress((Longtail_ProgressAPI*)_progressApi, totalCount, doneCount);

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void OnProgressFunc(Longtail_ProgressAPI* api, uint totalCount, uint doneCount)
    {
        var extended = (ProgressApiInternal*)api;
        if (extended->Handle.IsAllocated)
        {
            var progress = extended->Handle.Target as ProgressApi;
            progress?._callback((doneCount, totalCount));
        }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void DisposeFunc(Longtail_API* api)
    {
        var extended = (ProgressApiInternal*)api;
        if (extended->Handle.IsAllocated)
        {
            extended->Handle.Free();
        }
        LongtailLibrary.Longtail_Free(api);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct ProgressApiInternal
    {
        public Longtail_ProgressAPI ProgressApi;
        public GCHandle Handle;
    }

    public void Dispose()
    {
        if (_progressApi != null)
        {
            LongtailLibrary.Longtail_DisposeAPI(&_progressApi->ProgressApi.m_API);
            _progressApi = null;
        }
    }
}