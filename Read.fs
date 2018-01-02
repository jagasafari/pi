module Read

open System
open Microsoft.FSharp.Reflection
open DataTypes
open System.Reflection

let formatParams (case: UnionCaseInfo) =
    let print (x: PropertyInfo) =
        let pt = x.PropertyType
        match pt.Name with
        | n when n.StartsWith("Tuple") -> 
            pt.GenericTypeArguments
            |> Array.map (fun y -> y.Name)
            |> Array.fold (sprintf "%s %s") ""
        | n -> n

    case.GetFields()
    |> Array.map print
    |> Array.fold (sprintf "%s %s") ""

let allCmds =
    let notErr (case: UnionCaseInfo) =
        case.Name = "Err" |> not
    let formatUnionCase (case: UnionCaseInfo) =
        sprintf "%s%s" 
            case.Name (formatParams case)

    FSharpType.GetUnionCases typeof<Element>
    |> Array.filter notErr
    |> Array.map formatUnionCase
