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
    async {
        printfn "\nCommand code: "
        let code = Console.ReadLine()
        match code with
        | "0" ->
            printfn "Exiting..."
        | "1" ->
            let! record = phoneBook.readRecord() |> Async.AwaitTask 
            do! phoneBook.addRecord record |> Async.AwaitTask 
            printfn "Record added."
        | "2" ->
            let! message = phoneBook.readData "name" |> Async.AwaitTask 
            let! numbers = phoneBook.findNumber message |> Async.AwaitTask 
            printfn $"Found: {numbers}"
        | "3" ->
            let! number = phoneBook.readData "number" |> Async.AwaitTask 
            let! name =  phoneBook.findName number |> Async.AwaitTask 
            printfn $"Found: {name}"
        | "4" -> 
            do! phoneBook.printFullList |> Async.AwaitTask 
        | "5" ->
            let! path =  phoneBook.readData "file path" |> Async.AwaitTask  
            do! phoneBook.printToFile path |> Async.AwaitTask  
        | "6" ->
            let! path =  phoneBook.readData "file path" |> Async.AwaitTask  
            let! newBook = phoneBook.readFromFile path |> Async.AwaitTask 
            do! PhoneBook newBook |> processing 
        | _ -> 
            printfn "Incorrect command"
        return! phoneBook |> processing
    }


// Entry point
[<EntryPoint>]
let main _ =
    firstStart()    
    PhoneBook Set.empty |> processing |> Async.RunSynchronously
    0
