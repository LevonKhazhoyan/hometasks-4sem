module CrawlerTests

open System.IO
open NUnit.Framework
open Crawler
open FsUnit


[<Test>]
let ``SearchClassInstancesAsync should find class instances in files``() =
    let directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles")

    let expectedOutput = [
        "In file C:\Users\я\Desktop\hometasks-4sem\classCrawler\CrawlerTests\\bin\Debug\\net6.0\TestFiles\FailedBeforeClass.cs instance of class Exception is created at line 9";
        "In file C:\Users\я\Desktop\hometasks-4sem\classCrawler\CrawlerTests\\bin\Debug\\net6.0\TestFiles\ExceptionsTests.cs instance of class ArgumentOutOfRangeException is created at line 15";
        "In file C:\Users\я\Desktop\hometasks-4sem\classCrawler\CrawlerTests\\bin\Debug\\net6.0\TestFiles\PassedTests.cs instance of class Exception is created at line 25";
        "In file C:\Users\я\Desktop\hometasks-4sem\classCrawler\CrawlerTests\\bin\Debug\\net6.0\TestFiles\PassedTests.cs instance of class Exception is created at line 34";
        "In file C:\Users\я\Desktop\hometasks-4sem\classCrawler\CrawlerTests\\bin\Debug\\net6.0\TestFiles\ExceptionsTests.cs instance of class ArgumentNullException is created at line 19";
        "In file C:\Users\я\Desktop\hometasks-4sem\classCrawler\CrawlerTests\\bin\Debug\\net6.0\TestFiles\PassedTests.cs instance of class Exception is created at line 43";
        "In file C:\Users\я\Desktop\hometasks-4sem\classCrawler\CrawlerTests\\bin\Debug\\net6.0\TestFiles\ExceptionsTests.cs instance of class ArgumentException is created at line 23";
        "In file C:\Users\я\Desktop\hometasks-4sem\classCrawler\CrawlerTests\\bin\Debug\\net6.0\TestFiles\PassedTests.cs instance of class Exception is created at line 52";
    ]

    async {
        let! results = searchClassInstancesAsync directoryPath
        CollectionAssert.AreEquivalent(expectedOutput, results)
    } |> Async.RunSynchronously
