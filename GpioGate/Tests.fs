module Tests

open System
open Xunit
open Random    

[<Fact>]
let ``nextRnd: `` () =
    let nxt = rndMilliSec ()
    [0 .. 10]
    |> List.map (fun _ -> nxt 1000 3000)
    |> ignore

[<Fact>]
let ``rndTimesStream: `` () =
    let stream, stop = rndTimesStream ()
    ()
