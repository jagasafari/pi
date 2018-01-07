module Validate

open System.Collections.Generic
open DataTypes
open ValidateSnd

let buildResult element = function
    | None -> element 
    | Some er -> Err (er, element)

let existsErr (err: BuildError) n =
    let getErr = function
        | true -> None | false -> Some err
    List.exists (fun e -> e = n) >> getErr

let buildGroundPin n =
    let pinNumbers = [6;9;14;20;25;30;34;39] 
    existsErr 
       NotGroundPinParameters n pinNumbers

let buildPowerPin (v, n) = 
    let err = NotPowerPinParameters
    match v with
    | 5.0 -> existsErr err n [2;4] 
    | 3.0 -> existsErr err n [1;17] 
    | _ -> Some err

let buildGpioPin = function
    | PowerPin p -> buildPowerPin p
    | GroundPin p -> buildGroundPin p
    | _ -> Some NotGpioPin

let buildBBHor num element =
    let err = NotBreadBoardParameters 
    existsErr err num [1 .. 63]

let buildBBVer element (h, v) positions =
    let err = NotBreadBoardParameters
    let eh = existsErr err h [1 .. 63]
    let ev = existsErr err v positions 
    match (eh, ev) with
    | None, None -> None
    | _ -> Some err

let isGpioPin = function
    | PowerPin _ | GroundPin _ -> true 
    | _ -> false

let isBBPos = function
    | TopHor _ 
    | TopGround _ 
    | BottomHor _ 
    | BottomGround _ 
    | TopVer _
    | BottomVer _ -> true
    | _ -> false

let buildBBPos element =
    let buildBBVer = buildBBVer element
    match element with
    | TopHor p 
    | BottomHor p 
    | TopGround p 
    | BottomGround p -> buildBBHor p element
    | TopVer p -> buildBBVer p ['A' .. 'E']
    | BottomVer p -> buildBBVer p ['F' .. 'J']
    | _ -> Some NotBreadBoardPosition

let buildLedKatode = function
    | TopGround _
    | BottomGround _ -> Some MinusChargeNotToGround
    | element -> buildBBPos element

let reservePosition add = function
    | CabelIn e 
    | CabelOut e 
    | LedAnode e
    | LedKatode e 
    | e -> add e 

let buildNewElement = function 
    | e when isBBPos e -> 
        Some BreadBoardPositionCanNotBeOnItsOwn
    | e when isGpioPin e -> Some PinCanNotBeOnItsOwn
    | CabelIn e -> buildGpioPin e
    | CabelOut e -> buildBBPos e
    | LedAnode e -> buildBBPos e
    | LedKatode e -> buildLedKatode e
    | e -> NotImplementedValidation |> Some

let getSet<'T> () =
    let s = HashSet<'T>()
    s.Add, fun () -> s.Count

// Element -> Element
let buildElement add element =
    if reservePosition add element 
    then buildNewElement element
    else PositionAlreadyTaken |> Some
    |> buildResult element

let buildZeroValidation (circuit: Element list) = 
    let minCircuitLength = 6
    if circuit.Length < minCircuitLength then
        let tmp = circuit |> List.toArray
        tmp.[0] <- Err (CircuitToShort, circuit.[0])
        tmp |> Array.toList
    else circuit

let buildFstValidation = 
    let add, _ = getSet<Element> ()
    List.map (buildElement add)

let (.>>) validatePrev validateNext circuit =
    let isErrElement = function
        | Err _ -> true | _ -> false

    let circuitPrev = validatePrev circuit
    circuitPrev
    |> (List.exists isErrElement >> not)
    |> function 
        | false -> circuitPrev
        | true -> circuitPrev |> validateNext

// Element list -> Element list
let build = 
    buildZeroValidation
    .>> buildFstValidation
    .>> buildSndValidation
