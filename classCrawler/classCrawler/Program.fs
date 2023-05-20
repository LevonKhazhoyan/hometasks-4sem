module Crawler

open System
open System.Collections.Generic
open System.IO
open System.Text.RegularExpressions
open System.Threading.Tasks

let searchClassInstancesAsync directoryPath =
    let csharpFilePattern = "*.cs"
    let classNamePattern = @"(?:\bnew\s+)(?<ClassName>[A-Za-z_][A-Za-z0-9_]*)(?:\s*\()"

    let getFilesWithExtensionAsync path extension =
        Task.Run(fun () -> Directory.GetFiles(path, extension, SearchOption.AllDirectories))

    let extractClassInstancesAsync filePath classNamePattern (output: List<string>) =
        async {
            let! fileLines = File.ReadAllLinesAsync(filePath) |> Async.AwaitTask
            fileLines
            |> Seq.mapi (fun index line ->
                Regex.Matches(line, classNamePattern)
                |> Seq.cast<Match>
                |> Seq.map (fun matchObj -> matchObj.Groups.["ClassName"].Value, index + 1, filePath)
            ) |> Seq.concat
            |> Seq.iter (fun (className, lineIndex, file) ->
                let result = $"In file {file} instance of class {className} is created at line {lineIndex}"
                output.Add(result))
        }

    let searchAsync () =
        async {
            let files = getFilesWithExtensionAsync directoryPath csharpFilePattern |> Async.AwaitTask |> Async.RunSynchronously
            let output = List<string>()
            let tasks =
                files
                |> Seq.map (fun file -> extractClassInstancesAsync file classNamePattern output)

            do! tasks |> Async.Parallel |> Async.Ignore

            return output
        }

    async {
        return! searchAsync()
    }
    
[<EntryPoint>]
let main _ =
    let directoryName = Console.ReadLine()
    async {
        let! results = searchClassInstancesAsync directoryName
        results |> Seq.iter (printfn "%s")
    } |> Async.RunSynchronously
    0