module PointFree.Tests

open NUnit.Framework
open Functions
open FsUnit
open FsCheck

[<Test>]
let ``Check whether myFunction is correct`` () =
    let answerList = [2; 4; 8; 16; 32]
    let list = [1; 2; 4; 8; 16]
    myFunction 2 list |> should equal answerList

[<Test>]
let ``Point-free and myFunction equality check`` () =
    Check.QuickThrowOnFailure(fun number list -> (myFunction number list) = (pointFree number list))

[<Test>]
let ``Point-free and myFunction2 equality check`` () =
    Check.QuickThrowOnFailure(fun number list -> (myFunction2 number list) = (pointFree number list))

[<Test>]
let ``Point-free and myFunction3 equality check`` () =
    Check.QuickThrowOnFailure(fun number list -> (myFunction3 number list) = (pointFree number list))