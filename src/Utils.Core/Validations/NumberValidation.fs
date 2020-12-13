module Utils.Core.Validations.NumberValidation

open Utils.Core.Validations.Validation

let inline strictlyHigherThan min error v =
    if v <= min then
        Invalid [ error ]
    else
        Valid v
    