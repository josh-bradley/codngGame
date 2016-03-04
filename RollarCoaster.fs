(* Auto-generated code below aims at helping you parse *)
(* the standard input according to the problem statement. *)
module RollaCoaster
open System
open System.Collections.Generic

let token = (Console.In.ReadLine()).Split [|' '|]

let L = int(token.[0])
let C = int(token.[1])
let N = int(token.[2])
let pi : int array = Array.zeroCreate N
let progressiveTotal : Int64 array = Array.zeroCreate N

for i in 0..(N-1) do 
    pi.[i] <- int(Console.In.ReadLine())
    progressiveTotal.[i] <- (if i > 0 then progressiveTotal.[0] else 0L) + int64(pi.[i])

let rec fillNextRide startIndex endIndex total =  
    let range = match startIndex = 0 with
                | true -> endIndex + 1
                | false -> N - startIndex + endIndex + 1 

    match total + progressiveTotal.[startIndex] > int64(L) with
    | false ->  let multiplier = (startIndex + range / 2) / N
                let midPoint = (startIndex + range / 2) % 2
                let laTotal = total + progressiveTotal.[midPoint] - (if startIndex = 0 then 0L else progressiveTotal.[startIndex - 1]) + int64(multiplier) * (progressiveTotal.[progressiveTotal.Length - 1])

                match laTotal > int64(L) with 
                      | false -> fillNextRide (midPoint + 1) endIndex laTotal  
                      | true -> fillNextRide startIndex (midPoint - 1) total
    | true -> total, startIndex
    

let rec ride rideNum startIndex =
  let endIndex = (startIndex + N - 1) % N
  let totalThisRide, nextIndex = fillNextRide startIndex endIndex 0L
                
  match rideNum = C with
  | true -> totalThisRide
  | false -> totalThisRide + (ride (rideNum + 1) nextIndex)

  

let answer = ride 1 0
printfn "%d" answer