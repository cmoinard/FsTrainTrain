module Fleet.Locomotives.Dto

open Fleet.Locomotives.DbModels
open Utils.Core.Validations.Validation
open Utils.Core.Validations.NumberValidation
open Utils.Core.Validations.StringValidation
open Utils.Web.ErrorDto

type LocomotiveDto =
    {
        Id: int
        Brand: string
        Model: string
        WeightInTons: decimal
        MaxTractionInTons: decimal
    }
    
module LocomotiveDto =
    let fromDbModel (dbModel: DbModels.DbLocomotive) =
        {
            Id = dbModel.Id
            Brand = dbModel.Brand
            Model = dbModel.Model
            WeightInTons = dbModel.WeightInTons
            MaxTractionInTons = dbModel.MaxTractionInTons
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


type NewLocomotiveDto =
    {
        Brand: string
        Model: string
        WeightInTons: decimal
        MaxTractionInTons: decimal
    }
    
module NewLocomotiveDto =
    let toDbModel dto : DbNewLocomotive =
        {
            Brand = dto.Brand 
            Model = dto.Model 
            WeightInTons = dto.WeightInTons 
            MaxTractionInTons = dto.MaxTractionInTons 
        }
        
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
                WeightInTons = weight'
                MaxTractionInTons = maxTraction'
            }
        }