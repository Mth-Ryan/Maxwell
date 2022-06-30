namespace Maxwell.LibMxc.Syntax.Parser

open Maxwell.LibMxc.Syntax.Rules.SyntaxPatterns
open Combinators
open GenericParsers
open MaxwellParser

module ReplParser =
    type ReplToken =
        | Literal    = 0
        | Keyword    = 1
        | Id         = 2
        | Delimiter  = 3
        | WhiteSpace = 4
        | NewLine    = 5
        | Quote      = 6
        | Normal     = 7


    let literal =
        fun s -> (s, ReplToken.Literal) 
        <&> (boolStringP <|> floatStringP   <|> integerStringP  <|> symbolStringP)

    let keyword =
        let parser =
            regexP definePattern      <|>
            regexP lambdaPattern      <|>
            regexP lambdaShortPattern <|>
            regexP ifPattern          <|>
            regexP doPattern  
        fun s -> (s, ReplToken.Keyword)
        <&> parser

    let id = (fun s -> (s, ReplToken.Id)) <&> idStringP

    let delimiters = 
        fun s -> (s, ReplToken.Delimiter)
        <&> (regexP @"\(" <|> regexP @"\)")

    let whitespaces =
        fun xs -> (xs, ReplToken.WhiteSpace)
        <&> regexP $"({whitespacePattern})+"

    let newlines =
        fun xs -> (xs, ReplToken.NewLine)
        <&> newlineP

    let quote =
        fun s -> (s, ReplToken.Quote)
        <&> quoteP

    let normal =
        fun s -> (s, ReplToken.Normal)
        <&> (regexP @"(?s).*")

    let parser =
        let element =
            whitespaces <|>
            newlines    <|>
            delimiters  <|>
            literal     <|>
            keyword     <|>
            id          <|>
            quote       <|>
            normal
        (fun xs -> List.toArray xs) <&> (many element)

    let parse str =
        match runParser parser (createInput str) with
        | Ok (value, _) -> value
        | _ -> failwith "[Repl parser] Unreachable"