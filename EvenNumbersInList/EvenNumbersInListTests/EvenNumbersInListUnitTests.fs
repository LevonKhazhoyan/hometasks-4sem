module EvenNumbersInListTests

open NUnit.Framework
open EvenNumbersInListFunc
open FsUnit

[<TestFixture>]
type evenNumbersInListTests() =

    static member TestData = 
        [|
            ([1; 2; 3; 5], 1)
            ([8; 2; 6; 4], 4)
            ([2; 2; 2; 2], 4)
            ([2; 1; 1; 2], 2)
            ([1; 3; 3; 5], 0)
            ([2; 2; 2; 0], 4)
            ([0; 0; 0; 0], 4)
            ([7; 7; 7; 7], 0)
        |]

    [<TestCaseSource("TestData")>]
    member this.successfulTestsForAllFunctions (testData: int list * int) =
        let lst, result = testData
        (evenNumbersInList lst) |> should equal result
        (evenNumbersInList' lst) |> should equal result
        (evenNumbersInList'' lst) |> should equal result

