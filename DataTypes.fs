module DataTypes

type Element = 
    | PowerPin of (float * int)
    | GroundPin of int
    | TopHor of int
    | TopGround of int
    | TopVer of (int*char)
    | BottomHor of int
    | BottomGround of int
    | BottomVer of (int*char)
    | CabelIn of Element
    | CabelOut of Element
    | LedAnode of Element
    | LedKatode of Element
    | Err of (BuildError * Element)
and BuildError =
    | NotImplementedValidation
    | NotGpioPin
    | NotPowerPinParameters
    | NotGroundPinParameters
    | NotBreadBoardPosition
    | NotBreadBoardParameters 
    | LedInvalidPosition
    | LedCanOnlyBeConnectedToBreadBoard 
    | LedMissingKatode
    | MinusChargeNotToGround
    | PositionAlreadyTaken
