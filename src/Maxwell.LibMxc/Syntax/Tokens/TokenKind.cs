namespace Maxwell.LibMxc.Syntax;

public enum TokenKind
{
    // Internal
    Invalid,
    WhiteSpace,
    EOF,        // \0

    // Literals
    Identifier, // [a-zA-Z][a-zA-Z0-9]+
    Int,        // [0-9]+
    Float,      // [0-9]+\,[0-9]+
    String,     // ^"*."$

    // Syntax Specific
    LeftParen,
    RightParen,
    LeftBrack,
    RightBrack,
    LeftBrace,
    RightBrace,
    Quote,

    // Keywords
    Def,
    Lambda,
    If,
    Cond
}

public static class TokenKindExtensions
{
    public static string GetValue(this TokenKind kind)
    {
        switch (kind)
        {
            case TokenKind.EOF: return "\0";

            case TokenKind.LeftParen: return "(";
            case TokenKind.RightParen: return ")";
            case TokenKind.LeftBrack: return "[";
            case TokenKind.RightBrack: return "]";
            case TokenKind.LeftBrace: return "{";
            case TokenKind.RightBrace: return "}";
            case TokenKind.Quote: return "'";
        }
        return $"<{kind.ToString()}>";
    }

    public static bool IsKeyword(this TokenKind kind)
    {
        return kind == TokenKind.Def ||
            kind == TokenKind.Lambda ||
            kind == TokenKind.If ||
            kind == TokenKind.Cond;
    }

    public static bool IsLiteral(this TokenKind kind)
    {
        return kind == TokenKind.Identifier ||
            kind == TokenKind.Int ||
            kind == TokenKind.Float ||
            kind == TokenKind.String;
    }

    public static bool IsOpenToken(this TokenKind kind)
    {
        return kind == TokenKind.LeftParen ||
            kind == TokenKind.LeftBrack ||
            kind == TokenKind.LeftBrace;
    }

    public static bool IsCloseToken(this TokenKind kind)
    {
        return kind == TokenKind.RightParen ||
            kind == TokenKind.RightBrack ||
            kind == TokenKind.RightBrace;
    }
}
