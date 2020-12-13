module Utils.Core.Validations.StringValidation

open System
open Utils.Core.Validations.Validation

let inline isNotEmpty error v =
    if String.IsNullOrWhiteSpace(v) then
        Invalid [ error ]
    else
        Valid v
    