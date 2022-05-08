using System;

namespace CodeGen.Syntax.Binding;

internal class BinderException : Exception
{
    public BinderException(string message) : base(message)
    {
    }
}