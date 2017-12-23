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
    =! (Choice1Of2 circuit)

[<Fact>]
let ``validaiting element`` () =
    PowerPin (5.0, 2) |> validateElement  
    =! None

[<Theory>]
[<InlineData(5.0, 2)>]
let ``validatePowerPin`` volts num =
    validatePowerPin (volts, num)
    =! None
