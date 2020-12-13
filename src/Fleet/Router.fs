module Fleet.Router

open Utils.Core.Validations.Validation
open Utils.Core.Validations.NumberValidation
open Utils.Core.Validations.StringValidation
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

type NewLocomotiveDto =
    {
        Brand: string
        Model: string
        WeightInTons: decimal
        MaxTractionInTons: decimal
    }
    
type CreationError =
    | BrandIsRequired
    | ModelIsRequired
    | WeightShouldBeStrictyPositive
    | MaxTractionShouldBeStrictlyPositive
    
module NewLocomotiveDto =
    let create brand model weight maxTraction =        
        validation {
            let! brand' = brand |> isNotEmpty BrandIsRequired 
            and! model' = model |> isNotEmpty ModelIsRequired
            and! weight' = weight |> strictlyHigherThan 0m WeightShouldBeStrictyPositive
            and! maxTraction' = maxTraction |> strictlyHigherThan 0m MaxTractionShouldBeStrictlyPositive 
            
            return {
                Brand = brand'
                Model = model'
                Weight = weight' * 1m<ton>
                MaxTraction = maxTraction' * 1m<ton>
            }
        }

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
        
    let create : HttpHandler =
        fun next ctx -> task {
            let! dto = ctx.BindJsonAsync<NewLocomotiveDto>()
            
            let newLoco =
                NewLocomotiveDto.create
                    dto.Brand
                    dto.Model
                    dto.WeightInTons
                    dto.MaxTractionInTons
            
            return!
                match newLoco with
                | Invalid errs ->
                    badRequest ctx errs
                    
                | Valid l ->
                    task {
                        do! Database.insert connStr l
                        return! ok ctx next
                    }
        }        
    
    router {
        get "" getAll
        getf "/%i" getById
        
        post "" create
    }
    