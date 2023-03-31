module Balanced_brackets_sequence.Tests

open NUnit.Framework
open FsUnit
open BalanceChecking

[<TestCase("((", false)>]
[<TestCase("", true)>]
[<TestCase("(()]", false)>]
[<TestCase("{()(())[]}", true)>]
[<TestCase(")}})[]", false)>]
let Test1 (string: string, result: bool) =
    checkBalance (Seq.toList string) |> should equal result