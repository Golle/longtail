namespace CodeGen.Lexer;


/**
 * Here are some tokens we can look at
 * https://github.com/llvm/llvm-project/blob/main/clang/include/clang/Basic/TokenKinds.def
 *
 */
internal enum TokenType
{
    Unknown,

    //If, Else, When, For, Switch, Case, Default, Continue, Break, Do, Return, Union, Goto, Try, Catch, Throw, NOTE(Jens); Should we use more explicit tokens?
    Statement,
    
    // Function/type modifiers etc
    DeclSpec,
    ForceInline,
    ClassModifier,
    CallType,
    FunctionModifier,
    CPPKeyword,
    VariadicArgument,
    
    // DLL
    DllImport,
    DllExport,
    
    
    // Scopes
    LeftParenthesis,
    RightParenthesis,
    LeftSquareBracket,
    RightSquareBracket,
    LeftCurlyBracer,
    RightCurlyBracer,
    Colon,
    ColonColon,
    Semicolon,
    Pointer,

    // Types
    Identifier,
    Class,
    Struct,
    Enum,
    Union,
    Typedef,
    Boolean,
    PrimitiveType,
    Character,
    String,
    Number,
    This,
    Null,

    // Operators
    Equal,
    EqualEqual,
    Operator,
    CompoundOperator,
    Comma,
    Punctuation,

    // Special
    Invalid,
    CPlusPlus,
    NewLine,
    Backslash,
    Comment,
    StaticAssert,
    PreProcessor,

    // EOF
    EndOfFile,
}