module Stamp

open System.Threading

let stamp =
    let mutable count = 0
    let get () = 
        Interlocked.Increment &count |> ignore
        count
    get

