using System.Runtime.CompilerServices;

namespace Longtail;

public class LongtailException : Exception
{
    public string Method { get; }
    public string LibraryFunction { get; }
    public int Err { get; }
    public string ErrorAsString { get; }

    public LongtailException(string libraryFunction, ErrorCodesEnum err, [CallerMemberName] string? method = null)
        : this(libraryFunction, (int)err, method){}

    public LongtailException(string libraryFunction, int err, [CallerMemberName] string? method = null)
        : base($"{method} - {libraryFunction} - Code: {err} ({ErrToString(err)})")
    {
        Method = method ?? "N/A";
        LibraryFunction = libraryFunction;
        Err = err;
        ErrorAsString = ErrToString(err);
    }
    private static string ErrToString(int err) => ((ErrorCodesEnum)err).ToString();
}