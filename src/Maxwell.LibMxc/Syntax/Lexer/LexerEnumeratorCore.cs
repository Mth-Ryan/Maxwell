namespace Maxwell.LibMxc.Syntax;

public partial class LexerEnumerator
{
    private readonly string _src;
    private uint _pos = 0;
    private char _ch
    {
        get
        {
            if (_src.Length < 1 || _pos >= _src.Length)
            {
                return '\u0000';
            }
            return _src[(int)_pos];
        }
    }

    public LexerEnumerator(string source)
    {
        _src = source;
    }

    private char Next()
    {
        _pos++;
        return _ch;
    }

    private string AdvanceWhile(Func<char, bool> cond)
    {
        var value = "";
        while (cond(_ch))
        {
            value += _ch;
            Next();
        }
        return value;
    }

    private Token TokenizeWhiteSpaces()
    {
        var pos = _pos;
        return new Token(AdvanceWhile(TokenRules.IsWhitespace), TokenKind.WhiteSpace, pos);
    }

    private Token TokenizeIdentifierOrKeyword()
    {
        var pos = _pos;
        var value = AdvanceWhile(TokenRules.IsIdentifierChar);
        var kind = TokenRules.GetIdentifierKind(value);
        return new Token(value, kind, pos);
    }

    private Token TokenizeNumber()
    {
        var pos = _pos;
        var lhs = AdvanceWhile(TokenRules.IsNumberChar);
        if (_ch == '.')
        {
            Next();
            var rhs = AdvanceWhile(TokenRules.IsNumberChar);
            return new Token($"{lhs}.{rhs}", TokenKind.Float, pos);
        }
        return new Token(lhs, TokenKind.Int, pos);
    }

    private Token TokenizeString()
    {
        var pos = _pos;

        Next(); // Skip first delimiter
        var value = AdvanceWhile(c => !TokenRules.IsStringDelimiter(c));
        Next(); // Skip last delimiter

        return new Token(value, TokenKind.String, pos);
    }

    private Token TokenizeRest()
    {
        var pos = _pos;
        var ch = _ch;

        Next();
        switch (ch)
        {
            case '(': return new Token(TokenKind.LeftParen, pos);
            case ')': return new Token(TokenKind.RightParen, pos);
            case '[': return new Token(TokenKind.LeftBrack, pos);
            case ']': return new Token(TokenKind.RightBrack, pos);
            case '{': return new Token(TokenKind.LeftBrace, pos);
            case '}': return new Token(TokenKind.RightBrace, pos);
            case '\'': return new Token(TokenKind.Quote, pos);
            case '\u0000': return new Token(TokenKind.EOF, pos);
            default: return new Token(ch.ToString(), TokenKind.Invalid, pos);
        }
    }

    private Token NextToken()
    {
        if (TokenRules.IsWhitespace(_ch))
            return TokenizeWhiteSpaces();
        else if (TokenRules.IsIdentifierFirstChar(_ch))
            return TokenizeIdentifierOrKeyword();
        else if (TokenRules.IsNumberChar(_ch))
            return TokenizeNumber();
        else if (TokenRules.IsStringDelimiter(_ch))
            return TokenizeString();
        else
            return TokenizeRest();
    }
}
