module LazyTests

open System
open System.Threading
open NUnit.Framework
open Lazy
open FsUnit

[<Test>]
let NaiveLazyShouldReturnExpectedString() =

    let string = "Test string"
    let naiveLazy supplier = SingleThreadedLazy supplier :> ILazy<'a>
    let foo = fun _ -> string
    let naiveChecker = naiveLazy(foo)  

    naiveChecker.Get() |> should equal string

[<Test>]
let LockFreeLazyShouldReturnExpectedString() =

    let string = "Test string"
    let lockFreeLazy supplier = LockFreeLazy supplier :> ILazy<'a>
    let foo = fun _ -> string
    let lockFreeChecker = lockFreeLazy(foo)

    lockFreeChecker.Get() |> should equal string

[<Test>]
let ConcurrentLazyShouldReturnExpectedString() =

    let string = "Test string"
    let foo = fun _ -> string
    let concurrentLazy supplier = BlockingLazy supplier :> ILazy<'a>
    let concurrentChecker = concurrentLazy(foo)
    
    concurrentChecker.Get() |> should equal string 

[<Test>]    
let CheckThatNaiveLazyAlwaysReturnsFirstCalculation() =

    let increment () =
        Object()
        
    let naiveLazy supplier = SingleThreadedLazy supplier :> ILazy<'a>
    let naive = naiveLazy(increment)

    Assert.AreSame(naive.Get(), naive.Get())

[<Test>]    
let CheckThatLockFreeLazyAlwaysReturnsFirstCalculation() =

    let lockFreeLazy supplier = LockFreeLazy supplier :> ILazy<'a>
    let mre = new ManualResetEvent(false)
    mre.Set() |> ignore
    let increment() =
        mre.WaitOne() |> ignore
        Object()
        
    let lockFree = lockFreeLazy(increment)
    
    let tasks = [
        for _ in 0 .. 100 do
        async {
            Assert.AreSame(lockFree.Get(), lockFree.Get())
            }
        ]

    let readyTasks = tasks |> Async.Parallel
    mre.Set() |> ignore
    readyTasks |> Async.RunSynchronously |> ignore

[<Test>]
let CheckThatConcurrentLazyAlwaysReturnsFirstCalculationAndDoNotEvaluateMoreThanOnce() =

    let concurrentLazy supplier = BlockingLazy supplier :> ILazy<'a>
    
    let mre = new ManualResetEvent(false)
    
    mre.Set() |> ignore
    
    let counter = ref 0

    let increment() =
        mre.WaitOne() |> ignore
        Interlocked.Increment counter |> ignore
        Object()

    let concurrent = concurrentLazy(increment)
    
    let tasks = [
        for _ in 0 .. 100 do
        async {
            Assert.AreSame(concurrent.Get(), concurrent.Get())
            }
        ]

    let readyTasks = tasks |> Async.Parallel
    mre.Set() |> ignore
    readyTasks |> Async.RunSynchronously |> ignore

    counter.Value |> should equal 1
