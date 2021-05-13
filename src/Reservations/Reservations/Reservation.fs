module Reservations.Reservation

type TravelId = TravelId of int
type SeatsToReserve = int

type Reservation = Reservation



type ReservationError =
    | TravelNotFound
    | OccupationThresholdExceeded

type Reserve = TravelId -> SeatsToReserve -> Async<Result<Reservation, ReservationError>>


let reserve
    (getTrainOfTravel: TravelId -> Async<Train option>)
    : Reserve =
    fun travelId seats ->
        async {
            let! trainOption = getTrainOfTravel travelId
            
            match trainOption with
            | None ->
                return Error TravelNotFound
            | Some train ->
                return Error OccupationThresholdExceeded
        }
        