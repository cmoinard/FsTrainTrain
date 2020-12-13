module Fleet.Migrations.Migration

open Fleet.Migrations
open Npgsql
open FSharp.Data
open SimpleMigrations
open SimpleMigrations.DatabaseProvider

type Appsettings = JsonProvider<"appsettings.json">

let migrate () =
    try
        let conf = Appsettings.Load("appsettings.json")    
        use connection = new NpgsqlConnection(conf.ConnectionStrings.Fleet)
        
        let provider = PostgresqlDatabaseProvider(connection)
        let migrator = SimpleMigrator(typeof<Initial>.Assembly, provider)
        
        migrator.Load()
        migrator.MigrateToLatest()
    with
        | e -> printfn "%s" e.Message 
    
    