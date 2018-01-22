module Errors

type ResistorErrors = | NotToleranceColor | NotDigitColor

let fail msg = msg |> sprintf "%A" |> failwith
