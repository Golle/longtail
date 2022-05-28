using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Longtail.Internal;

namespace Longtail;

// NOTE(Jens): The log function is a Macro(LONGTAIL_LOG) and takes a va_list as a parameter, not sure how to convert that to C#. Might be able to manually implement the function and do the formatting and just send null in va_list?

public record struct LongtailLogField(string Name, string Value);
public record LongtailLogContext(string File, string Function, int Line, LongtailLogLevel Level, LongtailLogField[] Fields);
public static unsafe class LongtailLog
{
    private static Action<LongtailLogContext?, string>? _callback;
    public static void SetLogCallback(Action<LongtailLogContext?, string>? callback)
    {
        _callback = callback;
        if (callback == null)
        {
            LongtailLibrary.Longtail_SetLog(null, null);
        }
        else
        {
            // TODO(Jens): add implementation for Context,not sure what its used for
            void* context = null;
            LongtailLibrary.Longtail_SetLog(&LogFunc, context);
        }
    }

    public static void SetLogLevel(LongtailLogLevel level) => LongtailLibrary.Longtail_SetLogLevel((int)level);
    public static LongtailLogLevel GetLogLevel() => (LongtailLogLevel)LongtailLibrary.Longtail_GetLogLevel();


    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void LogFunc(Longtail_LogContext* context, byte* str)
    {
        if (_callback == null)
        {
            return;
        }

        _callback(ConvertContext(context), Utf8String.GetString(str));
    }

    private static LongtailLogContext? ConvertContext(Longtail_LogContext* context)
    {
        if (context == null)
        {
            return null;
        }
        
        //context->context // NOTE(Jens): not sure how to handle this

        //NOTE(Jens): There are a lot of allocations to create a log message. We might be able to improve this by using spans/memory.
        // Maybe Lazy evaulate fields, fucntion and file when they are accessed?
        var fields = Enumerable.Range(0, context->field_count)
            .Select(i => new LongtailLogField(Utf8String.GetString(context->fields[i].name), Utf8String.GetString(context->fields[i].value)))
            .ToArray();

        var file = Utf8String.GetString(context->file);
        var function = Utf8String.GetString(context->function);
        var line = context->line;
        var level = (LongtailLogLevel)context->level;
        return new LongtailLogContext(file, function, line, level, fields);
    }
}