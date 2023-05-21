module FactorialTests

open FactorialFunc
open NUnit.Framework
open FsUnit

[<TestFixture>]
type facTests() =

    static member TestData = 
        [|
            (0, 1)
            (1, 1)
            (2, 2)
            (3, 6)
            (5, 120)
            (11, 39916800)
        |]

    [<TestCaseSource("TestData")>]
    member this.successfulTests (testData: int * int) =
        let n, result = testData
        factorial n |> should equal result 

    [<Test>]
    member this.failedTest () =
        (fun () -> factorial -3 |> ignore) |>  should (throwWithMessage "Expected non-negative n") typeof<System.Exception>