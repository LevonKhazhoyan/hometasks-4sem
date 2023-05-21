module ParseTreeImpl

type Operation =
    | Value of int
    | Subtract of Operation * Operation
    | Sum of Operation * Operation
    | Multiply of Operation * Operation
    | Divide of Operation * Operation
    
    member this.evaluate =
        match this with
        | Value value -> value
        | Subtract (left, right) -> left.evaluate - right.evaluate
        | Sum (left, right) -> left.evaluate + right.evaluate
        | Multiply (left, right) -> left.evaluate * right.evaluate
        | Divide (left, right) -> left.evaluate / right.evaluate