using System;
using CodeGen.Logging;
using CodeGen.Parser.Types;
using CodeGen.Tokenizer;

namespace CodeGen.Parser;

internal class SyntaxNode
{

}

internal class LongtailParser
{
    private readonly TypeLookupTable _lookupTable;

    public LongtailParser(TypeLookupTable? lookupTable = null)
    {
        _lookupTable = lookupTable ?? TypeLookupTable.CreateDefault();
    }

    public SyntaxNode Parse(ReadOnlySpan<Token> tokens)
    {
        var cursor = new TokenCursor(tokens);
        do
        {
            ref readonly var token = ref cursor.Current;

            switch (token.Type)
            {
                case TokenType.PreProcessor:
                    ParsePreProcessorDirective(ref cursor);
                    break;
                default:
                    //Logger.Info(token.Value);
                    cursor.Advance();
                    break;
            }

        } while (cursor.Current.Type != TokenType.EndOfFile);


        return null;
    }

    private static void ParsePreProcessorDirective(ref TokenCursor cursor)
    {
        ref readonly var token = ref cursor.Current;
        if (token.Value == "pragma")
        {
            Logger.Trace($"Skipping #pragma on line {token.Line}");
            if (cursor.Peek(1).Value == "once")
            {
                cursor.Advance(2);
                return;
            }
            throw new ParserException(token, "Expected 'once' as the next pragma statement");
        }

        if (token.Value == "include")
        {
            Logger.Trace($"Skipping #include on line {token.Line}"); 
            ref readonly var next = ref cursor.Peek(1);
            if (next.Type == TokenType.String)
            {
                // #include "header.h"
                cursor.Advance(2, false);
                return;
            }

            if (next.Type == TokenType.Operator && next.Value == "<")
            {
                var count = cursor.FindNext(TokenType.Operator, ">");
                if (count == -1)
                {
                    throw new ParserException(token, "Failed to find a closing tag for the include.");
                }

                cursor.Advance((uint)count + 1, false);
                return;
            }
        }

        //NOTE(Jens): this will just ignore anything inside if statements, maybe not what we want?
        //NOTE(Jens): Probably good enough for the naive parser we're building
        if (token.Value is "if" or "ifdef" or "ifndef")
        {
            Logger.Trace($"Skipping the scope of #{token.Value} on line {token.Line}");
            while (cursor.Advance())
            {
                ref readonly var next = ref cursor.Current;
                if (next.Type == TokenType.PreProcessor && next.Value is "if" or "ifdef" or "ifndef")
                {
                    ParsePreProcessorDirective(ref cursor);
                }

                if (next.Type == TokenType.PreProcessor && next.Value == "endif")
                {
                    cursor.Advance();
                    return;
                }
            }
            throw new ParserException(token, $"Failed to find a closing endif for the {token.Value}");
        }

        if (token.Value is "define")
        {
            Logger.Trace($"Skipping #define on line {token.Line}");
            while (true)
            {
                var nextNewLine = cursor.FindNext(TokenType.NewLine);
                if (nextNewLine == -1)
                {
                    throw new ParserException("Define on last line, not sure how to handle that.");
                }
                cursor.Advance((uint)nextNewLine, false);

                var previous = cursor.Peek(-1);
                if (previous.Type == TokenType.Backslash)
                {
                    Logger.Trace("Skipping multiline define");
                    continue;
                }
                break;
            }
            return;
        }

        throw new ParserException(token, "Unhandled PreProcessor directive, must implement.");
    }



}

internal class ParserException : Exception
{
    public ParserException(in Token token, string message) : base($"{message}. Column: {token.Column} Line: {token.Line} Type: {token.Type} Value: {token.Value}")
    {
    }
    public ParserException(string message) : base(message)
    {

    }
}