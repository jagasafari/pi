module Tests

open Xunit
open System.Collections.Generic
open DataTypes
open Swensen.Unquote
open Validate

[<Fact>]
let ``is array copied`` () =
    let a = [|1;1|]
    let updateArray (ar: int []) =
        ar.[0] <- 2; ()
    a.[0] =! 1
let testCircuit build circuit expected =
    circuit |> build |> Seq.zip <| expected 
    |> Seq.iter (fun (x, y) -> x =! y)

let testBuild = testCircuit build
let testSndLine c e = 
    testCircuit buildSndValidation c e 
    
[<Fact>]
let ``buildSndValidation`` () =
    let circuit = [ BottomVer (8,'J') ]
    let result = circuit |> buildSndValidation
    testSndLine result circuit

//[<Fact>]
//let ``buildSndValidation:  invalid circuit`` () =
//    let circuit: Element list = 
//        [ 
//        (Err (NotGpioPin, (TopHor 3))) 
//        ]
//    let result = [] |> buildSndValidation      
//    testSndLine result circuit
//
//[<Fact>] 
//let ``buildSndValidation: no katode`` () =
//    let circuit = 
//        [
//        LedAnode (TopVer (4, 'A'))
//        TopVer (6, 'B')
//        ]
//    let expected =
//        [
//        Err (LedMissingKatode, (LedAnode (TopVer (4, 'B'))))
//        TopVer (6, 'B')
//        ]
//    let result = circuit |> buildSndValidation
//    testSndLine result expected
//
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
    TopHor 7 |> getElementName =! "TopHor"


[<Fact>]
let ``reservePosition`` () =
    let hs = HashSet<Element>()
    let simple = TopHor 3
    let led = LedAnode simple
    let cabel = CabelIn (TopHor 4)
    reservePosition hs led =! true 
    reservePosition hs simple =! false 
    reservePosition hs simple =! false 
    reservePosition hs cabel =! true 
    reservePosition hs cabel =! false 
    hs.Count =! 2

[<Fact>]
let ``circuit: position already taken error`` () =
    let pos = BottomVer (2, 'H')
    let circuit =
        [
        pos
        pos
        ] 

    let expected =
        [
        pos
        Err (PositionAlreadyTaken, pos)
        ]
    testBuild circuit expected

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
    testBuild circuit expected

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
