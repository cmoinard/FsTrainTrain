module Reservations.Tests.ReservationTests

open Reservations
open Reservations.Reservation
open Swensen.Unquote
open Xunit

[<Fact>]
let ``Should return NotFound when no train associated with TravelId`` () =
    let getTrain _ =
        async {
            return (Error TravelNotFound)
        }
        
    let travelId = TravelId 1
    async {
        let reserve = Reservation.reserve getTrain
        let! actual = reserve travelId 3
        
        actual =! Error TravelNotFound
    }