module LocalNetwork.Virus

open System
open System.Collections.Generic
open LocalNetwork.Computer

type Virus (name: string, firstInfected: Computer[], connections: bool[,], computers: Computer[]) =
    
    let infectedComputers = HashSet(firstInfected)
    
    let getNeighbours (computer: Computer) =
        let acc = HashSet()
        connections[computer.Id, *]
        |> Array.iteri (fun index elem -> if elem then acc.Add computers[index] |> ignore)
        acc
    
    let newInfectionCandidates infected =
        infected
        |> Seq.map getNeighbours
        |> Seq.concat
        |> Seq.distinct
        |> Seq.filter (not << infectedComputers.Contains)
        |> Seq.filter (fun x -> x.OperationSystem.Immunity < 1)
        
    let mutable infectionCandidates = newInfectionCandidates infectedComputers

    member this.InfectedComputers = infectedComputers

    member this.AbleToInfect = not (Seq.isEmpty infectionCandidates)
        
    member this.Infect (computer: Computer) =
        if (Random().NextDouble() > computer.OperationSystem.Immunity) then
            computer.IsInfected <- true
            
    member this.SpreadInfection() =
        let newInfected = List()

        for computer in infectionCandidates do
            this.Infect computer
            newInfected.Add(computer)

        infectedComputers.UnionWith(newInfected)
        infectionCandidates <- newInfectionCandidates infectedComputers
    
    member this.Name = name
            
    