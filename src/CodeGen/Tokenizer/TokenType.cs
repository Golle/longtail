namespace CodeGen.Tokenizer;


/**
 * Here are some tokens we can look at
 * https://github.com/llvm/llvm-project/blob/main/clang/include/clang/Basic/TokenKinds.def
 *
 */
internal enum TokenType
{
    Unknown,
    NewLine,
    Operator,
    CompoundOperator,
    Identifier,
    Comma,
    Punctuation,
    LeftParenthesis,
    RightParenthesis,
    LeftSquareBracket,
    RightSquareBracket, 
    LeftCurlyBracer,
    RightCurlyBracer,
    Colon,
    ColonColon,
    Semicolon,
    Backslash,
    Comment,
    Pointer,
    PreProcessor,
    Character,
    String,
    Number,
    VariadicArgument,
    Statement,
    PrimitiveType,
    Class,
    Struct,
    Enum,
    Union,
    Typedef,
    ClassModifier,
    CallType,
    FunctionModifier,
    CPPKeyword,
    This,
    DllImport,
    DllExport,
    Boolean,
    Null,
    DeclSpec,
    ForceInline,
    Equal,
    EqualEqual,
    StaticAssert,
    CPlusPlus,
    EndOfFile
}