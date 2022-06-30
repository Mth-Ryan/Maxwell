namespace Maxwell.LibMxc.Syntax.Rules

module SyntaxPatterns =
    let newlinePattern = "\r|\n"
    
    let newlineWindowsPattern = "\r\n"
    
    let whitespacePattern = " |\t"

    let integerPattern = @"[+-]?\d+"

    let floatPattern = @"[+-]?\d+[.]\d+"
    
    let idPattern = @"[a-zA-Z_*/+-<>=!?][a-zA-Z0-9_*/+-<>=!?]*"

    let booleanPattern = @"#t|#f"

    let definePattern = @"def"

    let lambdaPattern = @"lambda"

    let lambdaShortPattern = @"λ"

    let ifPattern = @"if"
    
    let doPattern = @"do"