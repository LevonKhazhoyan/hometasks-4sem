module LazyTests

open System
open System.Threading
open NUnit.Framework
open Lazy
open FsUnit

    
[<Test>]
let NaiveLazyShouldReturnExpectedString() =

    let naiveLazy supplier = SingleThreadedLazy supplier :> ILazy<'a>
    
    let string = "Test string"

    let foo = fun _ -> string

    let naiveChecker = naiveLazy(foo)  

    naiveChecker.Get() |> should equal string

[<Test>]
let LockFreeLazyShouldReturnExpectedString() =

    let string = "test string"

    let lockFreeLazy supplier = LockFreeLazy supplier :> ILazy<'a>
    
    let foo = fun _ -> string

    let lockFreeChecker = lockFreeLazy(foo)

    lockFreeChecker.Get() |> should equal string

[<Test>]
let ConcurrentLazyShouldReturnExpectedString() =

    let string = "test string"

    let foo = fun _ -> string

    let concurrentLazy supplier = ConcurrentLazy supplier :> ILazy<'a>
    
    let concurrentChecker = concurrentLazy(foo)

    concurrentChecker.Get() |> should equal string 

[<Test>]    

let CheckThatNaiveLazyAlwaysReturnsFirstCalculation() =

    // mutable in order to create object that will be located in heap
    let mutable x = 0
    let increment () = x <- x + 1
    
    let naiveLazy supplier = SingleThreadedLazy supplier :> ILazy<'a>

    let naive = naiveLazy(increment)

    Object.ReferenceEquals(naive.Get(), naive.Get()) |> Assert.IsTrue

[<Test>]    
let CheckThatLockFreeLazyAlwaysReturnsFirstCalculation() =

    let lockFreeLazy supplier = LockFreeLazy supplier :> ILazy<'a>
    
    let mutable x = 0
    
    let increment () = x <- x + 1
        
    let lockFree = lockFreeLazy(increment)
     
    Object.ReferenceEquals(lockFree.Get(), lockFree.Get()) |> Assert.IsTrue   

[<Test>]
let CheckThatConcurrentLazyAlwaysReturnsFirstCalculation() =

    let concurrentLazy supplier = ConcurrentLazy supplier :> ILazy<'a>
    
    let mutable x = 0
    let mutable p = 0
    let incrementValue() =
        x <- x + 1
        Interlocked.Increment(ref p) |> ignore
        x

    let concurrent = concurrentLazy(incrementValue)
    
    let task = async {
        concurrent.Get()
        |> should equal 1
    }
    
    Seq.init 2 (fun _ -> task)
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore

[<Test>]
let CheckConcurrencyInConcurrentLazy() =

    let concurrentLazy supplier = ConcurrentLazy supplier :> ILazy<'a>

    let mutable x = 0
    let random() = Interlocked.Increment(ref x) 

    let concurrent = concurrentLazy(random)

    [ for _ in 0 .. 100 do async {

        concurrent.Get() |> should equal (concurrent.Get())

    } ] |> Async.Parallel |> Async.RunSynchronously |> ignore

[<Test>]   
let CheckConcurrencyInLockFreeLazy() =

    let lockFreeLazy supplier = LockFreeLazy supplier :> ILazy<'a>
    
    let mutable x = 0
    let random() = Interlocked.Increment(ref x)
    
    let lockFree = lockFreeLazy(random)

    [ for _ in 0 .. 100 do async {

        lockFree.Get() |> should equal (lockFree.Get())

    } ] |> Async.Parallel |> Async.RunSynchronously |> ignore