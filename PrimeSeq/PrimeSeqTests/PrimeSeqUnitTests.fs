module PrimeSeqTests

open NUnit.Framework
open PrimeSeqImpl
open FsUnit

[<TestFixture>]
type PrimeTests () =
    
    static member TestData = 
        [|
            (10, [2; 3; 5; 7; 11; 13; 17; 19; 23; 29])
            (2, [2; 3])
            (1, [2])
        |]
        
        
    [<TestCaseSource("TestData")>]
    member this.primeTests (testData: int * int list) =
        let n, result = testData
        primeSeq n |> Seq.toList |> should equal result
        
    [<Test>]
    member this.failedTest () =
        (fun () -> primeSeq -3 |> ignore) |>  should (throwWithMessage "Expected non-negative n") typeof<System.ArgumentException>
        
    [<Test>]
    member this.isPrimeShouldWorkCorrectly () =
        isPrime 2 |> should equal true
        isPrime 3 |> should equal true
        isPrime 1 |> should equal false
        isPrime 4 |> should equal false
        isPrime 0 |> should equal false
