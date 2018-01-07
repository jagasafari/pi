module ValidateSnd

open System.Collections.Generic
open Microsoft.FSharp.Reflection
open DataTypes

let pairwiseRules = Dictionary<string, string>()
pairwiseRules.Add("LedAnode", "LedKatode") 
pairwiseRules.Add("LedKatode", "LedAnode") 
pairwiseRules.Add("CabelIn", "CabelOut") 
pairwiseRules.Add("CabelOut", "CabelIn") 

let getElementName case =
    (FSharpValue.GetUnionFields(
        case, typeof<Element>)
    |> fst).Name

let isPair: Element -> string -> bool = 
    getElementName >> (=)

let nextState element idx key = function
    | false, None -> (None, false)
    | false, Some _ -> (None, true)
    | true, None when idx = 0 -> (None, true)
    | true, None -> (Some element, false)
    | true, Some element ->
        let ok = pairwiseRules.[key] |> isPair element
        if ok then (None, false) else (None, true)

let buildSndValidation (circuit: Element list) = 
    let folder element (prev, tail, idx) =
        let key = getElementName element
        let applyRule = pairwiseRules.ContainsKey key
        let (nextPrev, isError) = 
            nextState element idx key (applyRule, prev)
        let updatedElement = 
            if isError 
            then (Err (InvalidPairwiseRule, element))
            else element
        (nextPrev, updatedElement::tail, idx - 1)
    let _, builtCircuit, _ =
        (None, [], (circuit.Length - 1))
        |> List.foldBack folder circuit    
    builtCircuit
