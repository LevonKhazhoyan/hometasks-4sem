module ControlTasks

type ConcurrentStack<'T>() =    
    let mutable _stack : List<'T> = []

    member this.Push value =
      lock _stack (fun () -> 
         _stack <- value :: _stack)

    member this.TryPop() =
      lock _stack (fun () ->
         match _stack with
         | result :: newStack ->
            _stack <- newStack
            Some result
         | [] -> None
      )
      
let diamond n =
    let rec line n x =
        if x = n then ""
        else " " + line n (x + 1)
    let rec stars n x =
        if x = n then "*"
        else "*" + stars n (x + 1)
    let rec top halfRows row =
        if row = halfRows + 1 then ""
        else
            (line halfRows row) + (stars (row * 2 - 1) 1) + "\n" + (top halfRows (row + 1))
    let rec bottom halfRows row =
        if row = 0 then ""
        else
            (line halfRows row) + (stars (row * 2 - 1) 1) + "\n" + (bottom halfRows (row - 1))
    top n 1 + bottom n (n - 1)
    
let rec superMap f xs =
    match xs with
    | [] -> []
    | x::xs' -> f x @ superMap f xs'