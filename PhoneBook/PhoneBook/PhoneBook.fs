module PhoneBookAgent   

open System
open System.IO
open System.Threading.Tasks
open Microsoft.FSharp.Control

// Phone book record
type PhoneNumber =
    { Name: string
      Number: string }

// Message types
type PhoneBookMessage =
    | AddRecord of PhoneNumber * TaskCompletionSource<unit>
    | FindNumber of string * TaskCompletionSource<string Set>
    | FindName of string * TaskCompletionSource<string Set>
    | ReadRecord of TaskCompletionSource<PhoneNumber>
    | ReadData of string * TaskCompletionSource<string>
    | PrintFullBook of TaskCompletionSource<unit>
    | PrintToFile of string * TaskCompletionSource<unit>
    | ReadFromFile of string * TaskCompletionSource<PhoneNumber Set>

type PhoneBook (pbSet: Set<PhoneNumber>) =

    // Phone book agent
    member val phoneBookAgent =
        MailboxProcessor<PhoneBookMessage>.Start(fun inbox ->
            let rec loop phoneBook =
                async {
                    let! msg = inbox.Receive()
                    match msg with
                    | AddRecord (newRecord, tcs) ->
                        let updatedPhoneBook = Set.add newRecord phoneBook
                        tcs.SetResult()
                        return! loop updatedPhoneBook

                    | FindNumber (name, tcs) ->
                        let numbers = phoneBook
                                      |> Set.filter (fun item -> item.Name = name)
                                      |> Set.map (fun item -> item.Number)
                        tcs.SetResult numbers
                        return! loop phoneBook

                    | FindName (number, tcs) ->
                        let names = phoneBook
                                    |> Set.filter (fun item -> item.Number = number)
                                    |> Set.map (fun item -> item.Name)                                    
                        tcs.SetResult names
                        return! loop phoneBook

                    | PrintFullBook tcs ->
                        match phoneBook with
                        | set when Set.isEmpty set -> printfn "Phone book is empty."
                        | set -> set |> Set.iter (fun record -> printfn $"{record}")
                        tcs.SetResult()
                        return! loop phoneBook

                    | ReadRecord tcs ->
                        printfn "Enter name and number"
                        let data = Console.ReadLine().Split(' ')
                        let record = { Name = data.[0]; Number = data.[1] }
                        tcs.SetResult record
                        return! loop phoneBook

                    | ReadData (message, tcs) ->
                        printfn $"Enter {message}"
                        let data = Console.ReadLine().Replace(" ", "")
                        tcs.SetResult data
                        return! loop phoneBook

                    | PrintToFile (path, tcs) ->
                        let data = phoneBook
                                   |> Set.map (fun item -> $"{item.Name} {item.Number}")
                                   |> Set.toArray
                        File.WriteAllLinesAsync(path, data) |> Async.AwaitTask |> Async.RunSynchronously
                        tcs.SetResult()
                        return! loop phoneBook

                    | ReadFromFile (path, tcs) ->
                        try
                            let lines = File.ReadAllLines(path)
                            let phoneBookData =
                                lines
                                |> Seq.map (fun string -> 
                                    let split = string.Split(' ')
                                    { Name = split.[0]; Number = split.[1] })
                                |> Set
                            tcs.SetResult phoneBookData
                            return! loop phoneBookData
                        with
                        | error ->
                            printfn $"{error.Message}"
                            return! loop phoneBook
                }
            loop pbSet)

    // Adds a record to the phone book
    member this.addRecord newRecord =
        let tcs = TaskCompletionSource<unit>()
        this.phoneBookAgent.Post (AddRecord (newRecord, tcs))
        tcs.Task

    // Prints all records in the phone book
    member this.printFullList =
        let tcs = TaskCompletionSource<unit>()
        this.phoneBookAgent.Post (PrintFullBook tcs)
        tcs.Task

    // Finds numbers by name
    member this.findNumber name =
        let tcs = TaskCompletionSource<string Set>()
        this.phoneBookAgent.Post (FindNumber (name, tcs))
        tcs.Task

    // Finds names by number
    member this.findName number =
        let tcs = TaskCompletionSource<string Set>()
        this.phoneBookAgent.Post (FindName (number, tcs))
        tcs.Task

    // Reads a record from the console
    member this.readRecord () =
        let tcs = TaskCompletionSource<PhoneNumber>()
        this.phoneBookAgent.Post (ReadRecord tcs)
        tcs.Task

    // Reads data from the console
    member this.readData message =
        let tcs = TaskCompletionSource<string>()
        this.phoneBookAgent.Post (ReadData (message, tcs))
        tcs.Task

    // Prints data to a file
    member this.printToFile path =
        let tcs = TaskCompletionSource<unit>()
        this.phoneBookAgent.Post (PrintToFile (path, tcs))
        tcs.Task

    // Reads data from a file
    member this.readFromFile path =
        let tcs = TaskCompletionSource<PhoneNumber Set>()
        this.phoneBookAgent.Post (ReadFromFile (path, tcs))
        tcs.Task