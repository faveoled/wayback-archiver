module LinksProcessor

open Wayback
open Storage

module LinksProcessor =
  
  let runProcessing (urls: string list) (urlFilter: string) (delQuery: bool) =
    let filtered = urls
                   |> List.filter (fun s -> s.Contains urlFilter)
                   |> List.map (fun s ->
                     if (delQuery) then
                       if (s.Contains("?")) then
                         s.Substring(0, s.IndexOf('?'))
                       else
                        s
                     else
                       s
                   )
                   |> List.distinct
    let notDone = filtered |> List.filter (fun s -> not (Storage.isUrlDone s))
    printf $"links done previously: {filtered.Length - notDone.Length}"
    printf $"links to process: {notDone.Length}"

    promise {
      for url in notDone do
        printf $"processing link: {url}"

        let! hasSnapshotsPr = Wayback.hasSnapshots url
        match hasSnapshotsPr with
        | Ok hasSnapshots ->
          if not hasSnapshots then
            printf $"waiting for throttle slot..."
            do! Promise.sleep (Storage.getDelay ())
            let! savedPr = Wayback.saveNow url
            match savedPr with
            | Ok _ ->
              printf $"saved url {url}"
            | Error err ->
              printf $"couldn't save url {url}, reason: {err}"
            ()
          else
            printf $"available already: {url}"

        | Error errorValue ->
          printf $"couldn't check snapshot availability for {url}, reason: {errorValue}"
        printf $"waiting for throttle slot..."
        do! Promise.sleep (Storage.getDelay ())
    } |> Promise.start