namespace Longtail;

public unsafe class CancelToken
{
    private Longtail_CancelAPI_CancelToken* _token;

    // NOTE(Jens): can this be replaced by CancellationToken ?
    internal Longtail_CancelAPI_CancelToken* AsPointer() => _token;
}