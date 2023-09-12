namespace Maxwell.LibMxc.Syntax;

public class Parser
{
    private readonly IEnumerable<Token> _tokens;

    public Parser(IEnumerable<Token> tokens)
    {
        _tokens = tokens;
    }
}
