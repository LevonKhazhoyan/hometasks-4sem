module ControlTasksUnitTests

open NUnit.Framework
open ControlTasks

[<Test>]
let ``diamond of n=1`` () =
    let actual = diamond 1
    let expected = "*\n"
    Assert.AreEqual(expected, actual)

[<Test>]
let ``diamond of n=2`` () =
    let actual = diamond 2
    let expected = " *\n***\n *\n"
    Assert.AreEqual(expected, actual)

[<Test>]
let ``diamond of n=3`` () =
    let actual = diamond 3
    let expected = "  *\n ***\n*****\n ***\n  *\n"
    Assert.AreEqual(expected, actual)

[<Test>]
let ``diamond of n=4`` () =
    let actual = diamond 4
    let expected = "   *\n  ***\n *****\n*******\n *****\n  ***\n   *\n"
    Assert.AreEqual(expected, actual)
    
[<Test>]
let ``superMap with empty list returns empty list`` () =
    Assert.AreEqual([], (superMap (fun x -> [x]) []))

[<Test>]
let ``superMap with single-element list applies function once`` () =
    Assert.AreEqual([1], (superMap (fun x -> [x]) [1]))
    
[<Test>]
let ``superMap with multiple-element list applies function to each element`` () =
    Assert.AreEqual([1; 2; 3], (superMap (fun x -> [x]) [1; 2; 3]))

[<Test>]
let ``superMap with function returning multiple values`` () =
    Assert.AreEqual([2; 3; 3; 4; 4; 5], (superMap (fun x -> [x+1; x+2]) [1; 2; 3]))

[<Test>]
let ``superMap with empty function result`` () =
    Assert.AreEqual([], (superMap (fun _ -> []) [1; 2; 3]))
    
[<Test>]
let ``Push adds element to the stack`` () =
    let stack = ConcurrentStack<int>()
    stack.Push(1)
    Assert.AreEqual(Some(1), stack.TryPop())

[<Test>]
let ``TryPop returns None when stack is empty`` () =
    let stack = ConcurrentStack<int>()
    Assert.AreEqual(None, stack.TryPop())

[<Test>]
let ``TryPop removes and returns top element of the stack`` () =
    let stack = ConcurrentStack<int>()
    stack.Push(1)
    stack.Push(2)
    Assert.AreEqual(Some(2), stack.TryPop())
    Assert.AreEqual(Some(1), stack.TryPop())