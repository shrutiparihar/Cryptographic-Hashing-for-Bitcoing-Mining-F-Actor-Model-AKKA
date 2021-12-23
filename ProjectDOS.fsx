open System.Numerics
#time "on"
#r "nuget:Akka.Fsharp"
#r "nuget:Akka.Testkit"

open System
open System.Diagnostics
open Akka.Actor
open Akka.Configuration
open Akka.FSharp
open Akka.TestKit
open System.Security.Cryptography
open System.Text
open System.Numerics

//Code to compute CPU Time and Real Time
let coreCount = Environment.ProcessorCount
let timer = Stopwatch()
timer.Start()
let pId = Process.GetCurrentProcess()
let cpu_time_stamp = pId.TotalProcessorTime


type Message =
    | SquareMessage of int*int
    | Answer of int * bool

let randomfun i: string =    //function that return i length of random string
    let randomWord =   //function to generate random string
              let R = System.Random()
              fun n -> System.String [|for _ in 1..n -> R.Next(93) + 33 |> char|]

    randomWord i

let generatehash (s3 : String) : string=  //generate hash 
    //printfn"Random String %s"s3

    let hash2 = 
       s3
       |> Encoding.ASCII.GetBytes
       |> (new SHA256Managed()).ComputeHash
       |> System.BitConverter.ToString

    let finhash = hash2.Replace("-","")
    //printfn"Hash: %s" finhash
    finhash

let countzero (hash1:string) (k:int) :bool=     //return true if it has k number of zeros
    let mutable count1=0
    let mutable i = 0
    let mutable check1 = true
    while (i < k+1 ) do
        if(hash1.Chars(i)='0') then
             count1<-count1+1  
        else
            i<-k
        i <- i + 1
    if(count1=k) then
        check1<-true
    else 
       check1<-false
    check1

let findcoin K (mailbox: Actor<'a>) = 
    let rec listener = 
        actor {
            let! msg = mailbox.Receive()
            match msg with
            | SquareMessage(idx,K) ->
                let mutable length= 10
                let s1= "shruti.parihar"
                let s2=randomfun length
                let s3= s1+s2
                let hash1= generatehash s3 //hash contains the hash for specified length
                let perfzero:bool=countzero hash1 K   //check if the hash has k number of zero 
                if perfzero = true then printfn "%s \t %s" s3 hash1
                mailbox.Sender() <! Answer(idx, perfzero) 
            return! listener
        }

    listener



let mainActor K  (mailbox: Actor<'a>) =
    let maxSpawn = 10

    let childRef = 
        [1 .. maxSpawn]
        |> List.map(fun id -> 
                        let name = sprintf "child%d" id
                        spawn mailbox name (findcoin K))
    [1 .. maxSpawn] |> List.map(fun id -> childRef |> List.item(id-1) <! SquareMessage(id-1, K)) |> ignore

    let rec listener childRef K=
        actor {
            let! msg = mailbox.Receive()
            match msg with
            |Answer(idx, boolval) ->
                if boolval= true then
                    mailbox.Context.System.Terminate() |> ignore
                if idx < maxSpawn then 
                    childRef |> List.item(idx) <! SquareMessage(idx, K)
                return! listener childRef (K)
            | _ -> return! listener childRef K
        }
    listener childRef K

let main(args) =
    let K = int fsi.CommandLineArgs.[1]
    
    let system = System.create "system" (Configuration.defaultConfig())
    
    let mainA = spawn system "mainActor" (mainActor K)
    
    system.WhenTerminated.Wait()

    0

main()