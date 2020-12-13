module Utils.Web.JsonHelpers

open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Microsoft.AspNetCore.Http
open Saturn
open Giraffe
    
let toJson (get: unit -> Task<'b>) (_: 'c) ctx =
    task {
        let! v = get ()
        return! Controller.json ctx v
    }
    
let toJson' (t: Task<'b>) (_: 'c) ctx =
    task {
        let! v = t
        return! Controller.json ctx v
    }

let withJsonBody (execute: 'a -> Task<'b>) (_: 'c) (ctx: HttpContext) =
    task {
        let! model = ctx.BindJsonAsync<'a>()
        let! response = execute model
        return! Controller.json ctx response
    }