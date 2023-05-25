module LocalNetwork.Computer


type OperationSystem (name: string, immunity: float) =
    member this.Name = name
    member this.Immunity = immunity

type Computer (name: string, id: int, operationSystem: OperationSystem, isInfectedAtStart: bool) =
    let mutable isInfected = isInfectedAtStart
    member this.Id = id
    member this.Name = name
    member this.OperationSystem = operationSystem
    member val IsInfected = isInfectedAtStart with get, set
    

    
    
    