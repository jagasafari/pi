module Validate

open DataTypes

let getResult element = function
    | None -> element | Some er -> Err er

let listErr (err: BuildError) n =
    let getErr = function
        | true -> None | false -> Some err
    List.exists (fun e -> e = n) >> getErr

let getGroundPinErr n =
    let pinNumbers = [6;9;14;20;25;30;34;39] 
    listErr (NotGroundPin n) n pinNumbers

let getPowerPinErr (v, n) = 
    let err = NotPowerPin (v, n) 
    match v with
    | 5.0 -> listErr err n [2;4] 
    | 3.0 -> listErr err n [1;17] 
    | _ -> Some err

let getBreadBoardHorErr num element =
    let err = 
        NotHorizontalBreadBoardPosition 
            element
    listErr err num [1 .. 63]

let getBreadBoardVerTopErr 
    (h, v) element positions =
    let err =
        NotVerticalBreadBoardPosition element
    let eh = listErr err h [1 .. 63]
    let ev = listErr err v positions 
    match (eh, ev) with
    | None, None -> None
    | _ -> Some err

let isGpioPin = function
    | PowerPin _ | GroundPin _ -> true 
    | _ -> false

let getCabelErr build (p1, p2) =
    let getPosNumErr () =
        let b1 = build p1
        let b2 = build p2
        match b1, b2 with
        | Err _, _ | _, Err _ -> 
            CabelNotValidPosition (b1, b2) 
            |> Some
        | _ -> None
    match isGpioPin p1, isGpioPin p2 with
    | (true, true) -> 
        CabelTwoGpioPositions (p1, p2) |> Some
    | true, false 
    | false, true -> getPosNumErr()
    | _ -> 
        CabelNoGpioPosition (p1, p2) |> Some

let getLedErr build (p1, p2) =
    let posErr () =
        let b1 = build p1
        let b2 = build p2
        match b1, b2 with
        | Err _, _ | _, Err _ -> 
            LedInvalidPosition (b1, b2) 
            |> Some
        | _ -> None
    match isGpioPin p1, isGpioPin p2 with
    | false, false -> posErr ()
    | _ -> 
        LedCanOnlyBeConnectedToBreadBoard (p1, p2) 
        |> Some

// Element -> Element
let rec buildElement element =
    match element with
    | PowerPin p -> p |> getPowerPinErr
    | GroundPin p -> p |> getGroundPinErr
    | TopHor p 
    | TopGround p 
    | BottomHor p 
    | BottomGround p -> 
        getBreadBoardHorErr p element
    | TopVer p ->
        getBreadBoardVerTopErr 
            p element ['A' .. 'E']
    | BottomVer p -> 
        getBreadBoardVerTopErr 
            p element ['F' .. 'J']
    | Cabel p -> getCabelErr buildElement p
    | Led p -> getLedErr buildElement p
    | e -> e |> WrongValidation |> Some
    |> getResult element

// Element seq -> Element seq
let build circuit = 
    circuit |> Seq.map buildElement 
