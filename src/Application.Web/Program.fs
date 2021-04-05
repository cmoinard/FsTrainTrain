open Giraffe
open Saturn
open Utils.Web.JsonHelpers
open FSharp.Control.Tasks.V2

// reminder        
// type HttpFuncResult = Task<HttpContext option>
// type HttpFunc = HttpContext -> HttpFuncResult
// type HttpHandler = HttpFunc -> HttpContext -> HttpFuncResult

let migrate () =
    task {
        Fleet.Migrations.Migration.migrate ()
        return ()
    }

let mainRouter = router {
    get "/" (text "Hello World from Saturn")
    
    get "/maj" (migrate |> toJson)
    
    forward "/locomotives" Fleet.Locomotives.Web.router
}

let app = application {
    use_router mainRouter
}

run app