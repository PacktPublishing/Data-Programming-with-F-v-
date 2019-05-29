module Agents

open System

type Agent<'T> = MailboxProcessor<'T>

type CounterMessage =
    | Increment of int
    | GetCount of AsyncReplyChannel<int>

let makeCounterAgent =
    let innerFunc () = 
        Agent.Start(
            fun mailbox ->
                let rec loop total =
                    async {
                        let! message = mailbox.Receive()
                        match message with
                        | Increment n
                            ->  return! loop (total + n)
                        | GetCount(channel)
                            ->  channel.Reply total
                                return! loop total
                    } 
                loop 0
        )
    innerFunc

type InventoryMessage =
    | AdjustInventory of string * int
    | GetBalanceForItem of string * AsyncReplyChannel<int>
    | GetBalanceForAll of AsyncReplyChannel<int>

let inventoryAgent =
    Agent.Start(fun mailbox ->
        let rec loop inventoryCache =
            async {
                let! msg = mailbox.Receive()
                match msg with
                | AdjustInventory (itemName, itemCount)
                    ->  match Map.tryFind itemName inventoryCache with
                        | None
                            ->  let itemCounter = makeCounterAgent ()
                                itemCounter.Post (Increment itemCount)
                                let newCache = inventoryCache.Add(itemName, itemCounter)
                                return! loop newCache
                        | Some (itemCounter)
                            ->  itemCounter.Post (Increment itemCount)
                                return! loop inventoryCache
                | GetBalanceForItem (itemName, channel)
                    ->  match Map.tryFind itemName inventoryCache with
                        | None
                            ->  channel.Reply 0
                                return! loop inventoryCache
                        | Some (itemCounter)
                            ->  let count = itemCounter.PostAndReply GetCount
                                channel.Reply count
                                return! loop inventoryCache
                | GetBalanceForAll channel
                    ->  let total =
                            inventoryCache
                            |> Map.fold (fun total _ itemCounter ->
                                let count = itemCounter.PostAndReply GetCount
                                total + count) 0
                        channel.Reply total
                        return! loop inventoryCache 
            }
        loop Map.empty)

inventoryAgent.PostAndReply GetBalanceForAll
inventoryAgent.PostAndReply (fun c -> GetBalanceForItem("sausages", c))
inventoryAgent.Post (AdjustInventory("sausages", 12))
inventoryAgent.Post (AdjustInventory("cheese", 8))
inventoryAgent.PostAndReply (fun c -> GetBalanceForItem("cheese", c))
inventoryAgent.PostAndReply GetBalanceForAll

