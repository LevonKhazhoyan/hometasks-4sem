open NUnit.Framework
open LocalNetwork.Computer
open LocalNetwork.Network
open LocalNetwork.Virus
open FsUnit

[<TestFixture>]
type NetworkTests() =
    
    [<Test>]
    member this.``Given a network with infected computers, the virus should spread to connected computers``() =
        let computer0 = Computer("Computer1", 0, OperationSystem("Linux", -0.1), true)
        let computer1 = Computer("Computer2", 1, OperationSystem("Windows", -0.1), false)
        let computer2 = Computer("Computer3", 2, OperationSystem("Windows", -0.1), false)
        let computer3 = Computer("Computer4", 3, OperationSystem("MacOS", -0.1), false)
        
        let connectionsMatrix = Array2D.init 4 4 (fun i j ->
            match i, j with
            | 0, 1 -> true 
            | 1, 0 -> true 
            | 1, 2 -> true 
            | 2, 1 -> true 
            | _ -> false
        )
        let virus = Virus("TestVirus", [|computer0|], connectionsMatrix, [|computer0; computer1; computer2; computer3|])
        
        let network = Network([|virus|])
        network.Start()
        
        // Assert that no computers have been infected
        computer0.IsInfected |> should equal true
        computer1.IsInfected |> should equal true
        computer2.IsInfected |> should equal true
        computer3.IsInfected |> should equal false

    [<Test>]
    member this.``Given a network with immune computers, when starting the simulation, no computers should get infected``() =
        
        let computer0 = Computer("Computer0", 0, OperationSystem("Linux", 1.0), false)
        let computer1 = Computer("Computer1", 1, OperationSystem("Windows", 1.0), true)
        let computer2 = Computer("Computer2", 2, OperationSystem("Windows", 1.0), false)
        let computer3 = Computer("Computer3", 3, OperationSystem("MacOS", 1.0), false)
        
        let connectionsMatrix = Array2D.init 4 4 (fun i j ->
            match i, j with
            | 0, 1 -> true 
            | 1, 0 -> true 
            | 1, 2 -> true 
            | 2, 1 -> true 
            | _ -> false
        )
        
        let virus = Virus("TestVirus", [|computer1|], connectionsMatrix, [|computer0; computer1; computer2; computer3|])
        let network = Network([|virus|])
        network.Start()

        // Assert that no computers have been infected
        computer0.IsInfected |> should equal false
        computer1.IsInfected |> should equal true
        computer2.IsInfected |> should equal false
        computer3.IsInfected |> should equal false
