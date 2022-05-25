namespace Longtail;

public unsafe class PathFilterApi
{
    private Longtail_PathFilterAPI* _pathFilterApi;

    internal Longtail_PathFilterAPI* AsPointer() => _pathFilterApi;
}