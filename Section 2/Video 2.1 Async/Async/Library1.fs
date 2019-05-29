module AsyncDemo

open System
open System.Diagnostics
open System.Threading
open System.Net

(*
#time
*)

let fetchSync (name, url) =
    let timer = Stopwatch.StartNew()
    let threadId1 = Thread.CurrentThread.ManagedThreadId

    let uri = new Uri(url)
    let client = new WebClient()
    let html = client.DownloadString(uri)
    let threadId2 = Thread.CurrentThread.ManagedThreadId
    let elapsed = timer.ElapsedMilliseconds
    timer.Stop()

    sprintf 
            "Read %d characters from %s on threads %d and %d in %d milliseconds" 
                html.Length name threadId1 threadId2 elapsed

let fetchAsync (name, url) =
    async {
        let timer = Stopwatch.StartNew()
        let threadId1 = Thread.CurrentThread.ManagedThreadId
        let uri = new Uri(url)
        let client = new WebClient()
        let! html = client.AsyncDownloadString(uri)
        let threadId2 = Thread.CurrentThread.ManagedThreadId
        let elapsed = timer.ElapsedMilliseconds
        timer.Stop()
        
        return sprintf 
            "Read %d characters from %s on threads %d and %d in %d milliseconds" 
                html.Length name threadId1 threadId2 elapsed
    }

let runAllSync urls =
    printfn "runAllSync started on thread %d" Thread.CurrentThread.ManagedThreadId
    urls
    |> List.fold (fun accum u -> (fetchSync u)::accum) []

let runAllAsync urlList =
    printfn "runAll started on thread %d" Thread.CurrentThread.ManagedThreadId
    urlList
    |> List.map fetchAsync
    |> Async.Parallel
    |> Async.RunSynchronously

urlList |> runAllSync
urlList |> runAllAsync