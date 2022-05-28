using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Longtail.Internal;

namespace Longtail;

public static unsafe class MemTracer
{
    private static volatile int _allocations;
    private static volatile int _deallocations;
    private static long _memoryAllocated;
    private static bool _initialized;

    public static int Allocations => _allocations;
    public static int Deallocations => _deallocations;
    public static long MemoryAllocated => _memoryAllocated;
    public static void Init()
    {
        if (!_initialized)
        {
            LongtailLibrary.Longtail_MemTracer_Init();
            LongtailLibrary.Longtail_SetAllocAndFree(&Alloc, &Free);
            _initialized = true;
        }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void* Alloc(byte* context, ulong size)
    {
        Interlocked.Add(ref _memoryAllocated, (long)size);
        Interlocked.Increment(ref _allocations);
        return LongtailLibrary.Longtail_MemTracer_Alloc(context, size);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void Free(void* p)
    {
        if (p != null)
        {
            // Only count memory that will actually be freed.
            Interlocked.Increment(ref _deallocations);
        }
        LongtailLibrary.Longtail_MemTracer_Free(p);
    }

    public static void DumpStats(string path)
    {
        using var utf8Path = new Utf8String(path);
        var err = LongtailLibrary.Longtail_MemTracer_DumpStats(utf8Path);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_MemTracer_DumpStats), err);
        }
    }

    public static string GetStatsSummary()
        => Utf8String.GetString(LongtailLibrary.Longtail_MemTracer_GetStats(LongtailLibrary.Longtail_GetMemTracerSummary()));

    public static string GetStatsDetailed()
        => Utf8String.GetString(LongtailLibrary.Longtail_MemTracer_GetStats(LongtailLibrary.Longtail_GetMemTracerDetailed()));

    public static void Dispose()
    {
        if (_initialized)
        {
            LongtailLibrary.Longtail_MemTracer_Dispose();
            LongtailLibrary.Longtail_SetAllocAndFree(null, null);
            _allocations = 0;
            _deallocations = 0;
            _memoryAllocated = 0;
            _initialized = false;
        }
    }
}