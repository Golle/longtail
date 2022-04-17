using CodeGen.Lexer;

namespace CodeGen.Syntax;

internal static class TokenTypeInfo
{

    public static int UnaryOperatorPrecedence(this Token token)
    {
        switch (token.Type)
        {
            // Assignment operators, should they be here?
            case TokenType.PlusPlus: // Suffix/Prefix has different priority...
            case TokenType.MinusMinus:

            case TokenType.Amp: // Address of
            case TokenType.Star: // Dereference
            case TokenType.Tilde:
            case TokenType.Bang:
            case TokenType.Minus:
            case TokenType.Plus:
                return 21;

            case TokenType.PointerStar:
                return 20;
        }
        return 0;
    }


    // https://en.cppreference.com/w/cpp/language/operator_precedence
    public static int BinaryOperatorPrecedence(this Token token)
    {
        switch (token.Type)
        {

            case TokenType.Star:
            case TokenType.Slash:
            case TokenType.Percent:
                return 9;

            case TokenType.Plus:
            case TokenType.Minus:
                return 8;
            
            case TokenType.LessLess:
            case TokenType.LessLessEqual:
            case TokenType.GreaterGreater:
            case TokenType.GreaterGreaterEqual:
                return 7;
            
            case TokenType.Less:
            case TokenType.LessEqual:
            case TokenType.Greater:
            case TokenType.GreaterEqual:
                return 6;

            case TokenType.Amp:
            case TokenType.AmpEqual:
                return 5;
            case TokenType.Caret:
            case TokenType.CaretEqual:
                return 4;
            case TokenType.Pipe:
            case TokenType.PipeEqual:
                return 3;
            case TokenType.AmpAmp:
                return 2;
            case TokenType.PipePipe:
                return 1;
        }

        return 0;
    }
}