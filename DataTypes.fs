module DataTypes

open System

type PowerPin = PowerPin2
type GpioPin = PowerPin of PowerPin
type Component = GpioPin of GpioPin | Cabel
type Circuit = Component []    
type ForCabel = ForCabel of Circuit
type ForBreadBoardPosition =
    | ForBreadBoardPosition of Circuit
type BreadBoardPosition = 
    | HorTop of int
    | GroundTop of int
    | VerTop of (int*int)
    | VerBottom of (int*int)
    | HorBottom of int
    | GroundBottom of int
