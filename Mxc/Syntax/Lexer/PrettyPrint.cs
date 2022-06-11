using System.Text;

namespace Maxwell.Mxc.Syntax;

public static class LexerPrettyPrint
{
    public static String GetString(Lexer lex, bool filterIndent = true)
    {
        var builder = new StringBuilder("");
        IEnumerable<Token> tokens = lex;
        if (filterIndent)
            tokens = lex.Where(x => x.Kind != TokenKind.Indent);

        int indent = 0;
        foreach (var token in tokens)
        {
            if (token.IsClosePair())
                indent = indent > 1 ? indent - 1 : indent;

            builder.Append(new String(' ', 2 * indent) + token.ToString() + "\n");

            if (token.IsOpenPair())
                indent++;
        }
        return builder.ToString();
    }
    public static void Print(Lexer lex, bool filterIndent = true)
    {
        var content = GetString(lex, filterIndent);
        Console.WriteLine(content);
    }
}