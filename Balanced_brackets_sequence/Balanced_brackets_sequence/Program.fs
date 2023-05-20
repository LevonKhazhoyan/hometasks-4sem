module BalanceChecking

open System.Collections.Generic

let checkBalance (input: string) =
    let isOpeningBracket (c: char) =
        c = '(' || c = '[' || c = '{'

    let isClosingBracket (c: char) =
        c = ')' || c = ']' || c = '}'

    let getClosingBracket (c: char) =
        match c with
        | '(' -> ')'
        | '[' -> ']'
        | '{' -> '}'
        | _ -> failwith "Invalid opening bracket"

    let rec isBalanced (stack: Stack<_>) remaining =
        match remaining with
        | [] ->
            match stack.TryPeek() with
                | false, _ -> true
                | true, _ -> false
        | head :: tail when isOpeningBracket head ->
            // Push the opening bracket onto the stack
            stack.Push head 
            isBalanced stack tail
        | head :: tail when isClosingBracket head ->
            match stack.TryPeek() with
            | true, top when getClosingBracket top = head ->
                // Matched closing bracket, pop from stack
                stack.Pop() |> ignore 
                isBalanced stack tail
            | _ ->
                false
        | _ :: tail ->
            // Skip non-bracket characters
            isBalanced stack tail

    isBalanced (Stack ()) (List.ofSeq input)