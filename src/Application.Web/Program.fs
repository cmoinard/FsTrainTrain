open Giraffe
open Saturn
open Utils.Web.JsonHelpers
open FSharp.Control.Tasks.V2

let migrate () =
    task {
        Fleet.Migrations.Migration.migrate ()
        return ()
    }

let mainRouter = router {
    get "/" (text "Hello World from Saturn")
    
    get "/maj" (migrate |> toJson)
    
    forward "/locomotives" Fleet.Router.locomotive
}

let app = application {
    use_router mainRouter
}

run app