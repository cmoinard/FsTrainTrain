module Utils.Core.Numbers

let (|Negative|StrictlyPositive|) n =
    if n <= 0 then
        Negative
    else
        StrictlyPositive
    
