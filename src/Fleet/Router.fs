module Fleet.Router

open Giraffe
open Saturn
open Saturn.ControllerHelpers.Response
open Utils.Web.JsonHelpers
open FSharp.Data
open FSharp.Control.Tasks.V2

type Appsettings = JsonProvider<"appsettings.json">

// reminder        
// type HttpFuncResult = Task<HttpContext option>
// type HttpFunc = HttpContext -> HttpFuncResult
// type HttpHandler = HttpFunc -> HttpContext -> HttpFuncResult

let locomotive =    
    let conf = Appsettings.Load("appsettings.json")
    let connStr = conf.ConnectionStrings.Fleet
    
    let getAll : HttpHandler =
        Database.getAll connStr
        |> toJson'
        
    let getById id : HttpHandler =
        fun next ctx -> task {
            let! locomotive = Database.getById connStr id
            
            return!
                match locomotive with
                | None -> notFound ctx ()
                | Some l -> json l next ctx
        }
    
    router {
        get "" getAll
        getf "/%i" getById
    }
    