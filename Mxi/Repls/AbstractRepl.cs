using Maxwell.Mxi.Info;
namespace Maxwell.Mxi.Repls;

public abstract class Repl
{
    // TODO Return a String.
    public void Header()
    {
        var version = ProgramInfo.Version;
        var dotnetVersion = SystemInfo.DotnetVersion;
        var os = SystemInfo.OsName;

        Console.WriteLine($"\nMaxwell Interactive {version}");
        Console.WriteLine($".Net {dotnetVersion} on {os}");
        Console.WriteLine("Type ':help' for more information or ':exit' to close.\n");
    }
    public abstract String Read();

    public ReplResult Eval(String input)
    {
        return new ReplResult(
            input,
            ReplResultType.Result,
            ReplEvent.None);
    }

    public void Print(ReplResult result)
    {
        switch (result.Type)
        {
            case ReplResultType.Result:
                Console.ForegroundColor = ConsoleColor.Cyan;
                break;

            case ReplResultType.Error:
                Console.ForegroundColor = ConsoleColor.Red;
                break;
        }
        Console.WriteLine(result.Value);
        Console.ResetColor();
    }

    public void EventLoop()
    {
        Header();
        while (true)
        {
            var input = Read();
            var result = Eval(input);
            switch (result.Event)
            {
                case ReplEvent.None:
                    break;
                case ReplEvent.Continue:
                    continue;
                case ReplEvent.Exit:
                    goto Exit;
            }
            Print(result);  
        }
        Exit:
            Console.WriteLine("Exit requested...");
    }
}