module LazyTests

open System
open System.Collections.Generic
open System.Threading
open NUnit.Framework
open Lazy
open FsUnit

type testObject(o) =
        
    member this.Increment() =
        o + 1
    
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
    let increment () =
        Stack<int>()
    
    let naiveLazy supplier = SingleThreadedLazy supplier :> ILazy<'a>

    let naive = naiveLazy(increment)

    Assert.AreSame(naive.Get, naive.Get)

[<Test>]    
let CheckThatLockFreeLazyAlwaysReturnsFirstCalculation() =

    let lockFreeLazy supplier = LockFreeLazy supplier :> ILazy<'a>
    
    let mutable x = 0
    
    let increment() =
        x <- x + 1
        x
        
    let lockFree = lockFreeLazy(increment)
     
    lockFree.Get() |> should equal 1
    Object.ReferenceEquals(lockFree.Get(), lockFree.Get()) |> should equal true

[<Test>]
let CheckThatConcurrentLazyAlwaysReturnsFirstCalculation() =

    let concurrentLazy supplier = ConcurrentLazy supplier :> ILazy<'a>
    
    let mutable x = 0
    let mutable p = 0

    let incrementValue() =
        x <- x + 1
        p <- Interlocked.Increment(&p)
        x

    let concurrent = concurrentLazy(incrementValue)
    
    [ for _ in 0 .. 2 do async {
    
        concurrent.Get() |> should equal 1

    } ] |> Async.Parallel |> Async.RunSynchronously |> ignore

[<Test>]
let CheckConcurrencyInConcurrentLazy() =

    let concurrentLazy supplier = ConcurrentLazy supplier :> ILazy<'a>

    let mutable x = 0
    let random() =
        Interlocked.Increment(ref x) |> ignore
        x

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