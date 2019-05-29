open System
open FSharp.Data.TypeProviders

type dbSchema = 
    SqlDataConnection<"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OrdersDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False">

let db = dbSchema.GetDataContext()

let products = 
    query {
        for row in db.Product do
            where (row.Name <> "unknown")
            select row
}

products 
|> Seq.iter (fun r ->
    printfn "%d %s" r.Id r.Name )

let po, pr, item = 
    query {
        for item in db.OrderItems do
        join po in db.PurchaseOrder on (item.PurchaseOrderId = po.Id)
        join pr in db.Product on (item.ProductId = pr.Id)
        select (po, pr, item)
    }
    |> Seq.head

let pr = 
    query {
        for row in db.Product do
            where (row.Name <> "unknown")
            select row
    }
    |> Seq.head

pr.OrderItems

let po = db.PurchaseOrder |> Seq.head

(po.OrderItems |> Seq.head).Product.Name

// function to turn a value into a nullable
let nullable value = new System.Nullable<_>(value)

// passing a nullable value to a stored procedure
let callProcedure1 a b =
  let results = db.
  for result in results do
    printfn "%d" (result.TestData1.GetValueOrDefault())
  results.ReturnValue :?> int

printfn "Return Value: %d" (callProcedure1 10 20)

let newPO = 
    new dbSchema.ServiceTypes.PurchaseOrder(
        Id = 300,
        Customer = "Booth Tarkington",
        Orderdate = DateTime.Now.AddDays(-38.0),
        DeliveryCode = 4
    )
    
let newItems =
    [ 
        new dbSchema.ServiceTypes.OrderItems (
            PurchaseOrderId = 300,
            ProductId = 3,
            Quantity = 12
        )
        new dbSchema.ServiceTypes.OrderItems (
            PurchaseOrderId = 300,
            ProductId = 1,
            Quantity = 3
        )
        new dbSchema.ServiceTypes.OrderItems (
            PurchaseOrderId = 300,
            ProductId = 5,
            Quantity = 60
        )
    ]

db.PurchaseOrder.InsertOnSubmit(newPO)
db.OrderItems.InsertAllOnSubmit(newItems)

try
    db.DataContext.SubmitChanges()
    printfn "Successfully inserted new rows."
with
    | exn -> printfn "Exception:\n%s" exn.Message

