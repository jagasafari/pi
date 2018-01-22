module Random

open System
open System.Threading

let rndInt () =
    let rnd = Random()
    fun lower upper -> rnd.Next(lower, upper)

let timer () =
    let mutable timer: Timer option = None
    let create callBack interval =
        timer <-
            new Timer(
                TimerCallback(callBack), 
                null, 
                interval, 
                Timeout.Infinite)
            |> Some
    let change interval =
        match timer with
        | Some x -> 
            x.Change(interval, Timeout.Infinite) |> ignore
        | None -> ()
    let stop () = 
        match timer with
        | Some x -> x.Dispose() | None -> ()
    create, change, stop

let rndTimesStream () =
    let publish, trigger = 
        let evt = Event<_>() in (evt.Publish, evt.Trigger)
    let rndTime = 
        let r = rndInt ()
        fun () -> r 1000 3000
    let create, change, stop = timer ()
    let callBack _ = 
        try (trigger ())
        finally (change (rndTime ()))
    create callBack (rndTime ())
    publish, stop
