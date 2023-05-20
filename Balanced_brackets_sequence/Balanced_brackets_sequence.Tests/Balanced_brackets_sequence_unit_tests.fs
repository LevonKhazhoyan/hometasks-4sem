module Balanced_brackets_sequence.Tests

open NUnit.Framework
open FsUnit
open BalanceChecking

[<TestCase("((", false)>]
[<TestCase("", true)>]
[<TestCase("(()]", false)>]
[<TestCase("{()(())[]}", true)>]
[<TestCase(")}})[]", false)>]
let Tests (string: string, result: bool) =
    checkBalance string |> should equal result