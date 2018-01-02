module Program 

open System
open Read

let [<EntryPoint>] main _ = 
    allCmds |> Seq.iter Console.WriteLine
    0
