namespace Maxwell.Mxi.Repls;

using Maxwell.LibMxc.Syntax.Parser;
public sealed class EditorRepl : Repl
{
    public override string Read()
    {
        var defaultForeground = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("λ ");
        Console.ForegroundColor = defaultForeground;
        var input = Console.ReadLine();
        if (input == null)
            return "";

        var tokens = ReplParser.parse(input!);
        foreach (var (token, kind) in tokens)
        {
            var color = Console.ForegroundColor;
            switch (kind)
            {
                case ReplParser.ReplToken.Keyword:
                color = ConsoleColor.Magenta;
                break;

                case ReplParser.ReplToken.Literal:
                color = ConsoleColor.DarkYellow;
                break;
                
                case ReplParser.ReplToken.Id:
                color = ConsoleColor.Gray;
                break;

                default: break;
            }
            Console.ForegroundColor = color;
            Console.Write(token);
            Console.ForegroundColor = defaultForeground;
        }

        Console.WriteLine("");
        return "res --";
    }
}