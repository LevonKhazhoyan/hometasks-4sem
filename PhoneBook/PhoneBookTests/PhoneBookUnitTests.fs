module PhoneBook.Tests

open System.IO
open NUnit.Framework
open FsUnit
open PhoneBookAgent

[<Test>]
let ``Test for findNumber`` () =
    let phoneBook = PhoneBook (Set.ofList [{ Name = "Paul"; Number = "1234567890" }; { Name = "Judy"; Number = "9876543210" }])
    let result = phoneBook.findNumber "Paul" |> Async.AwaitTask |> Async.RunSynchronously
    Assert.AreEqual(Set.ofList ["1234567890"], result)

[<Test>]
let ``Test for findNumber with same name`` () =
    let phoneBook = PhoneBook (Set.ofList [{ Name = "Paul"; Number = "1234567890" }; { Name = "Paul"; Number = "9876543210" }])
    let result = phoneBook.findNumber "Paul" |> Async.AwaitTask |> Async.RunSynchronously
    Assert.AreEqual(Set.ofList ["1234567890"; "9876543210"], result)

[<Test>]
let ``Test for findName`` () =
    let phoneBook = PhoneBook (Set.ofList [{ Name = "Cole"; Number = "1234567890" }; { Name = "Marry"; Number = "9876543210" }])
    let number = "9876543210"
    let result = phoneBook.findName number |> Async.AwaitTask |> Async.RunSynchronously
    Assert.AreEqual(Set.ofList ["Marry"], result)

[<Test>]
let ``Test for addRecord with same names and different numbers`` () =
    let phoneBook = PhoneBook Set.empty
    let newRecord1 = { Name = "Paul"; Number = "1234567890" }
    let newRecord2 = { Name = "Paul"; Number = "12345678901" }
    phoneBook.addRecord newRecord1 |> Async.AwaitTask |> Async.RunSynchronously
    phoneBook.addRecord newRecord2 |> Async.AwaitTask |> Async.RunSynchronously
    let result = phoneBook.findNumber "Paul" |> Async.AwaitTask |> Async.RunSynchronously
    Assert.AreEqual(Set.ofList ["1234567890"; "12345678901"], result)

[<Test>]
let ``Test for addRecord with same numbers and names`` () =
    let phoneBook = PhoneBook Set.empty
    let newRecord1 = { Name = "Paul"; Number = "1234567890" }
    let newRecord2 = { Name = "Paul"; Number = "1234567890" }
    phoneBook.addRecord newRecord2 |> Async.AwaitTask |> Async.RunSynchronously
    phoneBook.addRecord newRecord1 |> Async.AwaitTask |> Async.RunSynchronously
    let result = phoneBook.findNumber "Paul" |> Async.AwaitTask |> Async.RunSynchronously
    Assert.AreEqual(Set.ofList ["1234567890"], result)


[<Test>]
let ``Test for readRecord`` () =
    let phoneBook = PhoneBook Set.empty
    let reader = new StringReader("John 1234567890")
    System.Console.SetIn(reader)
    let result = phoneBook.readRecord() |> Async.AwaitTask |> Async.RunSynchronously
    Assert.AreEqual({ Name = "John"; Number = "1234567890" }, result)

[<Test>] 
let ``Test for printFullList`` () =
    let phoneBook = PhoneBook (Set.ofList [{ Name = "Johnny"; Number = "1234567890" }; { Name = "Jane"; Number = "9876543210" }])
    let output = new StringWriter()
    System.Console.SetOut(output)
    phoneBook.printFullList |> Async.AwaitTask |> Async.RunSynchronously
    let printedOutput = output.ToString().Trim()
    Assert.AreEqual("{ Name = \"Jane\"\n  Number = \"9876543210\" }\n{ Name = \"Johnny\"\n  Number = \"1234567890\" }", printedOutput)