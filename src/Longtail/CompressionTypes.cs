namespace Longtail;

public static class CompressionTypes
{
    public static readonly uint BrotliGenericDefaultQuality = LongtailLibrary.Longtail_GetBrotliGenericDefaultQuality();
    public static readonly uint BrotliGenericMinQuality = LongtailLibrary.Longtail_GetBrotliGenericMinQuality();
    public static readonly uint BrotliGenericMaxQuality = LongtailLibrary.Longtail_GetBrotliGenericMaxQuality();
    public static readonly uint BrotliTextDefaultQuality = LongtailLibrary.Longtail_GetBrotliTextDefaultQuality();
    public static readonly uint BrotliTextMinQuality = LongtailLibrary.Longtail_GetBrotliTextMinQuality();
    public static readonly uint BrotliTextMaxQuality = LongtailLibrary.Longtail_GetBrotliTextMaxQuality();
    public static readonly uint LZ4DefaultQuality = LongtailLibrary.Longtail_GetLZ4DefaultQuality();
    public static readonly uint ZStdDefaultQuality = LongtailLibrary.Longtail_GetZStdDefaultQuality();
    public static readonly uint ZStdMinQuality = LongtailLibrary.Longtail_GetZStdMinQuality();
    public static readonly uint ZStdMaxQuality = LongtailLibrary.Longtail_GetZStdMaxQuality();
    public static readonly uint ZStdHighQuality = LongtailLibrary.Longtail_GetZStdHighQuality();
    public static readonly uint ZStdLowQuality = LongtailLibrary.Longtail_GetZStdLowQuality();
}