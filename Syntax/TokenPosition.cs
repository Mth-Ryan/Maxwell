namespace Syntax;

public struct TokenPosition
{
    public uint Line { get; }
    public uint Column { get; }

    public TokenPosition(uint l, uint c)
    {
        this.Line = l;
        this.Column = c;
    }

    public override string ToString()
    {
        return $"{this.Line}, {this.Column}";
    }
}