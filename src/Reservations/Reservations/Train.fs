namespace Reservations

type CarNumber = CarNumber of int
type TrainNumber = TrainNumber of int

type Car = {
    Number: CarNumber
    Capacity: int
    ReservedSeats: int
}

type Train = private {
    Cars: Car list
}


type TrainInitializationError =
    | EmptyCapacities
    | InitialCapacityBelowZero

module Train =
    let cars train = train.Cars
    
    let init carCapacities =
        match carCapacities with
        | [] ->
            Error EmptyCapacities
        
        | xs when xs |> List.exists (fun x -> x <= 0) ->
            Error InitialCapacityBelowZero
            
        | _ ->
            let cars =
                carCapacities
                |> List.mapi (fun index capacity ->
                    {
                        Number = CarNumber (index + 1)
                        Capacity = capacity
                        ReservedSeats = 0
                    })
            
            Ok { Cars = cars }