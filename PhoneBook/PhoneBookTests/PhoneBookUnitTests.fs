module PhoneBook.Tests

open System.IO
open NUnit.Framework
open FsUnit
open PhoneBookFunctions

[<Test>]
let ``Test for addRecord`` () =
    let phoneBook = PhoneBook []
    let newRecord = { Name = "Paul"; Number = "1234567890" }
    let result = phoneBook.addRecord newRecord |> Async.AwaitTask |> Async.RunSynchronously
    Assert.DoesNotThrow(fun () -> result)

[<Test>]
let ``Test for findNumber`` () =
    let phoneBook = PhoneBook [{ Name = "Paul"; Number = "1234567890" }; { Name = "Judy"; Number = "9876543210" }]
    let name = "Paul"
    let result = phoneBook.findNumber name |> Async.AwaitTask |> Async.RunSynchronously
    Assert.AreEqual(["1234567890"], result)

[<Test>]
let ``Test for findName`` () =
    let phoneBook = PhoneBook [{ Name = "Cole"; Number = "1234567890" }; { Name = "Marry"; Number = "9876543210" }]
    let number = "9876543210"
    let result = phoneBook.findName number |> Async.AwaitTask |> Async.RunSynchronously
    Assert.AreEqual(["Marry"], result)

[<Test>]
let ``Test for readRecord`` () =
    let phoneBook = PhoneBook []
    let reader = new StringReader("John 1234567890")
    System.Console.SetIn(reader)
    let result = phoneBook.readRecord() |> Async.AwaitTask |> Async.RunSynchronously
    Assert.AreEqual({ Name = "John"; Number = "1234567890" }, result)

[<Test>]
let ``Test for printFullList`` () =
    let phoneBook = PhoneBook [{ Name = "Johnny"; Number = "1234567890" }; { Name = "Jane"; Number = "9876543210" }]
    let output = new StringWriter()
    System.Console.SetOut(output)
    phoneBook.printFullList |> Async.AwaitTask |> Async.RunSynchronously
    let printedOutput = output.ToString().Trim()
    Assert.AreEqual("{ Name = \"Johnny\"\n  Number = \"1234567890\" }\n{ Name = \"Jane\"\n  Number = \"9876543210\" }", printedOutput)
    