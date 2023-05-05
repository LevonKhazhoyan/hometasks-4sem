module StringCalcTests

open NUnit.Framework
open StringCalc

[<Test>]
let StringCalculatorBuilderTest () =
    let calculate = StringCalculatorBuilder()
    let result = calculate {
        let! x = "1"
        let! y = "2"
        let z = x + y
        return z
    }
    Assert.AreEqual(Some(3), result)

[<Test>]
let StringCalculatorBuilderTest2 () =
    let calculate = StringCalculatorBuilder()
    let result = calculate {
        let! x = "1"
        let! y = "X"
        let z = x + y
        return z
    }
    Assert.AreEqual(None, result)