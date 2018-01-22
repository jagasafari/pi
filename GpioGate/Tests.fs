module Tests

open System
open Xunit
open Swensen.Unquote
open Random    
open Led
open CmdBuilder
open Stamp

[<Fact>]
let ``rndTimesStream: `` () =
    let stream, stop = rndTimesStream ()
    stop ()

[<Fact>]
let ``cmdBuilder: add then create`` () =
    let add, create  = cmdBuilder (fun () -> 0) ()
    add (0, [||])
    add (34, [|1|])
    create () =! ([|0; 0; 34|], [|1|])

let ``getStamp: increment`` () =
    let getStamp = stamp ()
    getStamp () =! 1
    getStamp () =! 2
    getStamp () =! 3

[<Fact>]
[<Trait("Category", "Integration")>]
let ``toggleRgbLed: changes colors`` () =
    let builder = cmdBuilder (fun () -> 0)
    let nxtRgbState = rndChangeRgbLed builder 0 1 2
    let a = nxtRgbState () 
    let b = nxtRgbState ()
    a = b =! false

[<Fact>]
[<Trait("Category", "Integration")>]
let ``rndInt: next randoms not equal`` () =
    let rndFromRange () = 
        let get = rndInt ()
        get 0 100000
    let a = rndFromRange ()
    let b = rndFromRange ()
    let c = rndFromRange ()
    (a = b) =! false
    (c = b) =! false
    (c = a) =! false
