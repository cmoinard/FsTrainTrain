module Reservations.Tests.Train.InitializationTests

open Reservations

open Xunit
open Swensen.Unquote

[<Fact>]
let ``Cannot create a train without car`` () =
    let actual = Train.init []
    
    actual =! Error (InitializationError EmptyCapacities)
    
[<Theory>]
[<InlineData(-1)>]
[<InlineData(0)>]
let ``Cannot create a train with a negative capacity`` capacity =
    let actual = Train.init [ capacity ]
    
    actual =! Error (InitializationError InitialCapacityBelowZero)
    
[<Fact>]
let ``Can create a valid train`` () =
    let actual =
        [ 100; 200 ]
        |> Train.init
        |> Result.map Train.cars
    
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