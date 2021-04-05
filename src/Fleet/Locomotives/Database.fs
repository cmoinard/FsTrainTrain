module Fleet.Locomotives.Database

open FSharp.Control.Tasks.V2

open Fleet.Database
open Fleet.Locomotives.DbModels

let getAll connectionString =
    task {    
        use cmd =
            DbFleet.CreateCommand<"
                SELECT id, brand, model, weight_in_tons, max_traction_in_tons
                FROM locomotives
                ">(connectionString)
        let! result = cmd.AsyncExecute()
        
        return
            result
            |> Seq.map (fun r ->
                {
                    Id = r.id
                    Brand = r.brand
                    Model = r.model
                    WeightInTons = r.weight_in_tons
                    MaxTractionInTons = r.max_traction_in_tons
                })
            |> Seq.toList
    }
    
let getById connectionString id =
    task {        
        use cmd =
            DbFleet.CreateCommand<"
                SELECT id, brand, model, weight_in_tons, max_traction_in_tons
                FROM locomotives
                WHERE id = @id
            ", SingleRow = true>(connectionString)
        
        let! dbLocomotive = cmd.AsyncExecute(id)
        
        return
            dbLocomotive
            |> Option.map (fun r ->
                {
                    Id = r.id
                    Brand = r.brand
                    Model = r.model
                    WeightInTons = r.weight_in_tons
                    MaxTractionInTons = r.max_traction_in_tons
                })
    }
    
let create connectionString newLoco =
    task {
        use cmd =
            DbFleet.CreateCommand<"
                INSERT INTO locomotives (brand, model, weight_in_tons, max_traction_in_tons)
                VALUES (@brand, @model, @weight, @max_traction)
            ">(connectionString)

        let! _ =
            cmd.AsyncExecute(
                newLoco.Brand,
                newLoco.Model,
                decimal newLoco.WeightInTons,
                decimal newLoco.MaxTractionInTons)
        
        return ()
    }
           