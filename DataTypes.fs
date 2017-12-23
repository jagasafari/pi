module DataTypes

type Element = 
    | PowerPin of (float * int)
    | GroundPin of int
    | TopHor of int
    | TopGround of int
    | Cabel of (Element * Element)
    | Led of (Element * Element)

// invalid builderr element, validate retrun eitehr valid element or buildErr
