module Tests

open Xunit
open DataTypes
open Swensen.Unquote
open Validate

[<Fact>]
let ``passFstValidation: predicate`` () =
    let circuit =
        [
        CabelIn (PowerPin (3.0, 8))
        CabelOut (BottomHor 5)
        ]
    passFstValidation circuit =! true

let testSealDict key value =
    pairwiseRules.[getElementName key]
    =! (getElementName value)

[<Fact>]
let ``pairwiseRules: seal dict`` () =
    let key = LedAnode (TopHor 8)
    let value = LedKatode (TopVer (8, 'A'))
    testSealDict key value

[<Fact>]
let ``pairwiseRules: cabel -> seal dict`` () =
    let key = CabelIn (PowerPin (6.0, 2))
    let value = CabelOut (TopHor 6)
    testSealDict key value

[<Fact>]
let ``pairwiseRules: count`` () =
    pairwiseRules.Count =! 4

[<Fact>]
let ``getElementName: string`` () =
    TopHor 7 |> getElementName 
    =! "TopHor"

let testCircuit circuit expected =
    circuit |> build |> Seq.zip <| expected 
    |> Seq.iter (fun (x, y) -> x =! y)

[<Fact>]
let ``circuit: position already taken error`` () =
    let pos = TopVer (2, 'A')
    let circuit =
        [
        LedAnode pos
        LedKatode pos
        ] 

    let expected =
        [
        LedAnode pos
        Err (PositionAlreadyTaken, LedKatode pos)
        ]
    testCircuit circuit expected

[<Fact>]
let ``CabelIn: successfull build`` () =
    let e = CabelIn (PowerPin (5.0, 2))
    buildElement () e =! e

[<Fact>]
let ``CabelIn: failuare build`` () =
    let e = CabelIn (TopHor 8) 
    buildElement () e =! (Err (NotGpioPin, e))

[<Fact>]
let ``CabelIn: wrong pin number`` () =
    let e = CabelIn (GroundPin 0)
    buildElement () e =! 
    ((NotGroundPinParameters, e) |> Err)

let testBreadBoardPos f =
    [1 .. 63] |> Seq.iter f

[<Fact>]
let ``buildBreadBoardVertical: valid positions`` () =
    let art element (x, y) = 
        element (x, y) |> buildElement ()
        =! (element (x, y))

    fun x -> 
        let a e = 
            Seq.iter (fun y -> art e (x, y)) 
        a TopVer ['A'..'E']
        a BottomVer ['F'..'J']
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
    testCircuit circuit expected

[<Fact>]
let ``validaiting element: PowerPin`` () =
    PowerPin (5.0, 2) |> buildElement ()
    =! (PowerPin (5.0, 2))

[<Theory>]
[<InlineData(5.0, 2)>]
[<InlineData(5.0, 4)>]
[<InlineData(3.0, 1)>]
[<InlineData(3.0, 17)>]
let ``buildPowerPin: valid cases`` 
    volts num =
    buildPowerPin (volts, num) =! None
    
    PowerPin (volts, num) |> buildElement ()
    =! (PowerPin (volts, num))

[<Theory>]
[<InlineData(4.5, 4)>]
[<InlineData(5.0, 9)>]
[<InlineData(3.0, 3)>]
let ``buildPowerPin: invalid cases`` volts num =
    buildPowerPin (volts, num) 
    =! (Some NotPowerPinParameters)

    let e = PowerPin (volts, num) 
    buildElement () e 
    =! (Err (NotPowerPinParameters, e))

[<Theory>]
[<InlineData(6)>]
[<InlineData(30)>]
let ``buildGroundPin: valid pin`` n =
    buildGroundPin n =! None
    GroundPin n |> buildElement ()
    =! (GroundPin n)

[<Theory>]
[<InlineData(1)>]
[<InlineData(100)>]
let ``buildGroundPin: not ground pin`` n =
    buildGroundPin n =! 
    (Some NotGroundPinParameters)
    let e = GroundPin n 
    e |> buildElement () =! 
    (Err (NotGroundPinParameters, e)) 

[<Fact>]
let ``buildBreadBoardPosition: valid positions`` () =
    fun x -> 
        TopHor x |> buildElement () =! (TopHor x)
    |> testBreadBoardPos
