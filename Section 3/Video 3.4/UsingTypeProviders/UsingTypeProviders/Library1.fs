module AzureTypeProvider

open FSharp.Azure.StorageTypeProvider

type Azure = AzureTypeProvider<"DefaultEndpointsProtocol=https;AccountName=rbroidastorage;AccountKey=doifo+6Lg1Q+5PqTTM06n+HpEQEEpDiV98GY07GLEcpe73GWVVX4bvy7uOSsOHHIvN9xXptOhonOXrPpry9BEw==;EndpointSuffix=core.windows.net">

Azure.Containers.allmyblobs.``Orders.csv``.Read()
Azure.Containers.allmyblobs.Upload("C:\\data\\Moby Dick.txt")

Azure
    .Tables
    .Employees
    .Query()
    .``Where Partition Key Is``
    .``Equal To``("Chemist")
    .Execute()
|> Array.map(fun row -> row.RowKey, row.JobTitle)
|> ignore
