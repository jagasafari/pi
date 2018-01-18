module Random

open System
open System.Threading

let rndMilliSec () =
    let rnd = Random(Environment.TickCount)
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
            x.Change(interval, Timeout.Infinite)
            |> ignore
        | None -> ()
    let stop () = 
        match timer with
        | Some x -> x.Dispose() | None -> ()
    create, change, stop

let rndTimesStream () =
    let publish, trigger = 
        let evt = Event<_>() in (evt.Publish, evt.Trigger)
    let rndTime () = rndMilliSec () 1000 3000
    let create, change, stop = timer ()
    let callBack _ = 
        try (trigger ())
        finally (change (rndTime ()))
    create callBack (rndTime ())
    publish, stop
// subscribe with handle
// handle calls wiringpi to toggle the single led
// need pin number
// simulator first
