module Crawler

open System
open System.Collections.Concurrent
open System.IO
open System.Text.RegularExpressions
open System.Threading.Tasks

let searchClassInstancesAsync directoryPath =
    
    let csharpFilePattern = "*.cs"
    let classNamePattern = @"(?:\bnew\s+)(?<ClassName>[A-Za-z_][A-Za-z0-9_]*)(?:\s*\()"

    let getFilesWithExtensionAsync path extension =
        Task.Run(fun () -> Directory.GetFiles(path, extension, SearchOption.AllDirectories))

    let extractClassInstancesAsync filePath classNamePattern =
        async {
            let! fileLines = File.ReadAllLinesAsync(filePath) |> Async.AwaitTask
            let results =
                fileLines
                |> Seq.mapi (fun index line ->
                    Regex.Matches(line, classNamePattern)
                    |> Seq.cast<Match>
                    |> Seq.map (fun matchObj -> matchObj.Groups.["ClassName"].Value, index + 1, filePath)
                ) |> Seq.concat
                |> Seq.map (fun (className, lineIndex, file) ->
                    $"In file {file} instance of class {className} is created at line {lineIndex}"
                )
            return results
        }

    async {
        let! files = getFilesWithExtensionAsync directoryPath csharpFilePattern |> Async.AwaitTask 
        let tasks =
            files
            |> Seq.map (fun file -> extractClassInstancesAsync file classNamePattern)

        let! results =
            tasks
            |> Seq.map (fun task ->
                async {
                    let! result = task
                    return result
                })
            |> Async.Parallel
            
        return results |> Seq.concat
    }
    
[<EntryPoint>]
let main _ =
    let directoryName = Console.ReadLine()
    let results = searchClassInstancesAsync directoryName |> Async.RunSynchronously
    results |> Seq.iter (printfn "%s")
    0