using CodeGen.Logging;

namespace CodeGen;

public interface IPrettyPrint
{
    void Write(string message, int indentation);
}

internal class SyntaxConsoleWriter : IPrettyPrint
{
    public void Write(string message, int indentation)
    {
        Logger.Raw($"{new string(' ', indentation)}{message}");
    }
}