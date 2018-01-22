module Stamp

open System
open System.Threading

let stamp () =
    let mutable count = 0
    let get () = 
        Interlocked.Increment &count |> ignore
        if count = Int32.MaxValue then
            Interlocked.Exchange(&count, 0) |> ignore
        count
    get

