namespace CodeGen.Syntax;

internal enum TokenKind
{
    // Operators
    Plus,
    Minus,
    Slash,
    Star,

    Identifier,
    Integer,
    Punctation,
    BackSlash,



    // Space etc
    Whitespace,
    Newline,
    CarriageReturn,
    Tab,
    // Special
    Invalid,
    EndOfFile,


    
}