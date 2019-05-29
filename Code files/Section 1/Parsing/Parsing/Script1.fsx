module ParsingLib

(*
#r @"..\Parsing\packages\FParsec.1.0.3\lib\net40-client\FParsecCS.dll"
#r @"..\Parsing\packages\FParsec.1.0.3\lib\net40-client\FParsec.dll"
*)

open FParsec

let parseA = pchar<unit> 'A'
parseA "A"
run parseA "A"
run parseA "ABC"

let testParser parser =
    let innerFunc input =
        match run parser input with
        | Success(result, _, remainderPos)
            ->  printfn "Success: %A" result
                printfn "Rest of input: %s" (input.Substring(int32(remainderPos.Index)))
        | Failure(errorMessage, _, _)
            ->  printfn "Failure: %s" errorMessage
    innerFunc

testParser parseA "ABC"

let parseB = pchar 'B'

let parseAorB = 
    parseA <|> parseB

testParser parseAorB "ABC"
testParser parseAorB "BAC"
testParser (many parseAorB) "ABC"

testParser pint32 "123 more stuff"
testParser pint32 "123,456,789"

let parseInts =
    sepBy pint32<unit> (pchar ',')

testParser parseInts "123,456,789"
testParser parseInts "123,456,789 000"

let parseDelimitedData pdata pdelim =
    sepBy pdata pdelim 

let parseInts = 
    parseDelimitedData pint32<unit> (pchar<unit> ',') 

let dataInQuotes p = 
    between (skipChar<unit> '"') (skipChar '"') p

let intInQuotes = dataInQuotes pint32

testParser intInQuotes "\"248\" more stuff"

let stringInQuotes = 
    dataInQuotes (manyChars (satisfy (fun c -> c <> '"')))

testParser stringInQuotes "\"Test Test\""

let decimalInQuotes = stringInQuotes |>> decimal
let datetimeInQuotes = 
    stringInQuotes 
    |>> fun s -> DateTime.ParseExact(s, "yyyy-mm-dd", null)

testParser datetimeInQuotes"\"2018-01-24\""
testParser decimalInQuotes "\"235.96\""

let skipCommaDelimiter p =
    p .>> skipChar<unit> ',' .>> spaces
