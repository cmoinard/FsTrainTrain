module Fleet.UseCases

open FSharp.Control.Tasks.V2

let dummyLocomotives () =
    task {
        do! Async.Sleep 200
        return [
            {
                Id = 1
                Brand = "SuperMotive"
                Model = "Loco X1"
                WeightInTons = 100m<ton>
                MaxTractionInTons = 2_000m<ton>
            }
        ]
    }
    
//module Locomotive =
//    let private createNewLocomotive brand model weight maxTraction =
//        {
//            Brand = brand
//            Model = model
//            WeightInTons = weight * 1m<ton>
//            MaxTractionInTons = maxTraction * 1m<ton>
//        }
//    
//    let createNewLocomotive brand model weight maxTraction =
//        let isLowerThan50 s =