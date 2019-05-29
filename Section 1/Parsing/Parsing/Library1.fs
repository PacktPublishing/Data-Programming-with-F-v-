module OrdersParser

open System
open System.IO

open FParsec

type Order =
    {
        OrderNumber : string
        CustomerName : string
        OrderDate : DateTime
        ShipVia : string
        Items : Item list
    }
and Item =
    {
        ProductNumber : string
        ProductName : string
        Quantity : int
        Price : decimal
    }

type ParsedData =
    | OrderData of Order
    | ItemData of Item

let input = File.ReadAllText("C:\Data\Orders.csv")

let testParser parser =
    let innerFunc input =
        match run parser input with
        | Success(result, _, remainderPos)
            ->  printfn "Success: %A" result
                printfn "Rest of input: %s" (input.Substring(int32(remainderPos.Index)))
        | Failure(errorMessage, _, _)
            ->  printfn "Failure: %s" errorMessage
    innerFunc

let dataInQuotes p = 
    between (skipChar<unit> '"') (skipChar '"') p
let intInQuotes = dataInQuotes pint32
let stringInQuotes = 
    dataInQuotes (manyChars (satisfy (fun c -> c <> '"')))
let decimalInQuotes = stringInQuotes |>> decimal
let datetimeInQuotes = 
    stringInQuotes 
    |>> fun s -> DateTime.ParseExact(s, "yyyy-mm-dd", null)

let skipCommaDelimiter p =
    p .>> skipChar<unit> ',' .>> spaces

let parseItemData =
    pipe4 
        (stringInQuotes |> skipCommaDelimiter) 
        (stringInQuotes |> skipCommaDelimiter) 
        (intInQuotes |> skipCommaDelimiter) 
        decimalInQuotes
        (fun productNumber productName quantity price ->
            ItemData(
                {
                    ProductNumber = productNumber
                    ProductName = productName
                    Quantity = quantity
                    Price = price
                })
            )

testParser parseItemData "\"P765J\", \"Bongos\", \"1\", \"68.98\""

let parseOrderData =
    pipe4 
        (stringInQuotes |> skipCommaDelimiter) 
        (stringInQuotes |> skipCommaDelimiter) 
        (datetimeInQuotes |> skipCommaDelimiter) 
        stringInQuotes
        (fun orderNumber customerName orderDate shipVia ->
            OrderData(
                {
                    OrderNumber = orderNumber 
                    CustomerName = customerName 
                    OrderDate = orderDate 
                    ShipVia = shipVia 
                    Items = []
                })
            )

testParser parseOrderData "\"1234\", \"Jane Miller\", \"2017-03-23\", \"FedEx\""

let parseLine =
    dataInQuotes (pchar<unit> 'O' <|> pchar<unit> 'I') 
    |> skipCommaDelimiter
    >>= fun c -> 
            if c = 'O'
                then parseOrderData
                else parseItemData
testParser parseLine "\"O\", \"1234\", \"Jane Miller\", \"2017-03-23\", \"FedEx\""
testParser parseLine "\"I\", \"P765J\", \"Bongos\", \"1\", \"68.98\""
testParser parseLine input

let parseLines = 
    sepBy parseLine newline .>> eof

testParser parseLines input

let toOrders parsedData =
    let transformToOrders orders data = 
        match data with
        | OrderData order
            ->  order::orders
        | ItemData item
            ->  let currentOrder::restOfOrders = orders
                let items = item::currentOrder.Items
                let newOrder = { currentOrder with Items = items }
                newOrder::restOfOrders

    parsedData 
    |> List.fold transformToOrders [] 
    |> List.rev

let parseOrders = parseLines |>> toOrders
    
testParser parseOrders input