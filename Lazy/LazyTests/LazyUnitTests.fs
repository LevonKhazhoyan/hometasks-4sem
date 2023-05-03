module LazyTests

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

    let string = "beb"

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

    let naiveLazy supplier = SingleThreadedLazy supplier :> ILazy<'a>
    
    let currentSecond() = System.DateTime.Now.Second 

    let naive = naiveLazy(currentSecond)

    naive.Get() |> should equal (naive.Get())




[<Test>]    

let CheckThatLockFreeLazyAlwaysReturnsFirstCalculation() =

    let lockFreeLazy supplier = LockFreeLazy supplier :> ILazy<'a>
    
    let currentSecond() = System.DateTime.Now.Second 

    let lockFree = lockFreeLazy(currentSecond)

    lockFree.Get() |> should equal (lockFree.Get())




[<Test>]

let CheckThatConcurrentLazyAlwaysReturnsFirstCalculation() =

    let concurrentLazy supplier = ConcurrentLazy supplier :> ILazy<'a>
    
    let currentSecond() = System.DateTime.Now.Second 

    let concurrent = concurrentLazy(currentSecond)  

    concurrent.Get() |> should equal (concurrent.Get())




[<Test>]   

let CheckConcurrencyInConcurrentLazy() =

    let concurrentLazy supplier = ConcurrentLazy supplier :> ILazy<'a>
    
    let random() = System.Random().Next()

    let concurrent = concurrentLazy(random)

    [ for _ in 0 .. 100 do async {

        concurrent.Get() |> should equal (concurrent.Get())

    } ] |> Async.Parallel |> Async.RunSynchronously |> ignore




[<Test>]   

let CheckConcurrencyInLockFreeLazy() =

    let lockFreeLazy supplier = LockFreeLazy supplier :> ILazy<'a>
    
    let random() = System.Random().Next()

    let lockFree = lockFreeLazy(random)

    [ for _ in 0 .. 100 do async {

        lockFree.Get() |> should equal (lockFree.Get())

    } ] |> Async.Parallel |> Async.RunSynchronously |> ignore