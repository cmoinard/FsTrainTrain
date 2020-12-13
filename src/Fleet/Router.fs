module Fleet.Router

open Saturn
open Utils.Web.JsonHelpers
open FSharp.Data

type Appsettings = JsonProvider<"appsettings.json">

let locomotive =    
    let conf = Appsettings.Load("appsettings.json")
    
    let getAll =
        Database.getAll conf.ConnectionStrings.Fleet
        |> toJson'
    
    router {
        get "" getAll
    }