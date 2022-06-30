namespace Maxwell.LibMxc.Syntax.Rules

module SyntaxPatterns =
    let newlinePattern = "\r\n|\r|\n"
    
    let whitespacePattern = "(\r\n|\r|\n| |\t)+"

    let integerPattern = @"[+-]?\d+"

    let floatPattern = @"[+-]?\d+[.]\d+"
    
    let idPattern = @"[a-zA-Z_*/+-<>=!?][a-zA-Z0-9_*/+-<>=!?]*"

    let booleanPattern = @"#t|#f"

    let definePattern = @"def"

    let lambdaPattern = @"lambda|λ"

    let ifPattern = @"if"
    
    let doPattern = @"do"

    let keywordPattern = @"def|lambda|λ|if|do"