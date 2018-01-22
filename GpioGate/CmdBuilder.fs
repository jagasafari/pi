module CmdBuilder

open System.Collections.Generic 

let cmdBuilder getStamp () =
    let cmds = List<int>()    
    cmds.Add (getStamp ())
    let add (cmd, _) = cmds.Add cmd
    let create () = (cmds |> Seq.toArray, [||])
    (add, create)

