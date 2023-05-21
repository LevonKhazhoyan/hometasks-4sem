module FactorialFunc

let factorial n =
    if n < 0 then failwith "Expected non-negative input"
    let rec loop acc n =
        match n with
        | 0 | 1 -> acc  
        | _ -> loop (acc * n) (n - 1) 
    loop 1 n