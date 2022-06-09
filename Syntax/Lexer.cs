namespace Syntax;

class LineLexer
{
    private readonly String _source;
    private readonly uint _line;
    private uint _position;
    private char _char
    {
        get => _position < _source.Length 
            ? _source[(int) _position]
            : '\n';
    }

    public LineLexer(String source, uint line)
    {
        this._source = source;
        this._line = line;
    }

    private void Next()
    {
        _position++;
    } 

    private Token TokenizeSpecialChar()
    {
        switch (_char)
        {
            case '\'': return TokenKind.Quote;
            case '(':  return TokenKind.LeftParen;
            case ')':  return TokenKind.RightParen;
            case '{':  return TokenKind.LeftBrace;
            case '}':  return TokenKind.RightBrace;
            case '[':  return TokenKind.LeftBrack;
            case ']':  return TokenKind.RightBrace;
            case '\n': return TokenKind.EndOfLine;
        }
    }
}