module Led

open Contract
open CmdBuilder
open Random

type LedState = On | Off

let led pin = 
    let mutable state: LedState option = None
    let toggle ()  = 
        match state with
        | None ->
            pinMode pin Output
            digitalWrite pin Low  
            state <- Some On
        | Some On -> 
            digitalWrite pin High
            state <- Some Off
        | Some Off ->
            digitalWrite pin Low
            state <- Some On
        ([||], [||], state.Value)
    toggle

let rndChangeRgbLed builder pinRed pinGreen pinBlue =
    let mutable color : (int * int * int) option = None
    let add, create = builder ()
    let init () =
        let createPwmPin pin = softPwmCreate pin 0 100
        createPwmPin pinRed
        createPwmPin pinGreen
        createPwmPin pinBlue
    let rndRgb =
        let get = rndInt ()
        fun () -> 
            let nxt () = get 0 255
            (nxt (), nxt (), nxt ())
    let change () =
        match color with
        | None -> init ()
        | Some _ -> ()
        color <- rndRgb () |> Some
        ([||], [||], color.Value)
    change
