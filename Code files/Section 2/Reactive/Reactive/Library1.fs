module ReactiveDemo

(*
#r "System.Reactive.Core.dll"
#r "System.Reactive.Interfaces.dll"
#r "System.Reactive.Linq.dll"
#r "FSharp.Control.Reactive"
*)

open System
open System.Threading
open FSharp.Control.Reactive
open FSharp.Control.Reactive.Builders

let generateInts max (sleep:int) =
    let rec loop n =
        observe {
            Thread.Sleep sleep
            yield n
            if n < max
                then yield! loop (n + 1)
            }
    loop 1

generateInts 50 0
|> Observable.filter (fun n -> n % 7 = 0)
|> Observable.map (fun n -> n - 2) 
|> Observable.reduce (fun total n -> total + n)
|> Observable.subscribe (printfn "%d")

(generateInts 50 2000)
|> Observable.sample (TimeSpan(0,0,5))
|> Observable.subscribe (printfn "%d")
|> ignore

rxquery {
  for n in (generateInts 50 0) do
  where (n % 7 = 0)
  take 10
  }
|> Observable.subscribe (printfn "%d")
|> ignore