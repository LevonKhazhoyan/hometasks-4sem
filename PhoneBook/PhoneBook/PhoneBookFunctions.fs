module PhoneBookFunctions

open System
open System.IO

// Phone book record
type PhoneNumber =
    { Name: string
      Number: string }

// Adds record to the phone book
let addRecord (newRecord: PhoneNumber) phoneBook = 
    newRecord :: phoneBook

// Finds number by name
let findNumber phoneBook name =
    List.filter(fun item -> item.Name = name) phoneBook |> List.map (fun item -> item.Number)

// Finds name by number
let findName phoneBook number =
    List.filter(fun item -> item.Number = number) phoneBook |> List.map (fun item -> item.Name)

// Reads record from console
let readRecord () =
    printfn("Enter name and number")
    let data = Console.ReadLine().Split(' ')
    { Name = data[0]; Number = data[1] }

// Reads data from console
let readData string =
    printfn($"Enter {string}")
    Console.ReadLine().Replace(" ", "")

// Prints given list to the console
let rec printList list = 
    match list with
    | [] -> printf ""
    | head :: tail -> 
        printfn "%A" head
        printList tail

// Prints data to file
let printToFile list path =
    let data = List.map (fun item -> $"{item.Name} {item.Number}") list |> List.toArray
    File.WriteAllLines(path, data)

// Reads data from file
let readFromFile path = 
    try
        File.ReadAllLines(path) |> Seq.map (fun string -> 
                                        let split = string.Split(' ')
                                        {Name = split[0]; Number = split[1]}) |> Seq.toList
    with 
    | error -> printfn $"{error.Message}"; []
