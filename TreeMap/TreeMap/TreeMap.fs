module TreeMapFunc

type BinaryTree<'t> =
    | None
    | BinaryTree of 't * BinaryTree<'t> * BinaryTree<'t>

    static member treeMap (func: 't -> 'a) (tree: BinaryTree<'t>) =
        let rec loop =
            function
            | BinaryTree (value, left, right) -> BinaryTree(func value, loop left, loop right)
            | _ -> None

        loop tree