module Tests

open Xunit
open System.Collections.Generic
open Swensen.Unquote
open CircuitTestUtils
open DataTypes
open Validate

let testBuild = testCircuit build

[<Fact>]
let ``buildZeroValidation:`` () =
    let test = testCircuit buildZeroValidation
    test validCircuit validCircuit

[<Fact>]
let ``reservePosition`` () =
    let add, count = getSet<Element> ()
    let simple = TopHor 3
    let led = LedAnode simple
    let cabel = CabelIn (TopHor 4)
    let reserve = reservePosition add
    reserve led =! true 
    reserve simple =! false 
    reserve simple =! false 
    reserve cabel =! true 
    reserve cabel =! false 
    count() =! 2

[<Fact>]
let ``circuit: position already taken error`` () =
    let circuit =
        [
        CabelIn (PowerPin (3.0, 1))
        CabelOut (TopHor 4)
        LedKatode (TopHor 4)
        LedAnode (TopGround 8)
        CabelOut (TopGround 10)
        CabelIn (GroundPin 6)
        ]
    let pos = BottomVer (2, 'H')
    let expected =
        [
        CabelIn (PowerPin (3.0, 1))
        CabelOut (TopHor 4)
        Err (PositionAlreadyTaken, LedKatode (TopHor 4))
        LedAnode (TopGround 8)
        CabelOut (TopGround 10)
        CabelIn (GroundPin 6)
        ]
    testBuild circuit expected

let add _ = true

[<Fact>]
let ``CabelIn: successfull build`` () =
    let e = CabelIn (PowerPin (5.0, 2))
    buildElement add e =! e

[<Fact>]
let ``CabelIn: failuare build`` () =
    let e = CabelIn (TopHor 8) 
    buildElement add e =! (Err (NotGpioPin, e))

[<Fact>]
let ``CabelIn: wrong pin number`` () =
    let e = CabelIn (GroundPin 0)
    buildElement add e =! 
    ((NotGroundPinParameters, e) |> Err)

let testBreadBoardPos f =
    [1 .. 63] |> Seq.iter f

[<Fact>]
let ``buildBreadBoardVertical: valid positions`` () =
    let test position (x, y) = 
        let element = CabelOut (position (x, y)) 
        element |> buildElement add
        =! element

    fun x -> 
        let run e = Seq.iter (fun y -> test e (x, y)) 
        run TopVer ['A'..'E']
        run BottomVer ['F'..'J']
    |> testBreadBoardPos

[<Fact>]
let ``modeling simple circuit`` () =
    let c = CabelIn (TopGround 9)
    let c2 = CabelOut (TopGround 5)
    let circuit =
        [
        CabelIn (PowerPin (5.0, 2))
        CabelOut (TopHor 3)
        LedAnode (TopHor 5)
        LedKatode (TopGround 7)
        c
        c2 
        ]
    let expected = 
        [
        CabelIn (PowerPin (5.0, 2))
        CabelOut (TopHor 3) 
        LedAnode (TopHor 5)
        Err (MinusChargeNotToGround, 
                            LedKatode (TopGround 7))
        Err (NotGpioPin, c) 
        c2
        ] 
    testBuild circuit expected

[<Fact>]
let ``buildElement: PowerPin`` () =
    let element = CabelIn (PowerPin (5.0, 2)) 
    element |> buildElement add
    =! element

[<Theory>]
[<InlineData(5.0, 2)>]
[<InlineData(5.0, 4)>]
[<InlineData(3.0, 1)>]
[<InlineData(3.0, 17)>]
let ``buildPowerPin: valid cases`` volts num =
    buildPowerPin (volts, num) =! None
    
    let element = CabelIn (PowerPin (volts, num))
    element |> buildElement add =! element

[<Theory>]
[<InlineData(4.5, 4)>]
[<InlineData(5.0, 9)>]
[<InlineData(3.0, 3)>]
let ``buildPowerPin: invalid cases`` volts num =
    buildPowerPin (volts, num) 
    =! (Some NotPowerPinParameters)

    let e = CabelIn (PowerPin (volts, num))
    buildElement add e 
    =! (Err (NotPowerPinParameters, e))

[<Theory>]
[<InlineData(6)>]
[<InlineData(30)>]
let ``buildGroundPin: valid pin`` n =
    buildGroundPin n =! None
    let element = CabelIn (GroundPin n)
    element |> buildElement add =! element

[<Theory>]
[<InlineData(1)>]
[<InlineData(100)>]
let ``buildGroundPin: not ground pin`` n =
    buildGroundPin n =! (Some NotGroundPinParameters)
    let e = CabelIn (GroundPin n)
    e |> buildElement add =! 
    (Err (NotGroundPinParameters, e)) 

[<Fact>]
let ``buildBreadBoardPosition: valid positions`` () =
    fun x -> 
        let element = CabelOut (TopHor x)
        element |> buildElement add =! element
    |> testBreadBoardPos
