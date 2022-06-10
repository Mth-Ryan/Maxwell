namespace Maxwell.Mxc.Syntax;

static class LexerRules
{
    public static String NewLinePatter => "\r\n|\r|\n";
    public static bool IsKeyword(String src) 
    {
        // Array.IndexOf is a efficient search for small arrays.
        String[] keywords = {
            "namespace",
            "using",
            "define",
            "define-syntax",
            "let",
            "lambda",
            "if",
            "cond"
        };
        return Array.IndexOf(keywords, src) != -1;
    }

    public static bool IsValidIdFistChar(char src)
    {
        char[] chars = {
            '!', '?', '+', '-', '*', '/', '<', '>', '='
        };
        return Array.IndexOf(chars, src) != -1 || Char.IsLetter(src);
    }

    public static bool IsValidIdChar(char src)
    {
        return IsValidIdFistChar(src) || Char.IsDigit(src);
    }

    public static bool IsCommentChar(char src) => src != '\n';
}