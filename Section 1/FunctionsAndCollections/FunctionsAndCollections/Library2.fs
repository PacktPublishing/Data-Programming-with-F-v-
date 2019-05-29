module FunctionsAndCollections2

    1
    |> Seq.unfold( fun n -> 
        if n < 300
            then Some(n, n + n) 
            else None) 
    |> Seq.iter (printf "%d ")
    
    (1,0)
    |> Seq.unfold( fun (n1, n2) -> Some(n1 + n2, (n2, n1 + n2)) ) 
    |> Seq.take 15
    |> Seq.iter (printf "%d ")

    let someNumbers = [ 1 .. 5 ]
    let moreNumbers = [ 6 .. 10 ]  
    let combinedNumbers = List.append someNumbers moreNumbers 
    
    let squareAndCube n =
        [ 
            n * n 
            n * n * n 
        ]

    combinedNumbers
    |> List.collect squareAndCube
    |> List.iter (printf "%d ") 
    
    let zippedList = List.zip someNumbers moreNumbers
    let unzippedList = List.unzip zippedList 

    let stillMoreNumbers = [ 11 .. 15 ]
    let zipped3List = List.zip3 someNumbers moreNumbers stillMoreNumbers
    let unzipped3List = List.unzip3 zipped3List 

    let notZippedList = List.zip someNumbers combinedNumbers 
    let zippedSeq = Seq.zip someNumbers moreNumbers 

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

