namespace Utils.Core

module Result =
    type ResultBuilder() =
        member this.Return(x) = Ok x
        member this.ReturnFrom(x) = x
        member this.Bind(x, f) = Result.bind f x
        
    let result = ResultBuilder()
    
    let (>>=) v f = Result.bind f v
    
    let toOption = function
        | Error _ -> None
        | Ok v -> Some v