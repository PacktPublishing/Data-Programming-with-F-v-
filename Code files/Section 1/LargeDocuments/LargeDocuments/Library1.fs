module LargeDocuments

    let rec fibNoTail n = 
        if n <= 2L
            then 1L 
            else fibNoTail(n - 1L) + fibNoTail(n - 2L)

    fibNoTail 42L

    let fibWithTail n =
        let rec tail n1 n2 counter = 
            match counter with
            | 0L -> n1
            | n -> tail n2 (n2 + n1) (n - 1L)

        tail 0L 1L n    

    fibWithTail 4000L
