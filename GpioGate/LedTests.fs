module LedTests

open System
open Xunit
open Swensen.Unquote
open Led
open CmdBuilder

let builder () = cmdBuilder (fun () -> 3)

[<Fact>]
let ``led: toogle few times`` () =
    let toggle = led builder 7
    toggle () =! (([|3; 7; 6|], [||]), On)
    toggle () =! (([|3; 7|], [||]), Off)

[<Fact>]
[<Trait("Category", "Integration")>]
let ``toggleRgbLed: changes colors`` () =
    let nxtRgbState = rndChangeRgbLed builder 0 1 2
    let a = nxtRgbState () 
    let b = nxtRgbState ()
    a = b =! false
