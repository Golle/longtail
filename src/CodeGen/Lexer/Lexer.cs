using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using CodeGen.IO;
using CodeGen.Logging;

namespace CodeGen.Lexer;

internal enum PreProcessorDirective
{
    Unknown,
    If,
    EndIf,
    ElIf,
    Else,
    Error,
    IfDef,
    IfNDef,
    Define,
    Undef,
    Include,
    Pragma
}

internal enum SyntaxTokenType
{
    Unknown,
    String,
    Integer,
    Identifier,
    PreprocessorDirective,
    SimpleOperator,
    Operator,
    Comment,
    NewLine,
    Error
}


[StructLayout(LayoutKind.Explicit, Pack = 4)]
[DebuggerDisplay("Type = {TypeToString()}, Value = {ToString()}")]
internal struct SyntaxToken
{
    [FieldOffset(0)]
    public SyntaxTokenType Type;
    [FieldOffset(4)]
    public char SingleValue;
    [FieldOffset(8)]
    public string Value;
    [FieldOffset(16)]
    public PreProcessorDirective PreProcessorDirective;
    [FieldOffset(16)]
    public int b; // operators?
    [FieldOffset(16)]
    public int c; // something else?

    public string TypeToString() => Type == SyntaxTokenType.PreprocessorDirective ? PreProcessorDirective.ToString() : Type.ToString();

    public override string ToString()
    {
        if (!string.IsNullOrEmpty(Value))
        {
            return Value;
        }
        return SingleValue != '\0' ? SingleValue.ToString() : string.Empty;
    }
}

internal class Lexer
{
    private readonly Cursor _cursor;
    public Lexer(string fileContents)
    {
        _cursor = new(fileContents);
    }

    public IReadOnlyList<SyntaxToken> Process()
    {
        Span<char> lineBuffer = stackalloc char[1000];
        var bufferCount = 0;
        var tokens = new List<SyntaxToken>(10_000);
        while (_cursor.Current != Cursor.InvalidCharacter)
        {
            var token = new SyntaxToken();
            switch (_cursor.Current)
            {
                case '/' or '*' or '!' or '+' or '-' or '^' or '&' or '=' or '?' or '~' or ':' or ';' or '<' or '>' or '%' or '|' or '.' or ',' or '{' or '}' or '[' or ']' or '(' or ')' or '\\' or ':' or '~':
                    OperatorOrComment(ref token);
                    break;
                case '"' or '\'':
                    StringLiteral(ref token);
                    break;
                case >= '0' and <= '9':
                    Integer(ref token);
                    break;
                case ' ' or '\t' or '\r':
                    break;
                case '\n':
                    token.Type = SyntaxTokenType.NewLine;
                    break;
                case '#':
                    if (IsFirstCharacter(lineBuffer, bufferCount))
                    {
                        PreProcessor(ref token);
                    }
                    else
                    {
                        goto default;
                    }
                    break;
                default:
                    // treat everything else as an identifier
                    Identifier(ref token);
                    break;
            }

            if (token.Type == SyntaxTokenType.Error)
            {
                Logger.Error($"Failed to read token on line {_cursor.Line} column {_cursor.Column} with message: {token.Value}");
                Logger.Error(_cursor.GetSurroundingString(20).ToString());
            }

            if (token.Type != SyntaxTokenType.Unknown)
            {
                tokens.Add(token);
            }

            // This wont reset the line buffer if there's a new line inside a comment
            if (token.Type == SyntaxTokenType.NewLine)
            {
                bufferCount = 0;
            }
            else
            {
                lineBuffer[bufferCount++] = _cursor.Current;
            }
            _cursor.Advance();
        }

        Logger.Info($"EOF reached. {tokens.Count} tokens added. ({tokens.Count(t => t.Type == SyntaxTokenType.Error)} error(s))");
        return tokens;


        static bool IsFirstCharacter(ReadOnlySpan<char> line, int count)
        {
            for (var i = 0; i < count; ++i)
            {
                var c = line[i];
                if (c != '\t' && c != '\n' && c != '\r' && c != ' ')
                {
                    return false;
                }
            }

            return true;
        }
    }

    private void OperatorOrComment(ref SyntaxToken token)
    {
        if (_cursor.Current == '/' && _cursor.Peek() is '*' or '/')
        {
            Comment(ref token);
        }
        else
        {
            Operator(ref token);
        }
    }


    private void PreProcessor(ref SyntaxToken token)
    {
        if (!SyntaxLiteral.ReadPreprocessorDirective(_cursor, ref token))
        {
            token.Type = SyntaxTokenType.Error;
            token.Value = "Failed to read PreProcessor directive.";
        }
    }

    private void Identifier(ref SyntaxToken token)
    {
        if (!SyntaxLiteral.ReadIdentifier(_cursor, ref token))
        {
            token.Type = SyntaxTokenType.Error;
            token.Value = "Failed to read Identifier.";
        }
    }

    private void Integer(ref SyntaxToken token)
    {
        if (!SyntaxLiteral.ReadIntegerLiteral(_cursor, ref token))
        {
            token.Type = SyntaxTokenType.Error;
            token.Value = "Failed to read Integer.";
        }
    }

    private void Comment(ref SyntaxToken token)
    {
        if (!SyntaxLiteral.ReadComment(_cursor, ref token))
        {
            token.Type = SyntaxTokenType.Error;
            token.Value = "Failed to read Comment.";
        }
    }
    private void Operator(ref SyntaxToken token)
    {
        if (!SyntaxLiteral.ReadOperator(_cursor, ref token))
        {
            token.Type = SyntaxTokenType.Error;
            token.Value = "Failed to read Comment.";
        }
    }

    private void StringLiteral(ref SyntaxToken token)
    {
        if (!SyntaxLiteral.ReadStringLiteral(_cursor, ref token))
        {
            token.Type = SyntaxTokenType.Error;
            token.Value = "Failed to read StringLiteral.";
        }
    }
}