module PowerSeriesFunc

let listOfPowerSeries n m =
    if n < 0 || m < 0 then failwith "Expected positive n and m"
    let rec loop acc value length =
        if length = m + 1 then
            acc
        else
            loop (value :: acc) (value / 2) (length + 1)

    loop [] (pown 2 (n + m)) 0
    