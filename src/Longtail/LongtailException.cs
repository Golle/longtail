namespace Longtail;

public class LongtailException : Exception
{
    public string Class { get; }
    public string Method { get; }
    public string LibraryFunction { get; }
    public int Err { get; }
    public string ErrorAsString { get; }
    public LongtailException(string @class, string method, string libraryFunction, int err) 
        : base($"{@class}:{method} - {libraryFunction} - Code: {err} ({ErrToString(err)})")
    {
        Class = @class;
        Method = method;
        LibraryFunction = libraryFunction;
        Err = err;
        ErrorAsString = ErrToString(err);
    }

    private static string ErrToString(int err) => ((ErrorCodesEnum)err).ToString();
}