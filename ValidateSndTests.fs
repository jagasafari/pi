module ValidateSndTests

open Xunit
open Swensen.Unquote
open CircuitTestUtils
open ValidateSnd
open DataTypes

let testSndLine c e = 
    testCircuit buildSndValidation c e 
    
[<Fact>]
let ``buildSndValidation:`` () =
    testSndLine validCircuit validCircuit

[<Fact>]
let ``buildSndValidation: to many cabel elements`` () =
    let e = CabelOut (TopHor 1)
    let circuit = e::validCircuit
    let expected =
        (Err (InvalidPairwiseRule, e))::validCircuit
    testSndLine circuit expected

[<Fact>]
let ``buildSndValidation:  invalid circuit`` () =
    let circuit =
        [
        CabelIn (PowerPin (3.0, 1))
        CabelOut (TopHor 4)
        LedKatode (TopHor 6)
        CabelIn (TopHor 1)
        CabelOut (TopGround 10)
        CabelIn (GroundPin 6)
        ]
    let expected =
        [
        CabelIn (PowerPin (3.0, 1))
        CabelOut (TopHor 4)
        Err (InvalidPairwiseRule, LedKatode (TopHor 6))
        CabelIn (TopHor 1)
        CabelOut (TopGround 10)
        CabelIn (GroundPin 6)
        ]
    testSndLine circuit expected

[<Fact>]
let ``isPair: `` () = 
    let value = "LedAnode"
    let element = LedAnode (TopHor 5)
    isPair element value

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

