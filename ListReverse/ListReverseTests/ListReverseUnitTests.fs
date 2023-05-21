module ListReverseTests

open NUnit.Framework
open ListReverseFunc
open FsUnit

[<TestFixture>]
type reverseTests() =

    static member TestData = 
        [|
            ([2;1;1;0], [0;1;1;2])
            ([0;1;1;2;2;1;1;0], [0;1;1;2;2;1;1;0])
            ([5;1;-3;8;0;17], [17;0;8;-3;1;5])
            ([-1], [-1])
            ([-1;1], [1;-1])
            ([-1;1], [1;-1])
        |]
    
    [<TestCaseSource("TestData")>]
    member this.successfulTests (testData: int list * int list) =
        let lst, result = testData
        reverse lst |> should equal result 