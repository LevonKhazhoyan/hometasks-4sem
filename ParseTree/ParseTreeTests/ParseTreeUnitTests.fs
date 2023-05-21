module ParseTreeTests

open NUnit.Framework
open ParseTreeImpl
open FsUnit


[<TestFixture>]
type parseTreeTests() =
    
    [<Test>]
    member this.``Evaluate should return the value of a single Value operation`` () =
        let operation = Value 5
        operation.evaluate |> should equal 5
    
    [<Test>]
    member this.evaluateSubtract () =
        let operation = Subtract (Value 10, Value 3)
        operation.evaluate |> should equal 7
    
    [<Test>]
    member this.evaluateSum () =
        let operation = Sum (Value 7, Value 4)
        let result = operation.evaluate
        Assert.AreEqual(11, result)
    
    [<Test>]
    member this.evaluateMultiply () =
        let operation = Multiply (Value 6, Value 3)
        operation.evaluate |> should equal 18
    
    [<Test>]
    member this.evaluateDivide () =
        let operation = Divide (Value 15, Value 5)
        operation.evaluate |> should equal 3
    
    [<Test>]
    member this.evaluateHandle () =
        let operation = Subtract (Sum (Value 10, Multiply (Value 2, Value 4)), Divide (Value 20, Value 5))
        operation.evaluate |> should equal 14