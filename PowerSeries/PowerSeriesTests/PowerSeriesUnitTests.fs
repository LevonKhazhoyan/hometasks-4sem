module PowerSeriesTests

open FsUnit
open NUnit.Framework
open PowerSeriesFunc

[<TestFixture>]
type reverseTests() =

    static member TestData = 
        [|
            (0, 5, [1; 2; 4; 8; 16; 32])
            (3, 4, [8; 16; 32; 64; 128])
            (1, 3, [2; 4; 8; 16])
            (5, 2, [32; 64; 128])
            (10, 0, [1024])
        |]

    [<TestCaseSource("TestData")>]
    member this.successfulTests (testData: int * int * int list) =
        let n, m, result = testData
        listOfPowerSeries n m |> should equal result