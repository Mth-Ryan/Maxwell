namespace Maxwell.LibMxc.Syntax;

public static class TokenRules
{
    public static bool IsWhitespace(char c) => Char.IsWhiteSpace(c);

    public static bool IsIdentifierFirstChar(char c) => Char.IsLetter(c) || c == '_';
    public static bool IsIdentifierChar(char c) => IsIdentifierFirstChar(c) || Char.IsDigit(c);

    public static bool IsNumberChar(char c) => Char.IsDigit(c);

    public static bool IsStringDelimiter(char c) => c == '"';

    public static TokenKind GetIdentifierKind(string value)
    {
        switch (value)
        {
            case "def": return TokenKind.Def;
            case "lambda": return TokenKind.Lambda;
            case "if": return TokenKind.If;
            case "cond": return TokenKind.Cond;
            default: return TokenKind.Identifier;
        }
    }
}
