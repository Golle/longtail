using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Longtail.Internal;

namespace Longtail;

public unsafe class PathFilterApi : IDisposable
{
    private Longtail_PathFilterAPI* _pathFilterApi;
    private PathFilterApi(Longtail_PathFilterAPI* filter)
    {
        _pathFilterApi = filter;
    }

    internal Longtail_PathFilterAPI* AsPointer() => _pathFilterApi;
    public bool Include(string rootPath, string assetPath, string assetName, bool isDir, ulong size, ushort permissions)
    {
        // NOTE(Jens): this will allocate a lot of memory, stackalloc or shared array can be used
        using var rootPathUtf8 = new Utf8String(rootPath);
        using var assetPathUtf8 = new Utf8String(assetPath);
        using var assetNameUtf8 = new Utf8String(assetName);
        return LongtailLibrary.Longtail_PathFilter_Include(_pathFilterApi, rootPathUtf8, assetPathUtf8, assetNameUtf8, isDir ? 1 : 0, size, permissions) != 0;
    }

    public static PathFilterApi MakePathFilterApi(IPathFilter pathFilter)
    {
        using var name = new Utf8String(nameof(PathFilterApi));
        var mem = (Longtail_PathFilterAPI*)LongtailLibrary.Longtail_Alloc(name, (ulong)sizeof(PathFilterInternal));
        if (mem == null)
        {
            throw new OutOfMemoryException();
        }

        mem = LongtailLibrary.Longtail_MakePathFilterAPI(mem, &DisposeFunc, &IncludeFunc);

        var filter = (PathFilterInternal*)mem;
        filter->Handle = GCHandle.Alloc(pathFilter);
        return new PathFilterApi(mem);
    }


    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void DisposeFunc(Longtail_API* api)
    {
        var pathFilter = ((PathFilterInternal*)api);
        if (pathFilter->Handle.IsAllocated)
        {
            pathFilter->Handle.Free();
        }
        LongtailLibrary.Longtail_Free(pathFilter);
    }


    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static int IncludeFunc(Longtail_PathFilterAPI* pathFilterApi, byte* rootPath, byte* assetPath, byte* assetName, int isDir, ulong size, ushort permissions)
    {
        var pathFilter = (IPathFilter)((PathFilterInternal*)pathFilterApi)->Handle.Target!;
        return pathFilter.Include(
            Utf8String.GetString(rootPath),
            Utf8String.GetString(assetPath),
            Utf8String.GetString(assetName),
            isDir != 0,
            size,
            permissions
        ) ? 1 : 0;
    }

    private struct PathFilterInternal
    {
        public Longtail_ProgressAPI PathFilterApi;
        public GCHandle Handle;
    }

    public void Dispose()
    {
        if (_pathFilterApi != null)
        {
            LongtailLibrary.Longtail_DisposeAPI(&_pathFilterApi->m_API);
            _pathFilterApi = null;
        }
    }
}