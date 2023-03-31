module BalanceChecking

let checkBalance string =
    let rec isBalanced string round square figure =
        match string with
        | [] -> round = 0 && square = 0 && figure = 0
        | head::tail -> 
            match head with
            | '(' -> isBalanced tail (round + 1) square figure
            | '[' -> isBalanced tail round (square + 1) figure
            | '{' -> isBalanced tail round square (figure + 1)
            | ')' -> isBalanced tail (round - 1) square figure
            | ']' -> isBalanced tail round (square - 1) figure
            | '}' -> isBalanced tail round square (figure - 1)
            | _ -> false 
    isBalanced string 0 0 0