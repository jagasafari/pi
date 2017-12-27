module Tests

open Xunit
open DataTypes
open Swensen.Unquote
open Validate

let testCabel p1 p2 =
    let ss = Cabel (p1, p2)
    ss |> buildElement =! ss

[<Fact>]
let ``Cabel: no gpio`` () =
    Cabel ((TopHor 7), (TopGround 9))
    |> buildElement
    =! (Err (CabelNoGpioPosition ((TopHor 7), (TopGround 9))))

let testInvalidCabel p1 p2 =
    let ss = Cabel (p1, p2)
    let expected = 
        CabelNoGpioPosition (p1, p2) |> Err
    ss |> buildElement =! expected

[<Fact>]
let ``Cabel: error`` () =
    let e1 = PowerPin (3.4, 9)
    let e2 = TopVer (2, 'd')
    let ss = Cabel (e1, e2)
    let e1e = Err (NotPowerPin (3.4, 9))
    let e2e = 
        Err (NotVerticalBreadBoardPosition e2)
    let expected = 
        CabelNotValidPosition (e1e, e2e) |> Err
    ss |> buildElement =! expected

[<Fact>]
let ``Cabel: valid positions`` () =
    testCabel 
        (PowerPin (5.0, 2)) (TopVer (9, 'A'))

let testBreadBoardPos f =
    [1 .. 63] |> Seq.iter f

[<Fact>]
let ``getBreadBoardVerticalErr: valid positions`` () =
    let art element (x, y) = 
        element (x, y) |> buildElement 
        =! (element (x, y))

    fun x -> 
        let a e = 
            Seq.iter (fun y -> art e (x, y)) 
        a TopVer ['A'..'E']
        a BottomVer ['F'..'J']
    |> testBreadBoardPos

[<Fact>]
let ``modeling simple circuit`` () =
    let circuit =
        [
        Cabel (PowerPin (5.0, 2), TopHor 3)
        Led (TopHor 5, TopGround 7)
        Cabel (TopGround 9, TopGround 5)
        ]
    let expected = 
        [
        Cabel (PowerPin (5.0, 2), TopHor 3) 
        Led (TopHor 5, TopGround 7)
        Err (CabelNoGpioPosition (TopGround 9, TopGround 5))
        ] 
    circuit |> build |> Seq.zip expected |> Seq.iter (fun (x, y) -> x =! y)

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
    =! (NotPowerPin (volts, num) |> Some)

    (PowerPin (volts, num) |> buildElement)
    =! (Err (NotPowerPin (volts, num)))

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

[<Fact>]
let ``getBreadBoardPositionErr: valid positions`` () =
    fun x -> 
        TopHor x |> buildElement =! (TopHor x)
    |> testBreadBoardPos
