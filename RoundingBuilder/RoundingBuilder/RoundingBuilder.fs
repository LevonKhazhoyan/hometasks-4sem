module RoundingBuilder

open System

/// Workflow for counting with given accuracy 
type RounderBuilder (digits: int) =
    member this.Bind(x: float, func) = 
        func <| Math.Round(x, digits)
    member this.Return(x: float) = Math.Round(x, digits)