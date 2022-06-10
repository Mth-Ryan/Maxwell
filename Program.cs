using Maxwell.Syntax;

namespace Maxwell;

class Program
{
    static void Main(string[] args) 
    {
        var source =
 @";; Comment
(namespace Maxwell.Example)

(using System)

(define (sum-12 x y)
    (+ x y 12))

12.0 ;; Float
'symbol ;; Symbol
#t ;;  Boolean";

        var lexer = new Lexer(source);
        
        LexerPrettyPrint.Print(lexer);
    }
}
