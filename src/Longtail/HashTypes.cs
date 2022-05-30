namespace Longtail;

public static class HashTypes
{
    public static readonly uint Meow = LongtailLibrary.Longtail_GetMeowHashType();
    public static readonly uint Blake2 = LongtailLibrary.Longtail_GetBlake2HashType();
    public static readonly uint Blake3 = LongtailLibrary.Longtail_GetBlake3HashType();
}