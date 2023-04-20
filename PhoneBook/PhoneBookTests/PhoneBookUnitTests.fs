module PhoneBook.Tests

open NUnit.Framework
open FsUnit
open PhoneBookFunctions

[<Test>]
let ``Test for readFromFile`` () =
    let book = [{ Name = "Name"; Number = "85"}; {Name = "Me"; Number = "87"}; {Name =  "Lizz"; Number = "15"}]
    readFromFile "..//..//..//testBook.txt" |> should equal book

[<Test>]
let ``Test for printToFile`` () =
    let path = "..//..//..//test.txt"
    let book = [{ Name = "Name"; Number = "85"}; {Name = "Me"; Number = "87"}; {Name =  "Lizz"; Number = "15"}]
    printToFile book path
    readFromFile path |> should equal book

[<TestCase("Name", "85")>]
[<TestCase("Me", "87")>]
[<TestCase("Lizz", "15")>]
let ``Test for searching`` (name, number) =
    let book = [{ Name = "Name"; Number = "85"}; {Name = "Me"; Number = "87"}; {Name =  "Lizz"; Number = "15"}]
    findNumber book name |> should equal [number]
    findName book number |> should equal [name]