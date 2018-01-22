module Resistor

open Errors

type Resistor = Resistor of (float * float * int option)

type MultiplierColor = 
    | Black 
    | Brown 
    | Red 
    | Orange 
    | Yellow 
    | Green 
    | Blue 
    | Violet 
    | Grey 
    | White 
    | Gold 
    | Silver

let figure = function
    | Black -> 0.0
    | Brown -> 1.0
    | Red -> 2.0
    | Orange -> 3.0
    | Yellow -> 4.0
    | Green -> 5.0
    | Blue -> 6.0
    | Violet -> 7.0
    | Grey -> 8.0
    | White -> 9.0
    | Gold | Silver -> fail NotDigitColor

let multiplier = function
    | Black -> 1.0      
    | Brown -> 10.0
    | Red -> 100.0
    | Orange -> 10.0**3.0
    | Yellow -> 10.0**4.0
    | Green -> 10.0**5.0
    | Blue -> 10.0**6.0
    | Violet -> 10.0**7.0
    | Grey -> 10.0**8.0
    | White -> 10.0**9.0
    | Gold -> 0.1
    | Silver -> 0.01

let tolerance = function
    | Brown -> 0.01
    | Red -> 0.02
    | Green -> 0.005
    | Blue -> 0.0025
    | Violet -> 0.001
    | Grey -> 0.0005
    | Gold -> 0.05
    | Silver -> 0.1
    | Black | White | Orange | Yellow 
        -> fail NotToleranceColor

let ohms f1 f2 m =
    m |> multiplier |> (*)
    <| ((figure f1) * 10.0 + (figure f2))
    
let resistor = function
    | [f1; f2; m; t] ->
        Resistor (ohms f1 f2 m, t |> tolerance, None)
    | [f1; f2; m] ->
        Resistor (ohms f1 f2 m, 0.2, None) 
    | _ -> failwith "not yet"
        
