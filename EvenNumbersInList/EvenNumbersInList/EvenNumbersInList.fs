module EvenNumbersInListFunc

let evenNumbersInList lst =
    List.map (fun x -> (abs x + 1) % 2) lst
    |> List.fold (+) 0

let evenNumbersInList' lst =
    List.filter (fun x -> x % 2 = 0) lst
    |> List.length

let evenNumbersInList'' lst =
    List.fold (fun acc x -> if x % 2 = 0 then acc + 1 else acc) 0 lst