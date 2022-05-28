using System.Runtime.InteropServices;
using System.Text;

namespace Longtail.Internal;
/// <summary>
/// Abstraction for creating a UTF8 String, make sure you Dispose it.
/// </summary>
internal unsafe struct Utf8String : IDisposable
{
    private byte* _ptr;
    public Utf8String(string str)
    {
        var strLength = str.Length * 2 + 1;
#if NET6_0_OR_GREATER
        _ptr = (byte*)NativeMemory.Alloc((nuint)strLength);
#else
        _ptr = (byte*)Marshal.AllocHGlobal(strLength);
#endif
        var length = Encoding.UTF8.GetBytes(str, new Span<byte>(_ptr, strLength));
        _ptr[length] = 0;
    }
    public override string ToString() => GetString(_ptr);
    public static string GetString(byte* cString)
    {
        if (cString == null)
        {
            return string.Empty;
        }
        var length = 0;
        while (cString[length] != 0)
        {
            length++;
        }
        return Encoding.UTF8.GetString(cString, length);
    }

    public static implicit operator byte*(in Utf8String str) => str._ptr;
    public static implicit operator void*(in Utf8String str) => str._ptr;

    public void Dispose()
    {
        if (_ptr != null)
        {
#if NET6_0_OR_GREATER
            NativeMemory.Free(_ptr);
#else
            Marshal.FreeHGlobal((IntPtr)_ptr);
#endif
            _ptr = null;
        }
    }
}