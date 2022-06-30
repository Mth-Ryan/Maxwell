namespace Maxwell.LibMxc.Syntax.Parser

open System.Text.RegularExpressions

open Maxwell.LibMxc.Syntax.Rules.SyntaxPatterns
open Combinators

module GenericParsers =
    type GenericParserError =
        | UnexpectedEOF
        | UnexpectedMatch of string
        interface IParserErrorKind

    let private next value (line, col) =
        let linesStr = Regex.Split(value, newlinePattern)
        match linesStr with
        | [||] -> (line, col)
        | arr -> (line + (arr.Length - 1), col + arr.[arr.Length - 1].Length)

    let private split (value : string) (input : ParserInput) =
        let (str, pos) = input
        let nextPos = next value pos
        if value.Length < str.Length
        then (str[value.Length..], nextPos)
        else ("", nextPos)

    let regexP pattern =
        let f input =
            match input with
            | ("", pos) -> Err (UnexpectedEOF, ("", pos))
            | (str,  _) ->
                let matches = Regex.Match(str, "^" + pattern)
                if matches.Success
                then Ok (matches.Value, split matches.Value input)
                else Err (UnexpectedMatch pattern, input)
        Parser f

    let optionalP parser =
        let g input =
            match runParser parser input with
            | Ok (res, rest) -> Ok (Some res, rest)
            | Err (_, inp)   -> Ok (None, inp)
        Parser g

    
    let integerStringP = regexP integerPattern

    let floatStringP = regexP floatPattern

    let boolStringP = regexP booleanPattern

    let newlineP =
        (regexP newlineWindowsPattern) <|> (regexP newlinePattern)

    let whitespaceP = regexP whitespacePattern

    let indentP = many (whitespaceP <|> newlineP)

    let optIndentP = optionalP indentP

    let quoteP = regexP @"'" <<. optIndentP

    let leftParenP = optIndentP .>> regexP @"\(" <<. optIndentP

    let rightParenP = optIndentP .>> regexP @"\)" <<. optIndentP

    let betweenPar parser = leftParenP .>> parser <<. rightParenP