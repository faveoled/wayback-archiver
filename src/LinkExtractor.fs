module LinkExtractor

module LinkExtractor =

    let extractLinks (html: string): string list =
        html.Split("\"")
        |> Array.filter (fun x -> x.StartsWith("http://") || x.StartsWith("https://"))
        |> Array.distinct
        |> Array.toList
