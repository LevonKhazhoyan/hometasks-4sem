module PrimeSeqImpl

open System

let isPrime n =
    if n <= 1 then false
    else
        let rec checkDivisors divisor =
            divisor * divisor > n || (n % divisor <> 0 && checkDivisors (divisor + 1))
        
        checkDivisors 2

let primeSeq n =
    if n < 0 then raise (ArgumentException "Expected non-negative n")
    let rec generateSeq current =
        seq {
            if isPrime current then
                yield current
            yield! generateSeq (current + 1)
        }
    generateSeq 2 |> Seq.take n

