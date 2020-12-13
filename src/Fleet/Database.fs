module Fleet.Database

open FSharp.Control.Tasks.V2
open FSharp.Data.LiteralProviders
open FSharp.Data.Npgsql

[<Literal>]
let private connectionStringForTypeCheck = TextFile<"Database.typecheck">.Text

type private DbFleet = NpgsqlConnection<connectionStringForTypeCheck, ReuseProvidedTypes = true>

let getAll (connectionString: string) =
    task {
        use cmd =
            DbFleet.CreateCommand<
                "SELECT id, brand, model, weight_in_tons, max_traction_in_tons
                FROM locomotives"
                >(connectionString)
        let! result = cmd.AsyncExecute()
        
        let locomotives =
            result
            |> Seq.map (fun r ->
                {
                    Id = r.id
                    Brand = r.brand
                    Model = r.model
                    WeightInTons = r.weight_in_tons * 1m<ton>
                    MaxTractionInTons = r.max_traction_in_tons * 1m<ton>
                })
            |> Seq.toList
        
        return locomotives
    }
    