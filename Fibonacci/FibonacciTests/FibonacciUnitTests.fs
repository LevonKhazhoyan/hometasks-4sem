module FibonacciTests

open FibonacciFunc
open NUnit.Framework
open FsUnit

[<TestFixture>]
type fibTests() =
    
    static member TestData = 
        [|
            (3, [0;1;1;2])
            (5, [0;1;1;2;3;5])
            (13, [0;1;1;2;3;5;8;13;21;34;55;89;144;233])
            (9, [0;1;1;2;3;5;8;13;21;34])
        |]
        
    [<TestCaseSource("TestData")>]
    member this.successfulTests (testData: int * int list) =
        let n, result = testData
        fibonacci n |> should equal result 
        
    [<Test>]
    member this.failedTest () =
        (fun () -> fibonacci -3 |> ignore) |>  should (throwWithMessage "Expected non-negative n") typeof<System.Exception>
        
