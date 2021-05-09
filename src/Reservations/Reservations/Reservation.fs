module Reservations.Reservation

type TravelId = TravelId of int
type SeatsToReserve = int

type Reservation = Reservation

type ReservationError =
    | TravelNotFound

type Reserve = TravelId -> SeatsToReserve -> Async<Result<Reservation, ReservationError>>


let reserve
    (getTrainOfTravel: TravelId -> Async<Result<Train, ReservationError>>)
    : Reserve =
    fun travelId seats ->
        async {
            return Error TravelNotFound
        }
        