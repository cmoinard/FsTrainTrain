module Utils.Core.Validation

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


let (&&&) f1 f2 v =
    match f1 v, f2 v with
    | Invalid errs1, Invalid errs2 -> Invalid (errs1 @ errs2)
    | Invalid errs, _ | _, Invalid errs -> Invalid errs
    | _ -> Valid v
    
let (|||) f1 f2 v =
    match f1 v, f2 v with
    | Invalid errs1, Invalid errs2 -> Invalid (errs1 @ errs2)
    | _ -> Valid v

