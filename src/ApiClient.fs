module ApiClient

open System.Collections.Generic
open Fetch

let fetchGet (url: string): Fable.Core.JS.Promise<string> =
    let p1 = 
        fetch url [
          requestHeaders [
              HttpRequestHeaders.ContentType "application/json"
          ] ]
    let p2 =
        p1
        |> Promise.bind (fun response ->
            response.text()
        )
    p2
    
    
let fetchPost (url: string) (headers: IDictionary<string,string>) (body: string): Fable.Core.JS.Promise<string> =
    let p1 = 
        fetch url [
          RequestProperties.Method HttpMethod.POST
          RequestProperties.Body (BodyInit.Case3 body)
          requestHeaders (headers |> Seq.map (fun kvp -> HttpRequestHeaders.Custom(kvp.Key, kvp.Value)) |> List.ofSeq)
        ]
    let p2 =
        p1
        |> Promise.bind (fun response ->
            response.text()
        )
    p2
