namespace Maxwell.Syntax;

public static class LexerPrettyPrint
{
    public static void Print(Lexer lex, bool filterIndent = true)
    {
        IEnumerable<Token> tokens = lex;
        if (filterIndent)
            tokens = lex.Where(x => x.Kind != TokenKind.Indent);

        int indent = 0;
        foreach (var token in tokens)
        {
            if (token.IsClosePair())
                indent = indent > 1 ? indent - 1 : indent;

            Console.WriteLine(new String(' ', 2 * indent) + token.ToString());

            if (token.IsOpenPair())
                indent++;
        }
    }
}