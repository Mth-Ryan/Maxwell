namespace Maxwell.LibMxc.Syntax.Parser

open System

module ParserUtils =
    let parseBool str =
        match str with
        | "#t" -> true
        | "#f" -> false
        | _ -> failwith "[toBool] Unreachable"

    let parseInt (str : string) =
        match Int32.TryParse str with
        | true, n -> Some n
        | _ -> None

    let parseFloat (str : string) = Double.Parse str