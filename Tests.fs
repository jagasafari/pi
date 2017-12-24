module Tests

open Xunit
open DataTypes
open Swensen.Unquote
open Validate

[<Fact>]
let ``modeling simple circuit`` () =
    let circuit =
        [
        Cabel (PowerPin (5.0, 2), TopHor 3)
        Led (TopHor 5, TopGround 7)
        Cabel (TopGround 9, TopGround 5)
        ] |> List.toSeq
    circuit |> build
    =! circuit

[<Fact>]
let ``validaiting element: PowerPin`` () =
    PowerPin (5.0, 2) |> buildElement  
    =! (PowerPin (5.0, 2))

[<Theory>]
[<InlineData(5.0, 2)>]
[<InlineData(5.0, 4)>]
[<InlineData(3.0, 1)>]
[<InlineData(3.0, 17)>]
let ``getPowerPinErr: valid cases`` 
    volts num =
    getPowerPinErr (volts, num) =! None
    
    PowerPin (volts, num) |> buildElement
    =! (PowerPin (volts, num))

[<Theory>]
[<InlineData(4.5, 4)>]
[<InlineData(5.0, 9)>]
[<InlineData(3.0, 3)>]
let ``getPowerPinErr: invalid cases`` volts num =
    getPowerPinErr (volts, num) 
    =! (NotGpioPowerPin (volts, num) |> Some)

    (PowerPin (volts, num) |> buildElement)
    =! (Err (NotGpioPowerPin (volts, num)))

[<Theory>]
[<InlineData(6)>]
[<InlineData(30)>]
let ``getGroundPinErr: valid pin`` n =
    getGroundPinErr n =! None
    GroundPin n |> buildElement 
    =! (GroundPin n)

[<Theory>]
[<InlineData(1)>]
[<InlineData(100)>]
let ``getGroundPinErr: not ground pin`` n =
    getGroundPinErr n =! (Some (NotGroundPin n))
    GroundPin n |> buildElement 
    =! (Err (NotGroundPin n))
