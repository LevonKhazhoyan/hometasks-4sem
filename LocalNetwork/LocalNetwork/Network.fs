module LocalNetwork.Network

open LocalNetwork.Virus

type Network (viruses: Virus[]) =
    let mutable counterOfSteps = 0
    
    let rec infection viruses =
        let viruses = viruses |> Seq.filter (fun (v: Virus) -> v.AbleToInfect)
        if Seq.isEmpty viruses then () else
        counterOfSteps <- counterOfSteps + 1
        for virus in viruses do virus.SpreadInfection()
        infection viruses

    member this.Start() =
        infection viruses

    member this.Viruses() = viruses

    member this.Steps() = counterOfSteps