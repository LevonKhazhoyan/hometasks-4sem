module FibonacciFunc

/// Fibonacci list with specified lenght
let fibonacci n =
    if n < 0 then failwith "Expected non-negative n"
    let rec loop acc1 acc2 n =
        match n with
        | 0 -> []
        | n -> acc1 :: loop acc2 (acc1 + acc2) (n - 1)
    
    loop 0 1 (n + 1)