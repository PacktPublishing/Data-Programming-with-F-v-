module WorldBank1

open FSharp.Data

let data = WorldBankData.GetDataContext()

data.Countries.Bermuda.CapitalCity

data
    .Countries
    .``Czech Republic``
    .Indicators
    .``Central government debt, total (% of GDP)``
|> Seq.maxBy fst

type GitHubIssues = 
    JsonProvider<"C:\\Data\\Github_Sample_Issues.json">

let topRecentlyUpdatedIssues = 
    GitHubIssues.Load("https://api.github.com/repos/fsharp/FSharp.Data/issues")
    |> Seq.filter (fun issue -> issue.State = "open")
    |> Seq.sortBy (fun issue -> System.DateTime.Now - issue.UpdatedAt)
    |> Seq.truncate 5
    |> Seq.iter (fun issue -> 
        printfn "#%d %s" issue.Number issue.Title)



