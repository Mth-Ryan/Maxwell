namespace Maxwell.Mxc.Syntax;

public struct Token
{
    public String Value { get; }
    public TokenKind Kind { get; }
    public TokenPosition Position { get; }

    public Token(String value, TokenKind kind, TokenPosition position)
    {
        this.Value = value;
        this.Kind = kind;
        this.Position = position;
    }

    public bool IsOpenPair()
    {
        return this.Kind == TokenKind.LeftParen
            || this.Kind == TokenKind.LeftBrace
            || this.Kind == TokenKind.LeftBrack;
    }

    public bool IsClosePair()
    {
        return this.Kind == TokenKind.RightParen
            || this.Kind == TokenKind.RightBrace
            || this.Kind == TokenKind.RightBrack;
    }

    public override String ToString()
    {
        if (IsOpenPair())
            return $"<{this.Kind.ToString()}>";
        else if (IsClosePair())
            return $"</{this.Kind.ToString()}>";
        else if (this.Kind == TokenKind.Indent
             ||  this.Kind == TokenKind.EndOfFile)
            return $"<{this.Kind.ToString()} />";
        else
            return $"<{this.Kind.ToString()} value=\"{this.Value}\" position=\"{this.Position.ToString()}\" />";
    }
}