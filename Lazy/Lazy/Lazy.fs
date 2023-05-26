﻿namespace Lazy

open System.Threading

/// Lazy evaluation interface
type ILazy<'a> =
    abstract member Get : unit -> 'a

type SingleThreadedLazy<'a>(supplier: unit -> 'a) =
    let mutable result = None

    interface ILazy<'a> with
        member this.Get() =
            if result.IsNone then
                result <- Some(supplier ())

            result.Value

type BlockingLazy<'a>(supplier: unit -> 'a) =
    let locker = obj ()
    [<VolatileField>]
    let mutable result = None

    interface ILazy<'a> with
        member this.Get() =
            if result.IsNone then
                lock locker
                    (fun _ ->
                        if result.IsNone then
                            result <- Some(supplier ()))

            result.Value

type LockFreeLazy<'a>(supplier: unit -> 'a) =
    let mutable result = None

    interface ILazy<'a> with
        member this.Get() =        
            if result.IsNone
            then
                let evaluated = Some(supplier ())
                Interlocked.CompareExchange(&result, evaluated, None)
                |> ignore

            result.Value
