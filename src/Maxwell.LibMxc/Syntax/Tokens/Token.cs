namespace  Maxwell.LibMxc.Syntax;

public struct Token
{
    public string Value { get; init; }
    public TokenKind Kind { get; init; }
    public uint TextPosition { get; init; }

    public Token(string value, TokenKind kind, uint position)
    {
        Value = value;
        Kind = kind;
        TextPosition = position;
    }

    public Token(TokenKind kind, uint position)
    {
        Value = kind.GetValue();
        Kind = kind;
        TextPosition = position;
    }
}


