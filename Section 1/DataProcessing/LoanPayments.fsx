//Loan_ID,loan_status,Principal,terms,effective_date,due_date,paid_off_time,past_due_days,age,education,Gender

open System.IO
open System

let lines = 
    File.ReadAllLines(@"C:\Data\Loan payments data.csv")
    |> Array.distinct
    |> Array.map (fun s -> s.Split(','))

let header = lines |> Array.take 1

let data = lines |> Array.skip 1

data.Length

type LoanPaymentData =
    {
        LoanId : string;
        LoanStatus : string;
        Principal : string;
        Terms : string;
        EffectiveDate : string;
        DueDate : string;
        PaidOffTime : string;
        PastDueDays : string;
        Age : string;
        Education : string;
        Gender : string
    }

let transformToLoanPaymentData (data : string array) =
    {
        
        LoanId = data.[0];
        LoanStatus = data.[1];
        Principal = data.[2];
        Terms = data.[3];
        EffectiveDate = data.[4];
        DueDate = data.[5];
        PaidOffTime = data.[6];
        PastDueDays = data.[7];
        Age = data.[8];
        Education = data.[9];
        Gender = data.[10]
    }

let paymentData =
    data
    |> Array.map transformToLoanPaymentData

[<Measure>] type dollar
[<Measure>] type terms
[<Measure>] type age
[<Measure>] type days

type LoanStatus =
    | PaidOff of PaidOffTime : DateTime
    | Collection of PastDueDays : int<days>
    | Collection_PaidOff of PaidOffTime : DateTime * PastDueDays : int<days>

type Education =
    | HighSchoolOrBelow
    | College
    | MasterOrAbove

type Gender =
    | Male
    | Female

type LoanPaymentData =
    {
        LoanId : string;
        LoanStatus : LoanStatus;
        Principal : int<dollar>;
        Terms : int<terms>;
        EffectiveDate : DateTime;
        DueDate : DateTime;
        Age : int<age>;
        Education : Education;
        Gender : Gender
    }

let transformToLoanStatus (status, paidOffTime, pastDueDays) =
    match status with
    | "PAIDOFF"
        ->  PaidOff(DateTime.Parse paidOffTime)
    | "COLLECTION"
        -> Collection((Int32.Parse(pastDueDays)) * 1<days>)
    | "COLLECTION_PAIDOFF"
        ->  Collection_PaidOff(DateTime.Parse paidOffTime, (Int32.Parse(pastDueDays)) * 1<days>)
    | x
        ->  failwith (sprintf "Unrecognized loan status: \"%s\"" x)

let transformToEduction = function
    |"High School or Below"
        -> HighSchoolOrBelow
    | "Bechalor"
    | "college"
        -> College
    | "Master or Above"
        -> MasterOrAbove
    | x
        ->  failwith (sprintf "Unrecognized eduction: \"%s\"" x)

let transformToGender = function
    | "male"
        -> Male
    | "female"
        -> Female
    | x
        ->  failwith (sprintf "Unrecognized gender: \"%s\"" x)

let transformToLoanPaymentData (data : string array) =
    {
        LoanId = data.[0];
        LoanStatus = transformToLoanStatus (data.[1], data.[6], data.[7]) ;
        Principal = Int32.Parse(data.[2]) * 1<dollar>;
        Terms = Int32.Parse(data.[3]) * 1<terms>;
        EffectiveDate = DateTime.Parse(data.[4]);
        DueDate = DateTime.Parse(data.[5]);
        Age = Int32.Parse(data.[8]) * 1<age>;
        Education = data.[9] |> transformToEduction;
        Gender = data.[10] |> transformToGender;
    }

let paymentData =
    data
    |> Array.map transformToLoanPaymentData

header

data
|> Array.map (fun d -> d.[10])
|> Array.exists(System.String.IsNullOrEmpty)

data
|> Array.filter(fun d -> System.String.IsNullOrEmpty(d.[6]) |> not)
|> Array.map (fun d -> d.[1])
|> Array.distinct

paymentData
|> Array.map (fun pd -> pd.Principal)
|> Array.distinct

type LoanStatus =
    | PaidOff 
    | Collection 
    | Collection_PaidOff 

let transformToLoanStatus status =
    match status with
    | "PAIDOFF"
        ->  PaidOff
    | "COLLECTION"
        -> Collection
    | "COLLECTION_PAIDOFF"
        ->  Collection_PaidOff
    | unknown
        ->  failwith (sprintf "Unrecognized loan status: \"%s\"" unknown)

        transformToLoanStatus data.[1]