module PhoneBook

open System
open PhoneBookFunctions

let firstStart () = 
    printfn "What would you like to do?
    0 - exit
    1 - add record (name and phone)
    2 - find number by name
    3 - find name by number
    4 - print all records
    5 - save data to the file
    6 - read data from file"

let rec processing (phoneBook: PhoneNumber list) = 
    printfn "\nCommand code: "
    let code = Console.ReadLine()
    match code with
    | "0" -> ()
    | "1" -> 
        let record = readRecord()
        addRecord record phoneBook |> processing 
    | "2" -> 
        let name = readData "name"
        printfn "Found: "
        findNumber phoneBook name |> printList
        processing phoneBook
    | "3" -> 
        let number = readData "number"
        printfn $"Found: {findName phoneBook number}"
        processing phoneBook
    | "4" -> 
        printList phoneBook
        processing phoneBook
    | "5" ->
        readData "file path" |> printToFile phoneBook
        processing phoneBook
    | "6" ->
        readData "file path" |> readFromFile |> processing
    | _ -> 
        printfn "Incorrect command"
        processing phoneBook

firstStart()
processing []