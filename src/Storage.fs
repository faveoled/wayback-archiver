module Storage

open System
open System.Text.RegularExpressions
open Node

module Storage =

  let mutable dirsCreated = false

  let getStorageVal (key: string) : string option =
    if (Node.Exists("storage")) then
      dirsCreated <- true
    if not dirsCreated then
      Node.CreateDirectory("storage")
      dirsCreated <- true
    let path = $"storage/%s{key}.txt"
    if not (Node.Exists(path)) then
      None
    else
      let value = Node.ReadAllText(path)
      Some value

  let setStorageVal (key: string) (vall: string) : unit =
    if not dirsCreated then
      Node.CreateDirectory("storage") |> ignore
      dirsCreated <- true
    let path = $"storage/%s{key}.txt"
    Node.WriteAllText(path, vall)

  let setDelay (toSet: int) : unit =
    setStorageVal "DELAY" (toSet.ToString())

  let getDelay () : int =
    let item = getStorageVal "DELAY"
    match item with
    | Some x -> Int32.Parse(x)
    | None -> 15000

  let setTextToContain (text: string) : unit =
    setStorageVal "TEXT_CONTAIN" text

  let getTextToContain () : string =
    let item = getStorageVal "TEXT_CONTAIN"
    match item with
    | Some x -> x
    | None -> "google.com"

  let setTargetWebsite (url: string) : unit =
    setStorageVal "TARGET_WEBSITE" url

  let getTargetWebsite () : string =
    let item = getStorageVal "TARGET_WEBSITE"
    match item with
    | Some x -> x
    | None -> "https://google.com"

  let getKey (url: string) : string =
    let escaped = Regex.Replace(url, "[/:\.]", "_")
    $"URL_%s{escaped}"

  let isUrlDone (url: string) : bool =
    let item = getStorageVal (getKey url)
    match item with
    | Some _ -> true
    | None -> false

  let setUrlDone (url: string) : unit =
    setStorageVal (getKey url) "+"