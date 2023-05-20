module RoundingBuilderUnitTests

open NUnit.Framework
open RoundingBuilder

[<Test>]
let RoundBuilderTest () =
    let rounding x = RounderBuilder(x)
    let result = rounding 3 {
        let! a = 2.0 / 12.0
        let! b = 3.5
        return a / b
    }
    Assert.AreEqual(0.048, result)

[<Test>]
let RoundBuilderTest2 () =
    let rounding x = RounderBuilder(x)
    Assert.Throws<System.ArgumentOutOfRangeException>(fun () ->
        rounding -3 {
            let! a = 2.0 / 12.0
            let! b = 3.5
            return a / b
        } |> ignore) |> ignore
    
[<Test>]
let RoundBuilderTest3 () =
    let rounding x = RounderBuilder(x)
    let result = rounding 3 {
        let! a = -2.0 / 12.0
        let! b = 3.5
        return a / b
    }
    Assert.AreEqual(-0.048, result)
