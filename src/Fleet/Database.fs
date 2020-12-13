module Fleet.Database

open FSharp.Control.Tasks.V2
open FSharp.Data.LiteralProviders
open FSharp.Data.Npgsql

[<Literal>]
let private connectionStringForTypeCheck = TextFile<"Database.typecheck">.Text

type private DbFleet = NpgsqlConnection<connectionStringForTypeCheck>

let getAll (connectionString: string) =
    task {
        use cmd =
            DbFleet.CreateCommand<"
                SELECT id, brand, model, weight_in_tons, max_traction_in_tons
                FROM locomotives
                ">(connectionString)
        let! result = cmd.AsyncExecute()
        
        let locomotives =
            result
            |> Seq.map (fun r ->
                {
                    Id = r.id
                    Brand = r.brand
                    Model = r.model
                    Weight = r.weight_in_tons * 1m<ton>
                    MaxTraction = r.max_traction_in_tons * 1m<ton>
                })
            |> Seq.toList
        
        return locomotives
    }
    
let getById (connectionString: string) id =
    task {
        use cmd =            
            DbFleet.CreateCommand<"
                SELECT id, brand, model, weight_in_tons, max_traction_in_tons
                FROM locomotives
                WHERE id = @id
            ", SingleRow = true>(connectionString)
        
        let! dbLocomotive = cmd.AsyncExecute(id)
        
        let locomotive =
            dbLocomotive
            |> Option.map (fun r ->
                {
                    Id = r.id
                    Brand = r.brand
                    Model = r.model
                    Weight = r.weight_in_tons * 1m<ton>
                    MaxTraction = r.max_traction_in_tons * 1m<ton>
                })
        
        return locomotive
    }
    
let insert (connectionString: string) newLoco =
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
                decimal newLoco.Weight,
                decimal newLoco.MaxTraction)
            
        return ()
    }