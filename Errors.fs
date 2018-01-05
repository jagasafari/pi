module Errors

type Error =
    | NoValidateMiddleForWindowed of int
let fail: Error -> unit = sprintf "%A" >> failwith

