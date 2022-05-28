namespace Longtail;

public unsafe class CancelApi
{
    private Longtail_CancelAPI* _cancelApi;
    internal Longtail_CancelAPI* AsPointer() => _cancelApi;
}