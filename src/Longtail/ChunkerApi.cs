namespace Longtail;

public unsafe class ChunkerApi
{
    private Longtail_ChunkerAPI* _chunkerApi;
    internal Longtail_ChunkerAPI* AsPointer() => _chunkerApi;
}