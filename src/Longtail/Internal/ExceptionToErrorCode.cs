namespace Longtail.Internal;

internal static class ExceptionToErrorCode
{
    // TODO: add more exceptions
    public static ErrorCodesEnum Translate(Exception? e) =>
        e switch
        {
            FileNotFoundException => ErrorCodesEnum.ENOENT,
            IOException => ErrorCodesEnum.EIO,
            NotSupportedException or NotImplementedException => ErrorCodesEnum.ENOTSUP,
            OutOfMemoryException => ErrorCodesEnum.ENOMEM,
            ArgumentException => ErrorCodesEnum.EINVAL,
            TaskCanceledException or OperationCanceledException => ErrorCodesEnum.ECANCELED,

            // NOTE(Jens): not really sure how to handle AggreateExceptions.
            AggregateException aggregateException => Translate(aggregateException.GetBaseException()),

            // If there's no exception, return success
            null => ErrorCodesEnum.SUCCESS,

            // Any other exception type, return EFAULT
            _ => ErrorCodesEnum.EFAULT
        };
}