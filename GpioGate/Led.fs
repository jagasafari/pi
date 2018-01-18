module Led

open Contract

type LedState = On | Off

let led pin = 
    let mutable state: LedState option = None
    let toggle stamp = 
        match state with
        | None ->
            pinMode stamp pin Output
            digitalWrite stamp pin Low  
            state <- Some On
        | Some On -> 
            digitalWrite stamp pin High
            state <- Some Off
        | Some Off ->
            digitalWrite stamp pin Low
            state <- Some On
        complete stamp
    toggle
