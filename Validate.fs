module Validate

open DataTypes

let getResult element = function
    | None -> element | Some er -> Err er

let contains n = List.exists (fun e -> e = n)

let getErr msg = function
    | true -> None | false -> Some msg

let getGroundPinErr n =
    contains n [6; 9; 14; 20; 25; 30; 34; 39]
    |> getErr (NotGroundPin n)

let getPowerPinErr (v, n) = 
    let er = NotGpioPowerPin (v, n)
    let getErr = contains n >> getErr er
    match v with
    | 5.0 -> getErr [2;4] 
    | 3.0 -> getErr [1;17] 
    | _ -> Some er

// Element -> Element
let rec buildElement element =
    match element with
    | PowerPin p -> p |> getPowerPinErr
    | GroundPin p -> p |> getGroundPinErr
    | e -> e |> WrongValidation |> Some
    |> getResult element

// Element seq -> Element seq
let build circuit = 
    circuit
    |> Seq.map buildElement 
    |> ignore
    circuit
