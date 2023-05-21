module EvenNumbersInListTests

open FsCheck
open NUnit.Framework
open EvenNumbersInListFunc
open FsUnit

type EvenNumbersProperties =
    static member ``map equivalent to filter`` (lst: list<int>) = evenNumbersInList lst = evenNumbersInList' lst
    static member ``filter equivalent to fold`` (lst: list<int>) = evenNumbersInList' lst = evenNumbersInList'' lst

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
    member this.testsForMapImplementation (testData: int list * int) =
        let lst, result = testData
        (evenNumbersInList lst) |> should equal result

    [<Test>]
    member this.checkForEquivalency () = Check.QuickAll<EvenNumbersProperties>()
