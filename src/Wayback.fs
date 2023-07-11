module Wayback

open System.Collections.Generic
open URLEncoder

type Wayback =
    static member hasSnapshots (url: string) : Fable.Core.JS.Promise<Result<bool, string>> =
        let encodedUrl = URLEncoder.urlEncode(url)
        ApiClient.fetchGet $"https://archive.org/wayback/available?url=%s{encodedUrl}"
        |> Promise.map (fun resp ->
            resp.Contains "\"timestamp\""
        )
        |> Promise.result

    static member saveNow (url: string): Fable.Core.JS.Promise<Result<unit, string>> =
        printfn $"posting link %s{url}"
        let headers: IDictionary<string,string> = dict [ "Content-Type", "application/x-www-form-urlencoded" ]
        ApiClient.fetchPost
            $"https://web.archive.org/save/%s{url}"
            headers
            $"url=%s{URLEncoder.urlEncode(url)}"
        |> Promise.map (fun resp -> ())
        |> Promise.result
        