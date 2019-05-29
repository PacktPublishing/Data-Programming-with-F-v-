module FunctionsAndCollections

    open System
    open System.IO

    seq { 1 .. 3 }
    |> Seq.iter (printfn "%d")

    seq { for i in 1 .. 3 -> i*i }
    |> Seq.iter (printfn "%d")

    seq { for i in 1 .. 3 do yield i*i }
    |> Seq.iter (printfn "%d")

    seq {
        for i in 1 .. 3 do
            for j in 4 .. 5 do 
                yield i + j
    }
    |> Seq.iter (printfn "%d")

    let rec listAllFiles (path:string) =
        seq {
            for file in Directory.GetFiles(path) do
                yield file
            for directory in Directory.GetDirectories(path) do
                yield! listAllFiles directory
        }

    listAllFiles @"C:\Sample\Deja Vu Fonts"
    |> Seq.iter (printfn "%s")
    
    let readSingleLines (filePath : string) =
        seq {
                let rec readLine (reader:StreamReader) =
                    seq {
                        if reader.EndOfStream = false
                            then 
                                yield reader.ReadLine()
                                yield! readLine reader    
                    }
                
                use stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None)
                let reader = new StreamReader(stream)
                yield! readLine reader
        }
    
    let lines = readSingleLines @"C:\Data\Loan payments data.csv"
    let enumerator = lines.GetEnumerator()
    enumerator.MoveNext()
    enumerator.Current
    enumerator.Dispose()

     lines 
    |> Seq.iter (printfn "%s")

    let someNumbers = [ 1 .. 5 ]
    let moreNumbers = [ 6 .. 10 ]  
    let combinedNumbers = List.append someNumbers moreNumbers
    
    let threePowers n =
        [ n ; n * n ; n * n * n ]

    combinedNumbers
    |> List.collect threePowers 
    |> List.iter (printf "%d ")
    
    let zippedList = List.zip someNumbers moreNumbers
    let unzippedList = List.unzip zippedList

    let stillMoreNumbers = [ 11 .. 15 ]
    let zipped3List = List.zip3 someNumbers moreNumbers stillMoreNumbers
    let unzipped3List = List.unzip3 zipped3List
    
    someNumbers |> List.pairwise
    someNumbers |> List.windowed 2
    someNumbers |> List.windowed 3

    let petBreeds =
        [ 
            ("Cat", "Persian")
            ("Dog", "Collie")
            ("Cat", "Russian Blue")
            ("Bird", "Canary")
            ("Dog", "Corgie")
            ("Cat", "Siamese")
        ]
    
    petBreeds
    |> List.groupBy (fun t -> fst t)

