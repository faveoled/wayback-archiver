open System
open Node.Api

open LinkExtractor
open LinksProcessor

let args = process.argv
if (args.Count <> 5) then
    printf $"program arguments: node app.js <link for source html> <links filter string> <whether to delete query params (bool)"
    raise (Exception("Wrong arguments passed to the program"))

let response: Fable.Core.JS.Promise<string> = ApiClient.fetchGet (args[2])
response
    |> Promise.result
    |> Promise.iter (fun response ->
        match response with
        | Ok html ->
            let links = LinkExtractor.extractLinks html
            for link in links do
                printf $"{link}"
            LinksProcessor.runProcessing links args[3] (bool.Parse(args[4]))
        | Error err ->
            printf $"couldn't read links source: {err}"
    )