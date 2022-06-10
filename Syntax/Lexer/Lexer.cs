using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace Maxwell.Syntax;

public class Lexer : IEnumerable<Token>
{
    private LexerEnumerator _enumerator; 
    public Lexer(String source)
    {
        _enumerator = new LexerEnumerator(source);
    }

    public IEnumerator<Token> GetEnumerator() => _enumerator;

    IEnumerator IEnumerable.GetEnumerator() => _enumerator;
}

internal class LexerEnumerator : IEnumerator<Token>
{
    private readonly String _source;
    private String[] _sourceMatrix;
    private uint _line;
    private uint _column;
    private char _char
    {
        get
        {
            if (_line >= _sourceMatrix.GetLength(0))
                return '\0';
            else if (_column >= _sourceMatrix[_line].Length)
                return '\n';
            else 
                return _sourceMatrix[(int) _line][(int) _column];
        }
    }

    private Token _current;

    public Token Current => _current;

    object IEnumerator.Current => _current;

    public LexerEnumerator(String source)
    {
        this._source = source;
        this._sourceMatrix = Regex.Split(_source, LexerRules.NewLinePatter);
        this._line = 0;
        this._column = 0;
        this._current = new Token("", TokenKind.Invalid, new TokenPosition(0, 0));
    }

    private void Next()
    {
        if (_line < _sourceMatrix.GetLength(0) && _column < _sourceMatrix[_line].Length)
            _column++;
        else
        {
            _line++;
            _column = 0;
        }
    }

    private TokenPosition GetPosition() =>
        new TokenPosition(_line + 1, _column + 1);

    private String AdvanceWhile(Func<char, bool> cond)
    {
        var builder = new StringBuilder("");
        while (cond(_char))
        {
            builder.Append(_char);
            Next();
        }
        return builder.ToString();
    }

    private Token TokenizeComment()
    {
        var position = GetPosition();
        Next(); // Skip ;
        var value = AdvanceWhile(LexerRules.IsCommentChar);
        return new Token(value, TokenKind.Comment, position);
    }

    private Token TokenizeIndent()
    {
        var position = GetPosition();
        var value = AdvanceWhile(Char.IsWhiteSpace);
        return new Token(value, TokenKind.Indent, position);
    }

    private Token TokenizeId()
    {
        var position = GetPosition();
        var value = AdvanceWhile(LexerRules.IsValidIdChar);
        var kind = TokenKind.Identifier;
        if (LexerRules.IsKeyword(value))
            kind = TokenKind.Keyword;
        
        return new Token(value, kind, position);
    }

    private Token TokenizeNumber()
    {
        var position = GetPosition();
        var integralPart = AdvanceWhile(Char.IsDigit);
        if (_char == '.')
        {
            Next();
            if (Char.IsDigit(_char))
            {
                var decimalPart = AdvanceWhile(Char.IsDigit);
                var value = $"{integralPart}.{decimalPart}";
                return new Token(value, TokenKind.FloatPoint, position);
            }
            return new Token($"{integralPart}.", TokenKind.Invalid, position);
        }
        return new Token(integralPart, TokenKind.Integer, position);
    }

    private Token TokenizeBool()
    {
        var position = GetPosition();
        Next(); // Skip #
        var value = _char.ToString();
        Next();
        return new Token(value, TokenKind.Boolean, position);
    }

    // TODO: Implement char tokenizer
    private Token TokenizeChar()
    {
        throw new NotImplementedException();
    }

    // TODO: Implement string tokenizer
    private Token TokenizeString()
    {
        throw new NotImplementedException();
    }

    private Token TokenizeSpecialChar()
    {
        var value = _char.ToString();
        var position = _column;
        var kind = TokenKind.Invalid;
        switch (_char)
        {
            case '.':  kind = TokenKind.Dot;        break;
            case '\'': kind = TokenKind.Quote;      break;
            case '(':  kind = TokenKind.LeftParen;  break;
            case ')':  kind = TokenKind.RightParen; break;
            case '{':  kind = TokenKind.LeftBrace;  break;
            case '}':  kind = TokenKind.RightBrace; break;
            case '[':  kind = TokenKind.LeftBrack;  break;
            case ']':  kind = TokenKind.RightBrace; break;
            case '\0': kind = TokenKind.EndOfFile;  break;
        }

        Next();
        return new Token(value, kind, GetPosition());
    }

    private Token NextToken() {
        if (Char.IsWhiteSpace(_char))
            return TokenizeIndent();
        else if (_char == ';')
            return TokenizeComment();
        else if (LexerRules.IsValidIdFistChar(_char))
            return TokenizeId();
        else if (Char.IsDigit(_char))
            return TokenizeNumber();
        else if (_char == '#')
            return TokenizeBool();
        else if (_char == '"')
            return TokenizeString();
        else if (_char == '$')
            return TokenizeChar();
        else
            return TokenizeSpecialChar();
    }

    public bool MoveNext()
    {
        if (_current.Kind == TokenKind.EndOfFile)
            return false;
        _current = NextToken();
        return true;
    }

    public void Reset() => throw new NotImplementedException();

    public void Dispose() { }
}