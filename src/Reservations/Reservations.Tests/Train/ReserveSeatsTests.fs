module Reservations.Tests.Train.ReserveSeatsTests

open Reservations

open Utils.Core.Result

open Xunit
open Swensen.Unquote

[<Theory>]
[<InlineData(-3)>]
[<InlineData(0)>]
let ``Cannot reserve empty or negative number of seats`` seatsCount =
    let carNumber = CarNumber 1
    
    let actual =
        Train.init [ 100 ; 200 ]
        >>= Train.reserveSeatsIn carNumber seatsCount
        
    actual =! Error (ReservationError NegativeNumberOfSeats)
    

[<Fact>]
let ``Cannot reserve a non-existing car number`` () =
    let carNumber = CarNumber 4
        
    let actual =
        Train.init [ 100 ; 200 ]
        >>= Train.reserveSeatsIn carNumber 3
        
    actual =! Error (ReservationError CarNumberNotFound)