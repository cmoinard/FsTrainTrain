namespace Reservations

type CarNumber = CarNumber of int
type TrainNumber = TrainNumber of int

type Car = {
    Number: CarNumber
    Capacity: int
    ReservedSeats: int
}
module Car =
    let availableSeats car =
        car.Capacity - car.ReservedSeats

type Train = private {
    Cars: Car list
}


type TrainInitializationError =
    | EmptyCapacities
    | InitialCapacityBelowZero
    
type TrainReservationError =
    | CarNumberNotFound
    | NegativeNumberOfSeats
    | NoSeatsAvailable of CarNumber * int 
    
type TrainError =
    | InitializationError of TrainInitializationError
    | ReservationError of TrainReservationError

module Train =
    let cars train = train.Cars
    
    let getCar carNumber train =
        train.Cars
        |> List.tryFind (fun c -> c.Number = carNumber)
    
    let init carCapacities =
        match carCapacities with
        | [] ->
            Error (InitializationError EmptyCapacities)
        
        | xs when xs |> List.exists (fun c -> c <= 0) ->
            Error (InitializationError InitialCapacityBelowZero)
            
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
            
    let reserveSeatsIn carNumber seatsCount train : Result<Train, TrainError> =
        match (train |> getCar carNumber) with
        | None -> Error (ReservationError CarNumberNotFound)
        | Some car ->
            if seatsCount = 0 then
                Error (ReservationError NegativeNumberOfSeats)
            else
                let availableSeats = car |> Car.availableSeats
                if seatsCount > availableSeats then
                    let error = NoSeatsAvailable (carNumber, availableSeats)
                    Error (ReservationError error)
                else
                    let newTrain = 
                        {
                            Cars =
                                train.Cars
                                |> List.map (fun car ->
                                    if car.Number = carNumber then
                                        { car with ReservedSeats = car.ReservedSeats + seatsCount }
                                    else
                                        car)
                        }
                    Ok newTrain
            