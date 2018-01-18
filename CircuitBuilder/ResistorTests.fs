module ResistorTests

open Xunit
open Swensen.Unquote
open Resistor

[<Fact>]
let ``multiplier: yellow`` () =
    Yellow |> multiplier =! 10000.0

[<Fact>]
let ``tolerance: gold`` () =
    Gold |> tolerance =! 0.05

[<Fact>]
let ``resistor:`` () =
    [Grey; Red; Orange; Gold]
    |> resistor =! (Resistor (82000.0, 0.05, None))
    
