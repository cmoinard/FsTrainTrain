module Fleet.Database

open FSharp.Data.LiteralProviders
open FSharp.Data.Npgsql

[<Literal>]
let private connectionStringForTypeCheck = TextFile<"Database.typecheck">.Text
type DbFleet = NpgsqlConnection<connectionStringForTypeCheck>