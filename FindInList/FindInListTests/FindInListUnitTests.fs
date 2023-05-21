module FindInListTests

open NUnit.Framework
open FsUnit
open FindInListFunc

[<TestFixture>]
type findInListTests() =

    static member TestData = 
        [|
            (1, [1; 2; 3; 5], Some(0))
            (1, [1; 2; 1; 5], Some(0))
            (6, [1; 2; 4; 8; 16; 32], None)
            (32, [1; 2; 4; 8; 16; 32], Some(5))
            (-6, [-8; 2; -6; 8; 16; 32], Some(2))
        |]

    [<TestCaseSource("TestData")>]
    member this.successfulTests (testData: int * int list * Option<int>) =
        let key, lst, result = testData
        (find key lst) |> should equal result