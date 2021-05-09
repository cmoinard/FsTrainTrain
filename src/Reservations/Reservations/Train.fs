namespace Reservations

open Utils.Core.Numbers
open Utils.Core

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
        let carOption = train |> getCar carNumber
        match seatsCount, carOption with
        | Negative, _ ->
            Error (ReservationError NegativeNumberOfSeats)
            
        | _, None ->
            Error (ReservationError CarNumberNotFound)
            
        | _, Some car ->
            match car |> Car.availableSeats with
            | availableSeats when availableSeats < seatsCount ->
                let error = NoSeatsAvailable (carNumber, availableSeats)
                Error (ReservationError error)
                
            | _ ->
                let matchesCarNumber car =
                    car.Number = carNumber
                    
                let increaseReservedSeats car =
                    { car with ReservedSeats = car.ReservedSeats + seatsCount }
                    
                Ok {
                    Cars =
                        train.Cars
                        |> List.replaceWhen matchesCarNumber increaseReservedSeats
                }
            