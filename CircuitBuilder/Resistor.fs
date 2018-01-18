module Resistor

type Color = | Black | Brown | Red | Orange | Yellow | Green | Blue | Violet | Grey | White | Gold | Silver

type Resistor = Resistor of (float * float * int option)

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
    | NoColor -> 1.0

let tolerance = function
    | Brown -> 0.01
    | Red -> 0.02
    | Green -> 0.005
    | Blue -> 0.0025
    | Violet -> 0.001
    | Grey -> 0.0005
    | Gold -> 0.05
    | Silver -> 0.1
    | NoColor -> 0.2
    | Black | Orange | Yellow -> 0.0

let figure _ = 0.0

let resistor = function
    | [f1; f2; m; t] ->
        let ohms = 
            m |> multiplier |> (*) 
            <| ((figure f1) * 10.0 + (figure f2))
        Resistor (ohms, t |> tolerance, None)
    | _ -> failwith "not yet"
        
