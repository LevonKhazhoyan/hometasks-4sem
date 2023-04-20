module StringCalc

open System

/// Workflow for counting with numbers given as a string
type StringCalculatorBuilder () =
    member this.Bind(x: string, func) = 
        try 
            x |> int |> func
        with :? FormatException
            -> None
    member this.Return(x) = x |> Some