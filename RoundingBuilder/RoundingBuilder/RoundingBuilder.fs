module RoundingBuilder

open System

/// Workflow for counting with given accuracy 
type RounderBuilder (digits: int) =
    member this.Bind(x, func) = func x
    member this.Return(x: float) = Math.Round(x, digits)