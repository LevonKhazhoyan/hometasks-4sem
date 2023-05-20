module CrawlerTests

open System.IO
open NUnit.Framework
open Crawler


[<Test>]
let ``SearchClassInstancesAsync should find class instances in files``() =
    let path = Path.Join("..", "..", "..", "..", "CrawlerTests", "TestFiles");
    
    let expectedOutput = [
        "In file ..\..\..\..\CrawlerTests\TestFiles\FailedBeforeClass.cs instance of class Exception is created at line 9";
        "In file ..\..\..\..\CrawlerTests\TestFiles\ExceptionsTests.cs instance of class ArgumentOutOfRangeException is created at line 15";
        "In file ..\..\..\..\CrawlerTests\TestFiles\PassedTests.cs instance of class Exception is created at line 25";
        "In file ..\..\..\..\CrawlerTests\TestFiles\PassedTests.cs instance of class Exception is created at line 34";
        "In file ..\..\..\..\CrawlerTests\TestFiles\ExceptionsTests.cs instance of class ArgumentNullException is created at line 19";
        "In file ..\..\..\..\CrawlerTests\TestFiles\PassedTests.cs instance of class Exception is created at line 43";
        "In file ..\..\..\..\CrawlerTests\TestFiles\ExceptionsTests.cs instance of class ArgumentException is created at line 23";
        "In file ..\..\..\..\CrawlerTests\TestFiles\PassedTests.cs instance of class Exception is created at line 52";
    ]

    async {
        let! results = searchClassInstancesAsync path
        CollectionAssert.AreEquivalent(expectedOutput, results)
    } |> Async.RunSynchronously
