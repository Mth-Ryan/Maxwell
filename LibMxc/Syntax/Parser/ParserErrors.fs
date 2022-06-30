namespace Maxwell.LibMxc.Syntax.Parser

open Combinators

module ParserErrors =

    type MaxwellParserError =
        | InvalidInt
        interface IParserErrorKind