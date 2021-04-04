module Fleet.HttpHandlers.LocomotiveCreation

open Fleet

open Utils.Core.Validations.Validation
open Utils.Core.Validations.NumberValidation
open Utils.Core.Validations.StringValidation
open Giraffe
open Saturn
open Saturn.ControllerHelpers.Response
open Utils.Web.ErrorDto
open FSharp.Control.Tasks.V2

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
    | WeightShouldBeStrictlyPositive
    | MaxTractionShouldBeStrictlyPositive
    
module CreationError =
    let mapError = function                        
        | BrandIsRequired ->
            { PropertyName = "Brand" ; Title = "Required" }
        | ModelIsRequired ->
            { PropertyName = "Model" ; Title = "Required" }
        | WeightShouldBeStrictlyPositive ->
            { PropertyName = "Weight" ; Title = "StrictlyPositive" }
        | MaxTractionShouldBeStrictlyPositive ->
            { PropertyName = "MaxTraction" ; Title = "StrictlyPositive" }
        
    let problem errs =
        errs
        |> List.map mapError
        |> toProblemDetails
    
    
module NewLocomotiveDto =
    let create brand model weight maxTraction =        
        let validateBrand = isNotEmpty BrandIsRequired
        let validateModel = isNotEmpty ModelIsRequired
        let validateWeight = strictlyHigherThan 0m WeightShouldBeStrictlyPositive
        let validateMaxTraction = strictlyHigherThan 0m MaxTractionShouldBeStrictlyPositive
        
        validation {
            let! brand' = validateBrand brand
            and! model' = validateModel model
            and! weight' = validateWeight weight
            and! maxTraction' = validateMaxTraction maxTraction
            
            return {
                Brand = brand'
                Model = model'
                Weight = weight' * 1m<ton>
                MaxTraction = maxTraction' * 1m<ton>
            }
        }

let create connStr : HttpHandler =
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
                let problem = errs |> CreationError.problem                        
                badRequest ctx problem
                
            | Valid l ->
                task {
                    do! Database.insert connStr l
                    return! ok ctx next
                }
    }
