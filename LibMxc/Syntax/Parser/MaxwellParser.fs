namespace Maxwell.LibMxc.Syntax.Parser

open Maxwell.LibMxc.Syntax.Rules.SyntaxPatterns
open Maxwell.LibMxc.Syntax.Ast
open ParserUtils
open Combinators
open GenericParsers
open ParserErrors

#nowarn "40"
module MaxwellParser = 
    // String parsers
    let idStringP = regexP idPattern

    let symbolStringP = (quoteP .>> idStringP)

    let defineP = (regexP definePattern) <<. indentP

    let lambdaP = ((regexP lambdaPattern) <|> (regexP lambdaShortPattern))
                  <<. indentP

    let ifP = (regexP ifPattern) <<. indentP

    let doP = (regexP doPattern) <<. indentP

    // Maxwell Literals
    let maxSymbolP = (fun v -> MaxSymbol (v, false)) <&> idStringP

    let maxQuotedSymbolP = (fun v -> MaxSymbol (v, true)) <&> symbolStringP

    let maxBoolP = (fun b -> MaxBool (parseBool b)) <&> boolStringP

    let private parseMaxInt (str, input, rest) =
        match parseInt str with
        | Some n -> Ok (MaxInt n, rest)
        | None -> Err (InvalidInt, input)

    let maxIntP = pmap parseMaxInt Err integerStringP

    let maxFloatP = (fun n -> MaxFloat (parseFloat n)) <&> floatStringP

    let rec maxListElementP =
        maxQuotedSymbolP <|>
        maxBoolP         <|>
        maxFloatP        <|>
        maxIntP          <|>
        maxSymbolP       <!>
        lazy(maxListP)   <!>
        lazy(maxQuotedListP)
    and maxListBodyP = 
       betweenPar (sepBy maxListElementP optIndentP)
    and maxListP =
        fun xs -> MaxList ((List.toArray xs), false)
        <&> maxListBodyP
    and maxQuotedListP = 
        fun xs -> MaxList ((List.toArray xs), true)
        <&> (quoteP .>> maxListBodyP)

    // Maxwell Expressions
    let maxLiteralP =
        MaxLit            <&>
        (between 
        (maxQuotedSymbolP <|>
        maxBoolP          <|>
        maxFloatP         <|>
        maxIntP           <!>
        lazy(maxQuotedListP))
        optIndentP)

    let maxIdP = MaxId <&> between idStringP optIndentP
    
    let rec maxExprP =
        maxLiteralP       <|>
        maxIdP            <!>
        lazy(maxDefP)     <!>
        lazy(maxLambdaP)  <!>
        lazy(maxIfP)      <!>
        lazy(maxDoP)      <!>
        lazy(maxProcP)
    and maxProcP =
        fun (id, args) -> MaxProc (id, List.toArray args)
        <&> (betweenPar (maxIdP <.> (many maxExprP)))
    and maxDefP =
        MaxDef 
        <&> (betweenPar (defineP .>> (maxIdP <<. indentP) <.> maxExprP))
    and maxLambdaP =
        fun (args, body) -> MaxLambda (List.toArray args, body)
        <&> (betweenPar (lambdaP .>>
            (betweenPar (sepBy maxIdP indentP)) <.> maxExprP))
    and maxIfP =
        MaxIf <&> 
        (betweenPar (ifP .>> (maxProcP) <.> (maxExprP <.> maxExprP)))
    and maxDoP =
        fun xs -> MaxDo (List.toArray xs) 
        <&> ((betweenPar (doP .>> (many maxExprP))) <|>
             (betweenPar ((regexP doPattern) .>> pureA [])))
             
