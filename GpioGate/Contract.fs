module Contract

type Write = Low | High
type PinMode = Output | Input

let complete stamp = ()
let wiringPiSetup stamp : int = -1
let pinMode stamp pin mode = ()
let delay stamp milliseonds = ()
let delayMicroseconds stamp microsecs = ()
let digitalWrite stamp pin state = ()
let readCounts 
    stamp 
    pin 
    numTransitions 
    badCount 
    outputOffset =
    [ 0 ]
