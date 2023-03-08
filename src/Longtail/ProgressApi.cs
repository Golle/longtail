using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Longtail.Internal;

namespace Longtail;

public sealed unsafe class ProgressApi : IDisposable
{
    private readonly Action<(uint DoneCount, uint TotalCount)> _callback;

    private ProgressApiInternal* _progressApi;
    internal Longtail_ProgressAPI* AsPointer() => _progressApi->RateLimitedProgressApi != null ? _progressApi->RateLimitedProgressApi : (Longtail_ProgressAPI*)_progressApi;

    private ProgressApi(ProgressApiInternal* progressApi, Action<(uint DoneCount, uint TotalCount)> callback)
    {
        _callback = callback;
        _progressApi = progressApi;
        _progressApi->Handle = GCHandle.Alloc(this);
    }

    public static ProgressApi Create(Action<(uint DoneCount, uint TotalCount)> callback)
    {
        using var name = new Utf8String(nameof(ProgressApi));
        var mem = LongtailLibrary.Longtail_Alloc(name, (ulong)sizeof(ProgressApiInternal));
        if (mem == null)
        {
            throw new OutOfMemoryException(nameof(LongtailLibrary.Longtail_Alloc));
        }
        var result = LongtailLibrary.Longtail_MakeProgressAPI(mem, &DisposeFunc, &OnProgressFunc);
        if (result == null)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_MakeProgressAPI), ErrorCodes.ENOMEM);
        }

        var progressApi = (ProgressApiInternal*)mem;
        progressApi->RateLimitedProgressApi = null;
        return new ProgressApi(progressApi, callback);
    }

    public static ProgressApi CreateRateLimitedProgress(Action<(uint DoneCount, uint TotalCount)> callback, uint percentRateLimit)
    {
        using var name = new Utf8String(nameof(ProgressApi));
        var mem = LongtailLibrary.Longtail_Alloc(name, (ulong)sizeof(ProgressApiInternal));
        if (mem == null)
        {
            throw new OutOfMemoryException(nameof(LongtailLibrary.Longtail_Alloc));
        }
        var result = LongtailLibrary.Longtail_MakeProgressAPI(mem, &DisposeFunc, &OnProgressFunc);
        if (result == null)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_MakeProgressAPI), ErrorCodes.ENOMEM);
        }

        var rateLimitProgress = LongtailLibrary.Longtail_CreateRateLimitedProgress(result, percentRateLimit);
        if (rateLimitProgress == null)
        {
            LongtailLibrary.Longtail_DisposeAPI(&result->m_API);
            throw new OutOfMemoryException(nameof(LongtailLibrary.Longtail_Alloc));
        }

        var progressApi = (ProgressApiInternal*)mem;
        progressApi->RateLimitedProgressApi = rateLimitProgress;
        return new ProgressApi(progressApi, callback);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnProgress(uint totalCount, uint doneCount)
        => LongtailLibrary.Longtail_Progress_OnProgress(AsPointer(), totalCount, doneCount);

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
        public Longtail_ProgressAPI* RateLimitedProgressApi;
        public GCHandle Handle;
    }

    public void Dispose()
    {
        if (_progressApi != null)
        {
            if (_progressApi->RateLimitedProgressApi != null)
            {
                LongtailLibrary.Longtail_DisposeAPI(&_progressApi->RateLimitedProgressApi->m_API);
            }
            else
            {
                LongtailLibrary.Longtail_DisposeAPI(&_progressApi->ProgressApi.m_API);
            }
            _progressApi = null;
        }
    }
}