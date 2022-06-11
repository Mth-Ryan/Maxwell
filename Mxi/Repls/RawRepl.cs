namespace Maxwell.Mxi.Repls;

public sealed class RawRepl : Repl
{
    public override string Read()
    {
        var defaultForeground = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("λ ");
        Console.ForegroundColor = defaultForeground;
        var input = Console.ReadLine();
        if (input != null)
            return input!;
        return "";
    }
}