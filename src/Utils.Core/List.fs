module Utils.Core.List

let replaceWhen predicate replace list =
    list
    |> List.map (fun x ->
        if predicate x then
            replace x
        else
            x)