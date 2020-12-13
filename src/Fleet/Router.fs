module Fleet.Router

open Saturn
open Utils.Web.JsonHelpers
open FSharp.Data
open Fleet

type Appsettings = JsonProvider<"appsettings.json">

let locomotive =    
    let conf = Appsettings.Load("appsettings.json")
    let connStr = conf.ConnectionStrings.Fleet
    
    let getAll =
        Database.getAll connStr
        |> toJson'
        
    // TODO : How to convert Option<> to NotFound ?
    let getById id =
        Database.getById connStr id
        |> toJson'
    
    router {
        get "" getAll
        getf "/%i" getById
    }