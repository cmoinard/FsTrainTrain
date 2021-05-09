module Reservations.Tests.Train.ReserveSeatsTests

open Reservations

open Utils.Core.Result

open Xunit
open Swensen.Unquote
open Xunit.Sdk

[<Theory>]
[<InlineData(0)>]
[<InlineData(-1)>]
let ``Cannot reserve empty seats`` seats =
    let carNumber = CarNumber 1
    
    let actual =
        Train.init [ 100 ; 200 ]
        >>= Train.reserveSeatsIn carNumber seats
        
    actual =! Error (ReservationError NegativeNumberOfSeats)
    

[<Fact>]
let ``Cannot reserve a non-existing car number`` () =
    let carNumber = CarNumber 4
        
    let actual =
        Train.init [ 100 ; 200 ]
        >>= Train.reserveSeatsIn carNumber 3
        
    actual =! Error (ReservationError CarNumberNotFound)

[<Theory>]
[<InlineData(1, 133, 100)>]
[<InlineData(1, 101, 100)>]
[<InlineData(2, 222, 200)>]
[<InlineData(2, 201, 200)>]
let ``Cannot reserve more than the car available seats in empty train`` number seatsAsked expectedAvailable =
    let carNumber = CarNumber number
    
    let actual =
        Train.init [ 100; 200 ]
        >>= Train.reserveSeatsIn carNumber seatsAsked
        
    let noSeatsAvailableError = NoSeatsAvailable (carNumber, expectedAvailable)
    actual =! Error (ReservationError noSeatsAvailableError)
    


[<Theory>]
[<InlineData(1,  20, 133,  80)>]
[<InlineData(1,  30,  71,  70)>]
[<InlineData(2,  20, 222, 180)>]
[<InlineData(2, 133,  68,  67)>]
let ``Cannot reserve more than the car available seats`` number alreadyReservedSeats seatsAsked expectedAvailable =
    let carNumber = CarNumber number
    
    let actual =
        Train.init [ 100 ; 200 ]
        >>= Train.reserveSeatsIn carNumber alreadyReservedSeats
        >>= Train.reserveSeatsIn carNumber seatsAsked
        
    let noSeatsAvailableError = NoSeatsAvailable (carNumber, expectedAvailable)
    actual =! Error (ReservationError noSeatsAvailableError)
    
    
[<Theory>]
[<InlineData(1,  20,  50,  70)>]
[<InlineData(1,  30,  70, 100)>]
[<InlineData(2,  20, 120, 140)>]
[<InlineData(2, 133,  67, 200)>]
let ``Can reserve less than the car available seats`` number alreadyReservedSeats seatsAsked expectedReserved =
    let carNumber = CarNumber number
    
    let actual =
        Train.init [ 100 ; 200 ]
        >>= Train.reserveSeatsIn carNumber alreadyReservedSeats
        >>= Train.reserveSeatsIn carNumber seatsAsked

    let expectedTrain =
        Train.init [ 100 ; 200 ]
        >>= Train.reserveSeatsIn carNumber expectedReserved
        
    match actual, expectedTrain with
    | Ok a, Ok e -> a =! e
    | Error _, _ | _, Error _ -> raise <| XunitException("There should not have errors")

        