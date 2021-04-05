module Reservations.Train.InitializationTests

open Reservations
open Xunit
open Swensen.Unquote

[<Fact>]
let ``Cannot create a train without car`` () =
    let actual = init []
    
    actual =! Error EmptyCapacities
    
[<Theory>]
[<InlineData(-1)>]
[<InlineData(0)>]
let ``Cannot create a train with a negative capacity`` capacity =
    let actual = init [ capacity ]
    
    actual =! Error InitialCapacityBelowZero
    
[<Fact>]
let ``Can create a valid train`` () =
    let actual =
        [ 100; 200 ]
        |> init
        |> Result.map cars
    
    let expectedCars =
        [
            { Number = CarNumber 1
              Capacity = 100
              ReservedSeats = 0 }
            
            { Number = CarNumber 2
              Capacity = 200
              ReservedSeats = 0 }
        ]
        
    actual =! Ok expectedCars