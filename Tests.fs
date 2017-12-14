module Tests

open System
open Xunit
open Swensen.Unquote
open DataTypes

let add circuit el = Array.append circuit [| el |]
let startCircuit (pin: PowerPin) = 
    pin |> PowerPin |> GpioPin |> add [||] |> ForCabel
let addCabel (ForCabel circuit) = 
    add circuit Cabel |> ForBreadBoardPosition
[<Fact>]
let ``simple circle compile`` () =
    let (ForBreadBoardPosition circuit) = 
        startCircuit PowerPin2 
        |> addCabel
    //    |> addBreadBoard (HorTop 3)
    circuit =! [| PowerPin2 |> PowerPin |> GpioPin; Cabel |]
