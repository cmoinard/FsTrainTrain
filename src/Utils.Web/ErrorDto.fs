module Utils.Web.ErrorDto

open Microsoft.AspNetCore.Mvc

type ErrorDto = {
    PropertyName: string
    Title: string
}

let toProblemDetails errors =
    errors
    |> List.groupBy (fun e -> e.PropertyName)
    |> List.map (fun (key, values) ->
        let titles =
            values
            |> List.map (fun e -> e.Title)
            |> List.toArray
        key, titles)
    |> dict
    |> ValidationProblemDetails
