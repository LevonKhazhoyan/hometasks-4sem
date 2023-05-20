module PhoneBook.Program

open System
open PhoneBookAgent

let firstStart () = 
    printfn "Phone Book Application"
    printfn "What would you like to do?
    0 - exit
    1 - add record (name and phone)
    2 - find number by name
    3 - find name by number
    4 - print all records
    5 - save data to the file
    6 - read data from file"

let rec processing (phoneBook: PhoneBook) = 
    printfn "\nCommand code: "
    let code = Console.ReadLine()
    match code with
    | "0" ->
        printfn "Exiting..."
    | "1" ->
        let record = phoneBook.readRecord() |> Async.AwaitTask |> Async.RunSynchronously 
        do phoneBook.addRecord record |> Async.AwaitTask |> Async.RunSynchronously
        printfn "Record added."
        processing phoneBook
    | "2" ->
        let message = phoneBook.readData "name" |> Async.AwaitTask |> Async.RunSynchronously
        let numbers = phoneBook.findNumber message |> Async.AwaitTask |> Async.RunSynchronously
        printfn $"Found: {numbers}"
        processing phoneBook
    | "3" ->
        let number = phoneBook.readData "number" |> Async.AwaitTask |> Async.RunSynchronously
        let name =  phoneBook.findName number |> Async.AwaitTask |> Async.RunSynchronously
        printfn $"Found: {name}"
        processing phoneBook
    | "4" -> 
        do phoneBook.printFullList |> Async.AwaitTask |> Async.RunSynchronously
        processing phoneBook
    | "5" ->
        let path =  phoneBook.readData "file path" |> Async.AwaitTask |> Async.RunSynchronously 
        do phoneBook.printToFile path |> Async.AwaitTask |> Async.RunSynchronously 
        processing phoneBook
    | "6" ->
        let path =  phoneBook.readData "file path" |> Async.AwaitTask |> Async.RunSynchronously 
        let newBook = phoneBook.readFromFile path |> Async.AwaitTask |> Async.RunSynchronously
        PhoneBook newBook |> processing 
    | _ -> 
        printfn "Incorrect command"
        processing phoneBook


// Entry point
[<EntryPoint>]
let main _ =
    firstStart()    
    PhoneBook [] |> processing
    0
