module Reservations.Tests.ReservationTests

open Reservations
open Reservations.Reservation
open Swensen.Unquote
open Xunit
open Utils.Core.Result

[<Fact>]
let ``Should return NotFound when no train associated with TravelId`` () =
    let getTrain _ =
        async {
            return None
        }
        
    let travelId = TravelId 1
    async {
        let reserve = Reservation.reserve getTrain
        let! actual = reserve travelId 3
        
        actual =! Error TravelNotFound
    }

[<Fact>]
let ``Should return OccupationThresholdExceeded`` () =
    let getTrain _ =
        async {
            let carNumber = CarNumber 1
            return
                Train.init [ 100 ]
                >>= Train.reserveSeatsIn carNumber 70
                |> toOption
        }
        
    let travelId = TravelId 1
    async {
        let reserve = Reservation.reserve getTrain
        let! actual = reserve travelId 3
        
        actual =! Error OccupationThresholdExceeded
    }