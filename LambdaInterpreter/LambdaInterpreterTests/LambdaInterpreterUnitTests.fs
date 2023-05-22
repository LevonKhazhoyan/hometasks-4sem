module LambdaInterpreter.Tests

open NUnit.Framework
open FsUnit
open LambdaInterpreterImpl
open NUnit.Framework.Internal

[<TestFixture>]
type PrimeTests () =

    static member TestData = 
        [|
           (LambdaApplication(LambdaAbstraction("x", Variable "x"), Variable "y"), Variable("y"))
           (LambdaApplication(LambdaAbstraction("x", Variable "x"), Variable "z"), Variable "z")
           (LambdaApplication(LambdaAbstraction("x", Variable "z"), Variable "x"), Variable "z")
           (LambdaApplication(LambdaAbstraction("x", LambdaApplication(Variable("x"), Variable("y"))),
                    LambdaAbstraction("z", Variable("z"))), Variable("y"))
           (LambdaApplication(
                   LambdaAbstraction("x", Variable "y"),
                   LambdaApplication(
                       LambdaAbstraction("x", LambdaApplication(Variable "x", LambdaApplication(Variable "x", Variable "x"))),
                       LambdaAbstraction("x", LambdaApplication(Variable "x", LambdaApplication(Variable "x", Variable "x")))
                   )
               ), Variable "y")
           (LambdaApplication(LambdaAbstraction("a", LambdaAbstraction("x", Variable "a")), Variable "x"),LambdaAbstraction("x'", Variable "x"))
        |]
        
    [<TestCaseSource("TestData")>]
    member this.lambdaInterpreterDDTs (testData: LambdaTerm * LambdaTerm) =
        let term, result = testData
        reduce term |> should equal result