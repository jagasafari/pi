namespace CircuitBuilder

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration

type Startup(configuration: IConfiguration) =
    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        app.UseMvc() |> ignore

    member val Configuration : IConfiguration = 
        null with get, set
