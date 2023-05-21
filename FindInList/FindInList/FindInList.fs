module FindInListFunc

let find key lst =
    let rec loop key index =
        function
        | [] -> None
        | head :: tail ->
            if head = key then
                Some index
            else
                loop key (index + 1) tail

    loop key 0 lst