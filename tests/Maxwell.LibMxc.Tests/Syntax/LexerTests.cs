using Maxwell.LibMxc.Syntax;

namespace Maxwell.LibMxc.Tests;

public class LexerTests
{
    [Fact]
    public void Lexer_WithEmptyInput_Should_Return_EOF()
    {
        var src = "";
        var lex = new Lexer(src);
        var tokenList = lex.ToList();

        tokenList.Select(t => t.Kind).Should().NotBeEmpty()
            .And.HaveCount(1)
            .And.BeEquivalentTo(new[] { TokenKind.EOF });
    }

    [Fact]
    public void Lexer_Should_Tokenize_Whitespaces()
    {
        var src = "    ";
        var lex = new Lexer(src);
        var tokenList = lex.ToList();

        tokenList.Select(t => (t.Value, t.Kind)).Should().HaveCount(2)
            .And.ContainInOrder(new[]
            {
                (src, TokenKind.WhiteSpace),
                ("\u0000", TokenKind.EOF)
            });
    }

    [Fact]
    public void Lexer_Should_Tokenize_Identifiers()
    {
        var src = new List<string> { "something", "somethingCompose123" };
        var lex = new Lexer(String.Join(" ", src));
        var tokenList = lex.Where(t => t.Kind != TokenKind.WhiteSpace).ToList();

        tokenList.Select(t => (t.Value, t.Kind)).Should().HaveCount(3)
            .And.ContainInOrder(new[] {
                (src[0], TokenKind.Identifier),
                (src[1], TokenKind.Identifier),
                ("\u0000", TokenKind.EOF)
            });
    }

    [Fact]
    public void Lexer_Should_Tokenize_Keywords()
    {
        var src = new List<string> { "def", "lambda", "if", "cond" };
        var lex = new Lexer(String.Join(" ", src));
        var tokenList = lex.Where(t => t.Kind != TokenKind.WhiteSpace).ToList();

        tokenList.Select(t => (t.Value, t.Kind)).Should().HaveCount(5)
            .And.ContainInOrder(new[] {
                (src[0], TokenKind.Def),
                (src[1], TokenKind.Lambda),
                (src[2], TokenKind.If),
                (src[3], TokenKind.Cond),
                ("\u0000", TokenKind.EOF)
            });
    }

    [Fact]
    public void Lexer_Should_Tokenize_Numbers()
    {
        var src = new List<string> { "123", "12.33" };
        var lex = new Lexer(String.Join(" ", src));
        var tokenList = lex.Where(t => t.Kind != TokenKind.WhiteSpace).ToList();

        tokenList.Select(t => (t.Value, t.Kind)).Should().HaveCount(3)
            .And.ContainInOrder(new[] {
                (src[0], TokenKind.Int),
                (src[1], TokenKind.Float),
                ("\u0000", TokenKind.EOF)
            });
    }

    [Fact]
    public void Lexer_Should_Tokenize_Strings()
    {
        var src = "\"Something similar\"";
        var lex = new Lexer(src);
        var tokenList = lex.Where(t => t.Kind != TokenKind.WhiteSpace).ToList();

        tokenList.Select(t => (t.Value, t.Kind)).Should().HaveCount(2)
            .And.ContainInOrder(new[] {
                ("Something similar", TokenKind.String),
                ("\u0000", TokenKind.EOF)
            });
    }

    [Fact]
    public void Lexer_Should_Tokenize_SingleChars()
    {
        var src = "()[]{}'";
        var lex = new Lexer(src);
        var tokenList = lex.Where(t => t.Kind != TokenKind.WhiteSpace).ToList();

        tokenList.Select(t => (t.Value, t.Kind)).Should().HaveCount(8)
            .And.ContainInOrder(new[] {
                ("(", TokenKind.LeftParen),
                (")", TokenKind.RightParen),
                ("[", TokenKind.LeftBrack),
                ("]", TokenKind.RightBrack),
                ("{", TokenKind.LeftBrace),
                ("}", TokenKind.RightBrace),
                ("'", TokenKind.Quote),
                ("\u0000", TokenKind.EOF)
            });
    }

    [Fact]
    public void Lexer_Should_Tokenize_Call()
    {
        var src = "(sumAll 1 2 3)";
        var lex = new Lexer(src);
        var tokenList = lex.Where(t => t.Kind != TokenKind.WhiteSpace).ToList();

        tokenList.Select(t => (t.Value, t.Kind)).Should().HaveCount(7)
            .And.ContainInOrder(new[] {
                ("(", TokenKind.LeftParen),
                ("sumAll", TokenKind.Identifier),
                ("1", TokenKind.Int),
                ("2", TokenKind.Int),
                ("3", TokenKind.Int),
                (")", TokenKind.RightParen),
                ("\u0000", TokenKind.EOF)
            });
    }

    [Fact]
    public void Lexer_Should_Tokenize_Definition()
    {
        var src = "(def sum1 [n] (sum n 1))";
        var lex = new Lexer(src);
        var tokenList = lex.Where(t => t.Kind != TokenKind.WhiteSpace).ToList();

        tokenList.Select(t => (t.Value, t.Kind)).Should().HaveCount(13)
            .And.ContainInOrder(new[] {
                ("(", TokenKind.LeftParen),
                ("def", TokenKind.Def),
                ("sum1", TokenKind.Identifier),
                ("[", TokenKind.LeftBrack),
                ("n", TokenKind.Identifier),
                ("]", TokenKind.RightBrack),
                ("(", TokenKind.LeftParen),
                ("sum", TokenKind.Identifier),
                ("n", TokenKind.Identifier),
                ("1", TokenKind.Int),
                (")", TokenKind.RightParen),
                (")", TokenKind.RightParen),
                ("\u0000", TokenKind.EOF)
            });
    }

    [Fact]
    public void Lexer_Should_Tokenize_Definition_Positions()
    {
        var src = "(def sum1 [n] (sum n 1))";
        var lex = new Lexer(src);
        var tokenList = lex.Where(t => t.Kind != TokenKind.WhiteSpace).ToList();

        tokenList.Select(t => t.TextPosition).Should().HaveCount(13)
            .And.ContainInOrder(new uint[] { 0, 1, 5, 10, 11, 12, 14, 15, 19, 21, 22, 23, 24 });
    }
}
