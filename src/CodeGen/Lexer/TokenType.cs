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
    

    // Built in types
    Signed,
    Unsigned,
    Int,
    Long,
    Short,
    Char,
    Bool,
    Double,
    Float,
    Void,

    // Function/type modifiers etc
    DeclSpec,
    ForceInline,
    ClassModifier,
    CallType,
    FunctionModifier,
    CPPKeyword,
    VariadicArgument,

    Static,
    Extern,
    Const,

    // DLL
    DllImport,
    DllExport,
    

    // Statements
    If,
    Else,
    When,
    For,
    Switch,
    Case,
    Default,
    Continue,
    Break,
    Do,
    //Union,
    Return, 
    Goto,
    Try,
    Catch,
    Throw,
    
    // Scopes
    LeftParenthesis,
    RightParenthesis,
    LeftSquareBracket,
    RightSquareBracket,
    LeftCurlyBracer,
    RightCurlyBracer,
    

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
    Colon,
    ColonColon,
    Semicolon,
    Pointer,
    Equal,
    EqualEqual,
    Amp,
    AmpAmp,
    AmpEqual,
    Star,
    StarEqual,
    Plus,
    PlusPlus,
    PlusEqual,
    Minus,
    MinusMinus,
    MinusEqual,
    Tilde, 
    Bang,
    BangEqual,
    Slash,
    SlashEqual,
    Percent,
    PercentEqual,
    Less,
    LessLess,
    LessEqual,
    LessLessEqual,
    Spaceship, // Name from llvm, <=>
    Greater,
    GreaterGreater,
    GreaterEqual,
    GreaterGreaterEqual,
    Caret, // ^
    CaretEqual,
    Pipe,
    PipeEqual,
    PipePipe,
    Question,
    // These are not used by the operator parser yet (Used in PreProcessor parsing)
    Hash,
    HashHash,
    HashHat, // #@
    // End not supported

    PeriodStar, // .*, pointer-to-member.  https://stackoverflow.com/questions/2548555/dot-asterisk-operator-in-c
    PointerStar, // ->* 

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

    // EOF/EOD
    EndOfDirective, // End of preprocessor directive
    EndOfFile,// End of the file
    
}