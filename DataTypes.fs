module DataTypes

type Element = 
    | PowerPin of (float * int)
    | GroundPin of int
    | TopHor of int
    | TopGround of int
    | Cabel of (Element * Element)
    | Led of (Element * Element)
    | Err of BuildError
and BuildError =
    | WrongValidation of Element
    | NotGpioPowerPin of (float * int)
    | NotGroundPin of int
