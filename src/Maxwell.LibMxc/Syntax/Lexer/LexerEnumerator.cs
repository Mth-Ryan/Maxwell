using System.Collections;

namespace Maxwell.LibMxc.Syntax;

public partial class LexerEnumerator : IEnumerator<Token>
{
    private Token _current = new Token(TokenKind.Invalid, 0);

    public Token Current => _current;

    object IEnumerator.Current => _current;

    public void Dispose() { }

    public bool MoveNext()
    {
        if (_current.Kind != TokenKind.EOF)
        {
            _current = NextToken();
            return true;
        }
        return false;
    }

    public void Reset() => _pos = 0;
}
