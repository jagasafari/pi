module Contract

type Write = Low | High
type PinMode = Output | Input
type CommandResult = | NoErrors

let wiringPiSetup : int = -1
let pinMode pin mode = ()
let delay milliseonds = ()
let delayMicroseconds microsecs = ()
let digitalWrite pin state = ()
let readCounts pin numTransitions badCount outputOffset =
    [ 0 ]
let softPwmCreate pin v1 v2 = ()
let softPwmWrite pin value = ()
