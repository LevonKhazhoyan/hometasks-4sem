module ListReverseFunc

// reverse list
let reverse l =
    let rec loop acc =
        function
        | [] -> acc
        | head :: tail -> loop (head :: acc) tail

    loop [] l