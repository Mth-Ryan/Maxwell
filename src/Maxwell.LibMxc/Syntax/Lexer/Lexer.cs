using System.Collections;

namespace Maxwell.LibMxc.Syntax;

public class Lexer : IEnumerable<Token>
{
    private LexerEnumerator _enumerator;

    public Lexer(string src)
    {
        _enumerator = new LexerEnumerator(src);
    }

    public IEnumerator<Token> GetEnumerator() => _enumerator;

    IEnumerator IEnumerable.GetEnumerator() => _enumerator;
}
