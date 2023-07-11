module URLEncoder

module URLEncoder =

    let toBytes (str: string): byte[] =
      let utf8 = ResizeArray()
      let mutable i = 0
      while i < str.Length do
        let mutable charCode = int str.[i]
        if charCode < 0x80 then utf8.Add(charCode)
        elif charCode < 0x800 then
          utf8.Add(0xc0 ||| (charCode >>> 6))
          utf8.Add(0x80 ||| (charCode &&& 0x3f))
        elif charCode < 0xd800 || charCode >= 0xe000 then
          utf8.Add(0xe0 ||| (charCode >>> 12))
          utf8.Add(0x80 ||| ((charCode >>> 6) &&& 0x3f))
          utf8.Add(0x80 ||| (charCode &&& 0x3f))
        else
          i <- i + 1
          // Surrogate pair:
          // UTF-16 encodes 0x10000-0x10FFFF by subtracting 0x10000 and
          // splitting the 20 bits of 0x0-0xFFFFF into two halves
          charCode <- 0x10000 + (((charCode &&& 0x3ff) <<< 10) ||| (int str.[i] &&& 0x3ff))
          utf8.Add(0xf0 ||| (charCode >>> 18))
          utf8.Add(0x80 ||| ((charCode >>> 12) &&& 0x3f))
          utf8.Add(0x80 ||| ((charCode >>> 6) &&& 0x3f))
          utf8.Add(0x80 ||| (charCode &&& 0x3f))
        
        i <- i + 1
      
      utf8.ToArray() |> Array.map byte
      

    let toHex (c: char): string =
      let bytes = System.BitConverter.GetBytes(c)
      let hex = System.String.Concat(bytes |> Array.take 1 |> Array.map (fun b -> b.ToString("X2")))
      hex

    // A function to URL encode a string in F#
    let urlEncode (s: string): string =
        System.Uri.EscapeDataString(s).Replace("%20", "+")