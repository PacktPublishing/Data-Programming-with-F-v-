module LargeDocuments

    (*
    #time
    *)

    let f =
        fun () ->
            for i in [ 1 .. 100 ] do printfn "%d" i 

    f()
    
    let timeAFunction func =
        let stopwatch = System.Diagnostics.Stopwatch.StartNew()
        let result = func()
        stopwatch.Stop()
        (stopwatch.ElapsedMilliseconds, result)

    timeAFunction f

    open System
    open System.IO
    open System.Collections.Generic

    let readAllLines fileName =
        fileName
        |> File.ReadAllLines 
        |> Seq.ofArray

    let readLines fileName =
        fileName
        |> File.ReadLines 

    let readSingleLines (filePath : string) =
        seq {
                let rec readLine (reader:StreamReader) =
                    seq {
                        let line = reader.ReadLine()
                        if line <> null 
                            then 
                                yield line
                                yield! readLine reader    
                    }
                
                use stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None)
                let reader = new StreamReader(stream)
                yield! readLine reader
        }
    
    let readWordsInLine (line : string) =
        line.Split([|' '|])
        |> Seq.ofArray
        |> Seq.map (fun s -> s.Trim())

    let readWordsInFile (fileName : string) (f : string -> string seq) =
        fileName
        |> f
        |> Seq.collect readWordsInLine

    let countWords words =
        fun () ->
            words |> Seq.length

    let AliceInWonderland_file = @"C:\Data\Alice In Wonderland.txt"
    let MobyDick_file = @"C:\Data\Moby Dick.txt"
    let WikiTitles_file = @"C:\Data\metawiki-latest-all-titles"

    let f = readWordsInFile WikiTitles_file readAllLines
    let f2 = readWordsInFile WikiTitles_file readSingleLines
    let f3 = readWordsInFile WikiTitles_file readLines
    
    #time

    let d1, r1 = timeAFunction (countWords f)
    open System.Collections.Generic

    printfn "Function call with File.ReadAllLines() took %i milliseconds and returned %d words"  d1 r1

    let d2, r2 = timeAFunction (countWords f2)
    printfn "Function call with File.ReadLines() took %i milliseconds and returned %d words"  d2 r2

    let d3, r3 = timeAFunction (countWords f3)
    printfn "Function call with readSingleLines() took %i milliseconds and returned %d words"  d3 r3

    let isWordInSequence words =
        let wordSet = words |> Set.ofSeq
        fun word -> wordSet.Contains(word)

    let aliceWords = readWordsInFile AliceInWonderland_file readLines
    let isWordInAlice = isWordInSequence aliceWords

    isWordInAlice "Moby"

    let countWordInSequence words =
        let wordSet = new Dictionary<string, int>()
        words
        |> Seq.iter (fun word -> 
            if wordSet.ContainsKey(word) 
                then wordSet.[word] <- wordSet.[word] + 1
                else wordSet.[word] <- 1)
        fun word -> 
            if wordSet.ContainsKey(word) 
                then wordSet.[word] 
                else 0

    let countWordInAlice = countWordInSequence aliceWords

    countWordInAlice "Rabbit"

    let rec fibNoTail n = 
        if n <= 2L
            then 1L 
            else fibNoTail(n - 1L) + fibNoTail(n - 2L)

    fibNoTail 40L    

    let fibWithTail n =
        let rec tail n1 n2 = function
        | 0L -> n1
        | n -> tail n2 (n2 + n1) (n - 1L)
        tail 0L 1L n    

    fibWithTail 40L
    

module LargeDocuments

    open System
    open System.IO
    open System.Collections.Generic

    let WikiTitles_file = @"C:\Data\metawiki-latest-all-titles"

    let readLines fileName =
        fileName
        |> File.ReadLines 

    let readWordsInLine (line : string) =
        line.Split([|' ';'_';'\\';'/';' '|])
        |> Seq.ofArray
        |> Seq.map (fun s -> s.Trim())

    let readWordsInFile fileName =
        fileName
        |> readLines
        |> Seq.collect readWordsInLine

    //let wordCounts = new Dictionary<string, int>(20210000)
    let wikiTitlesWords = readWordsInFile WikiTitles_file

    let countWordInSequence words =
        let wordCounts = new Dictionary<string, int>()
        words
        |> Seq.iter (fun word -> 
            if wordCounts.ContainsKey(word) 
                then wordCounts.[word] <- wordCounts.[word] + 1
                else wordCounts.[word] <- 1)
        fun word -> 
            if wordCounts.ContainsKey(word) 
                then wordCounts.[word] 
                else 0

    let countWordsInWikiTitles = countWordInSequence wikiTitlesWords
    countWordsInWikiTitles "LexiSession"

    let isWordInSequence (words : string seq) =
        let wordSet = new HashSet<string>()
        let enumerator = words.GetEnumerator()
        fun word -> 
            if wordSet.Contains word
                then true
                else 
                    let mutable isFound = false
                    while not isFound do 
                        if enumerator.MoveNext() 
                            then
                                let w = enumerator.Current
                                wordSet.Add w |> ignore
                                if w = word
                                    then isFound <- true
                        
                    isFound

    let isWordInWikiTitles = isWordInSequence wikiTitlesWords
    isWordInWikiTitles "Australia"