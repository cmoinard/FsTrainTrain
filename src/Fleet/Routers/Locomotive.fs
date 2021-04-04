module Fleet.Routers.Locomotive

open Fleet
open Fleet.HttpHandlers
open Giraffe
open Saturn
open Saturn.ControllerHelpers.Response
open Utils.Web.JsonHelpers
open FSharp.Control.Tasks.V2
open FSharp.Data

// reminder        
// type HttpFuncResult = Task<HttpContext option>
// type HttpFunc = HttpContext -> HttpFuncResult
// type HttpHandler = HttpFunc -> HttpContext -> HttpFuncResult

type Appsettings = JsonProvider<"appsettings.json">

    
let private getAll connStr : HttpHandler =
    Database.getAll connStr
    |> toJson'
    
let private getById connStr id : HttpHandler =
    fun next ctx -> task {
        let! locomotive = Database.getById connStr id
        
        return!
            match locomotive with
            | None -> notFound ctx ()
            | Some l -> json l next ctx
    }
    
let router =
    let conf = Appsettings.Load("appsettings.json")
    let connStr = conf.ConnectionStrings.Fleet
    
    router {
        get "" (getAll connStr)
        getf "/%i" (getById connStr)
        
        post "" (LocomotiveCreation.create connStr)
    }
    