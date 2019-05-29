module AsyncDemo

open System
open System.Net
open System.Threading

let urlList = 
    [
        ("Twitter", "https://www.twitter.com/")
        ("Facebook", "https://www.facebook.com/")
        ("Meetup", "https://www.meetup.com/")
        ("Try F#", "http://www.tryfsharp.org/")
        ("Bing", "https://www.bing.com/#")
        ("MSDN", "https://msdn.microsoft.com/en-us/dn308572.aspx")
    ]

let fetchAsync (name, url) =
    async {
        let uri = new Uri(url)
        let client = new WebClient()

        //let! html = client.AsyncDownloadString(uri)

        let! html = 
            client.DownloadStringTaskAsync(uri)
            |> Async.AwaitTask 
        
        return sprintf
            "Read %d characters from %s" 
                html.Length name 
    }

let runAllAsync urlList =
    urlList
    |> List.map fetchAsync
    |> Async.Parallel
    |> Async.RunSynchronously

urlList |> runAllAsync

fetchAsync ("Twitter", "https://www.twitter.com/")
|> Async.Start

let toUnitWorkFlow wf =
    async {
        let! childWf = Async.StartChild wf
        let! result = childWf
        printfn "%s" result
    }

fetchAsync ("Twitter", "https://www.twitter.com/")
|> toUnitWorkFlow
|> Async.Start

Async.CancelDefaultToken()

let runWithSomeCancellations urlList =
    urlList
    |> List.map (fun u ->
        let ts = new CancellationTokenSource()
        let f = toUnitWorkFlow (fetchAsync u)
        Async.Start(f, ts.Token)
        ts)
    |> List.take 3
    |> List.iter (fun ts -> ts.Cancel() )

urlList |> runWithSomeCancellations

let fetchAsyncWithContinuations u =
    Async.StartWithContinuations(
        fetchAsync u,
        (printfn "Success: %s"),
        (printfn "Caught exception: %O"),
        (fun _ -> 
            printfn "Continuing after function cancelled")
    )
    Async.CancelDefaultToken()

("Twitter", "https://www.twitter.com/")
|> fetchAsyncWithContinuations 
