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
    | Cabel of (Element * Element)
    | Led of (Element * Element)
    | Err of BuildError
and BuildError =
    | WrongValidation of Element
    | NotPowerPin of (float * int)
    | NotGroundPin of int
    | NotHorizontalBreadBoardPosition 
        of Element
    | NotVerticalBreadBoardPosition of Element
    | CabelTwoGpioPositions of (Element * Element)
    | CabelNoGpioPosition of (Element * Element)
    | CabelNotValidPosition of 
        (Element * Element)
    | LedInvalidPosition of (Element * Element)
    | LedCanOnlyBeConnectedToBreadBoard 
        of (Element * Element)
