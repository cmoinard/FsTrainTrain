module Utils.Core.Validations.Validation

type Validation<'value, 'error> =
    | Valid of 'value
    | Invalid of 'error list
    
let map (f: 'a -> 'b) (v:Validation<'a,'error>) : Validation<'b, 'error> =
    match v with
    | Invalid errors ->  Invalid errors
    | Valid value -> Valid (f value)

let (<!>) = map

let map2 f v1 v2 =
    match v1, v2 with
    | Invalid errs1, Invalid errs2 -> Invalid (errs1 @ errs2)
    | Invalid errs, _ | _, Invalid errs -> Invalid errs
    | Valid value1, Valid value2 -> Valid (f value1 value2)
    
let apply f v = map2 id f v
let (<*>) = apply

let bind f v =
    match v with
    | Invalid errs -> Invalid errs
    | Valid v' -> f v'

let (&&&) f1 f2 v =
    match f1 v, f2 v with
    | Invalid errs1, Invalid errs2 -> Invalid (errs1 @ errs2)
    | Invalid errs, _ | _, Invalid errs -> Invalid errs
    | _ -> Valid v
    
let (|||) f1 f2 v =
    match f1 v, f2 v with
    | Invalid errs1, Invalid errs2 -> Invalid (errs1 @ errs2)
    | _ -> Valid v

type ValidationBuilder() =
    member __.Return(x) = Valid x
    member __.ReturnFrom(x) = x
    
    member __.Bind(x, f) = bind f x
    
    member __.MergeSources(v1, v2) =
        match v1, v2 with
        | Valid v1', Valid v2' -> Valid (v1', v2')
        | Invalid errs, _ -> Invalid errs
        | _, Invalid errs -> Invalid errs
        
    member __.BindReturn(x, f) = map f x


let validation = ValidationBuilder()