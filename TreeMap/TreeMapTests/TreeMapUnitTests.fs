module TreeMapTests

open NUnit.Framework
open TreeMapFunc
open FsUnit

[<TestFixture>]
type treeMapTests() =
    
    static member TestData = 
        [|
            (BinaryTree (1, None, None),
             id,
             BinaryTree (1, None, None))
            (BinaryTree (1, None, None),
             (fun x -> x * 0),
             BinaryTree (0, None, None))
            (BinaryTree (2,  BinaryTree(3,  BinaryTree(4, None, None), BinaryTree(5, None, None)), BinaryTree(4, None, None)),
             (fun x -> x + 2),
             BinaryTree (4,  BinaryTree(5,  BinaryTree(6, None, None), BinaryTree(7, None, None)), BinaryTree(6, None, None)))
            (BinaryTree (1,  BinaryTree(1,  BinaryTree(1, None, None), BinaryTree(1, None, None)), BinaryTree(1, None, None)),
             (pown 1),
             BinaryTree (1,  BinaryTree(1,  BinaryTree(1, None, None), BinaryTree(1, None, None)), BinaryTree(1, None, None)))
        |]

    [<TestCaseSource("TestData")>]
    member this.successfulTests (testData: BinaryTree<int> * (int -> int)  * BinaryTree<int>) =
        let key, lst, result = testData
        (BinaryTree.treeMap lst key) |> should equal result