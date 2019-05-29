open System
open FSharp.Data

type KentuckyDerbyTypeProvider = 
    HtmlProvider<"C://data//KentuckyDerby.html">

let url = "https://en.wikipedia.org/wiki/Kentucky_Derby"

let derbyInfo = 
    KentuckyDerbyTypeProvider.Load(url)

for row in derbyInfo.Tables.Winners.Rows do
    printfn "%d %s %s" row.Year row.Winner row.Time

for furtherReading in derbyInfo.Lists.``Further reading``.Values do
    printfn "%s" furtherReading