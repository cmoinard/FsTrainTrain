module Fleet.Locomotives.Web

open Fleet
open Fleet.Locomotives.Dto
open Giraffe
open Saturn
open Saturn.ControllerHelpers.Response

open Utils.Core.Validations.Validation

open FSharp.Control.Tasks.V2
open FSharp.Data

type AppSettings = JsonProvider<"appsettings.json">
let conf = AppSettings.Load("appsettings.json")
let connectionString = conf.ConnectionStrings.Fleet

let private getAll : HttpHandler =
    fun next ctx -> task {            
        let! dbLocomotives = Database.getAll connectionString
        
        let locomotives =
            dbLocomotives
            |> List.map LocomotiveDto.fromDbModel
        
        return! json locomotives next ctx
    }

let private getById id : HttpHandler =
    fun next ctx -> task {
        let! dbLocomotive = Database.getById connectionString id
        
        let locomotive =
            dbLocomotive
            |> Option.map LocomotiveDto.fromDbModel 
        
        return!
            match locomotive with
            | None -> notFound ctx ()
            | Some l -> json l next ctx
    }
    
let private create : HttpHandler =
    fun next ctx -> task {
        let! dto = ctx.BindJsonAsync<NewLocomotiveDto>()
        
        let newLoco =
            NewLocomotiveDto.create
                dto.Brand
                dto.Model
                dto.WeightInTons
                dto.MaxTractionInTons
        
        match newLoco with
        | Invalid errs ->
            let problem = CreationError.problem errs                        
            return! badRequest ctx problem
            
        | Valid dto ->
            let dbLocomotive = NewLocomotiveDto.toDbModel dto
            
            do! Database.create connectionString dbLocomotive
           
            return! ok ctx next
    }
    
let router =
    router {
        get "" getAll
        getf "/%i" getById
        
        post "" create
    }