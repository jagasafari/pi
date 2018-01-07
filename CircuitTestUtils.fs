module CircuitTestUtils

open Swensen.Unquote
open DataTypes

let testCircuit build circuit =
    Seq.zip (build circuit)
    >> Seq.iter (fun (x, y) -> x =! y)

let validCircuit =
    [
    CabelIn (PowerPin (3.0, 1))
    CabelOut (TopHor 4)
    LedKatode (TopHor 6)
    LedAnode (TopGround 8)
    CabelOut (TopGround 10)
    CabelIn (GroundPin 6)
    ]
