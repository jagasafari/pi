module Validate

open DataTypes

type BuildError =
    | NotGpioPowerPin of (float * int)
    | WrongValidation of Element

let contains list n = 
    List.exists (fun e -> e = n) list

let getErr msg = function
    | true -> None | false -> Some msg

let validatePowerPin p = 
    let getPowerPinErr l v n =    
        contains l n
        |> getErr (NotGpioPowerPin (v, n))
    match p with
    | 5.0, n -> getPowerPinErr [2;4] 5.0 n
    | 3.0, n -> getPowerPinErr [1;17] 3.0 n
    | _ -> None

// Element -> BuildError option
let validateElement (element: Element) =
    match element with
    | PowerPin p -> validatePowerPin p
    | e -> Some (WrongValidation e)

// Element seq -> 
// Choice<Element seq, BuildError seq>
let build circuit = 
    circuit
    |> Seq.map validateElement 
    |> ignore
    Choice1Of2 circuit
