namespace Syntax;

public enum TokenKind
{
    Identifier,  // [a-zA-Z+-/*?!<>=][a-zA-Z0-9+-/*?!<>=]+
    Keyword,     // define, if, cond, ...

    // Literals
    Integer,     // 13
    FloatPoint,  // 13.5
    Char,        // $a
    String,      // "String"
    Boolean,     // #t, #f

    // Specific Syntax chars
    LeftParen,   // (
    RightParen,  // )
    LeftBrace,   // {
    RightBrace,  // }
    LeftBrack,   // [
    RightBrack,  // ]
    Quote,       // '

    Comment,    // ;; Comment
    Indent,
    EndOfLine,
    Invalid,
    EndOfFile
}