module Node

open Fable.Core
open Node.Api

module Node =

    let WriteAllText(path: string, body: string) =
        fs.writeFileSync(path, body)

    let ReadAllText(path: string): string =
        let buf = fs.readFileSync(path)
        buf.ToString()
        
    let CreateDirectory(path: string) =
        fs.mkdirSync(path)
        
    let Exists(path: string) =
        fs.existsSync(U2.Case1(path))
        
    let ReadDir(path: string) =
        let arr = fs.readdirSync(U2.Case1(path))
        arr
