module MiniCrawlerTests

open System
open NUnit.Framework
open Crawler
open FsUnit

[<Test>]
let Test1 () =
    let url = "https://se.math.spbu.ru/"

    let expected =
        [ ("https://oops.math.spbu.ru/SE/alumni", Some 49175)
          ("https://ru.wikipedia.org/wiki/%D0%A2%D0%B5%D1%80%D0%B5%D1%85%D0%BE%D0%B2,_%D0%90%D0%BD%D0%B4%D1%80%D0%B5%D0%B9_%D0%9D%D0%B8%D0%BA%D0%BE%D0%BB%D0%B0%D0%B5%D0%B2%D0%B8%D1%87",
           Some 86927)
          ("https://ru.wikipedia.org/wiki/%D0%A2%D0%B5%D1%80%D0%B5%D1%85%D0%BE%D0%B2,_%D0%90%D0%BD%D0%B4%D1%80%D0%B5%D0%B9_%D0%9D%D0%B8%D0%BA%D0%BE%D0%BB%D0%B0%D0%B5%D0%B2%D0%B8%D1%87",
           Some 86927)
          ("https://oops.math.spbu.ru/SE/alumni", Some 49175) ]
        |> Seq.ofList

    let result = getInfo url |> Async.RunSynchronously

    result |> should equal expected

[<Test>]
let Test2 () =
    (fun () -> getInfo "abcd" |> Async.RunSynchronously |> ignore) |> should throw typeof<InvalidOperationException>

[<Test>]
let Test3 () =
    let result = getInfo "https://www.youtube.com/" |> Async.RunSynchronously
    result |> should equal []