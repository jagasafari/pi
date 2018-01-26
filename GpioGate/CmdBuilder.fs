module CmdBuilder

open System.Collections.Generic 

let cmdBuilder getStamp =
    let cmds = List<int>()    
    let cmdArgs = List<int>()    
    cmds.Add (getStamp ())
    let add cmd = cmds.Add cmd
    let addArgs args = cmdArgs.AddRange args
    let create () = 
        (cmds |> Seq.toArray, cmdArgs |> Seq.toArray)
    (add, addArgs, create)

