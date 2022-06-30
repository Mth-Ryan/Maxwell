namespace Maxwell.LibMxc.Syntax

module Ast =
    type MaxLiteral =
        | MaxSymbol of string * bool
        | MaxInt    of int
        | MaxFloat  of float
        | MaxBool   of bool
        | MaxList   of array<MaxLiteral> * bool

    type MaxExpr =
        | MaxLit     of MaxLiteral
        | MaxId      of string
        | MaxProc    of MaxExpr * array<MaxExpr>      // id * args
        | MaxDef     of MaxExpr * MaxExpr             // id * value
        | MaxLambda  of array<MaxExpr> * MaxExpr      // args * body
        | MaxIf      of MaxExpr * (MaxExpr * MaxExpr) // cond * (if * else)
        | MaxDo      of array<MaxExpr>