namespace CircuitBuilder.Controllers

open Microsoft.AspNetCore.Mvc

[<Route("api/[controller]")>]
type CalculatorController () =
    inherit Controller()

    [<HttpGet("/resistor")>]
    member this.Get() = 10.0
        
