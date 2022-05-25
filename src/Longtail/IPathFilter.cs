namespace Longtail;

public interface IPathFilter
{
    bool Include(string rootPath, string assetPath, string assetName, bool isDir, ulong size, ushort permissions);
}