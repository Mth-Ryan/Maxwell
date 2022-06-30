namespace Maxwell.MxcLib.Syntax.Parser

open System

module Combinators =
    type ParserInput = string * (int * int)

    let createInput str = str, (1, 1)

    type IParserErrorKind = interface end

    type InternalParserError = 
        | UnexpectedInput of string
        interface IParserErrorKind

    type ParserResult<'a> = 
        | Ok  of 'a * ParserInput
        | Err of IParserErrorKind * ParserInput

    type Parser<'a> = Parser of (ParserInput -> ParserResult<'a>)

    let runParser (Parser p) input = p input

    let pmap f g (Parser p) =
        let inner input =
            match p input with
            | Err (err, inp) -> g (err, inp)
            | Ok (x, rest)   -> f (x, input, rest)
        Parser inner

    let fmap f parser = 
        pmap (fun (x, _, rest) -> Ok (f x, rest)) Err parser

    let (<&>) = fmap

    let emap f parser = 
        pmap (fun (x, _, rest) -> Ok (x, rest))
             (fun (err, input) -> Err (f err, input))
             parser

    let private constF x _ = x

    let (<&) a b = (fmap << constF) a b

    let pureA x = Parser (fun input -> Ok (x, input))

    let (<*>) (Parser p1) (Parser p2) =
        let g input =
            match p1 input with
            | Err (err1, inp1) -> Err (err1, inp1)
            | Ok (f, rest1) ->
                match p2 rest1 with
                | Err (err2, inp2) -> Err (err2, inp2)
                | Ok (x, rest2) -> Ok (f x, rest2)
        Parser g

    let liftA2 f x y = f <&> x <*> y

    let (.>>) x y = (id <& x) <*> y

    let (<<.) x y = liftA2 constF x y

    let empty = Parser (fun (input, pos) -> 
        Err (UnexpectedInput input, (input, pos)))

    let (<|>) (Parser p1) (Parser p2) =
        let g input =
            match p1 input with
            | Ok (x, rest1) -> Ok (x, rest1)
            | Err _ ->
                match p2 input with
                | Ok (y, rest2) -> Ok (y, rest2)
                | Err (err, inp) -> Err (err, inp)
        Parser g

    let (<!>) p1 (p2 : Lazy<Parser<'a>>) =
        let g input =
            match runParser p1 input with
            | Ok (x, rest1) -> Ok (x, rest1)
            | Err _ ->
                let p = p2.Force()
                match  runParser p input with
                | Ok (y, rest2) -> Ok (y, rest2)
                | Err (err, inp) -> Err (err, inp)
        Parser g

    let rec private manyResult parser input =
        let first = runParser parser input
        match first with
        | Err _ -> ([], input)
        | Ok (h, rest1) ->
            let (t, rest2) = manyResult parser rest1
            (h::t, rest2)

    let many parser =
       Parser (fun input -> Ok (manyResult parser input))

    let private cons x y = x :: y

    let some parser =
        liftA2 cons parser (many parser)

    let sepBy parser sep =
        liftA2 cons parser (many (sep .>> parser))
        <|> pureA []

    let between parser sep = sep .>> parser <<. sep

    // Monad
    let returnM = pureA

    let (>>=) (Parser p) f =
        let g input =
            match p input with
            | Err (err, inp) -> Err (err, inp)
            | Ok (x, rest) ->
                let (Parser p1) = f x
                p1 rest
        Parser g

    // Extra Operations
    let private tuple x y = (x, y)

    let (<.>) p1 p2 = liftA2 tuple p1 p2

    let (<..>) p1 (p2 : Lazy<Parser<'b>>) =
        let g input =
            match runParser p1 input with
            | Err (err, inp) -> Err(err, inp)
            | Ok (x, rest1) ->
                let p = p2.Force()
                match runParser p rest1 with
                | Err (err, inp) -> Err (err, inp)
                | Ok (y, rest2) -> Ok ((x, y), rest2)
        Parser g
