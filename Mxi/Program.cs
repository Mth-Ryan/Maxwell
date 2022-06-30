using Maxwell.Mxi.Repls;

namespace Maxwell.Mxi;

class Program
{
    static void Main(String[] args)
    {
        var repl = new EditorRepl();
        repl.EventLoop();
    }
}
