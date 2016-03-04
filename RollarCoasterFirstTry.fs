(* Auto-generated code below aims at helping you parse *)
(* the standard input according to the problem statement. *)
module RollaCoaster1
open System
open System.Collections.Generic

let token = (Console.In.ReadLine()).Split [|' '|]

let L = int(token.[0])
let C = int(token.[1])
let N = int(token.[2])
let pi = [|for i in 1..N do yield int(Console.In.ReadLine())|]

let rec ride idx cur (total : Int64) (rideNum : int) groupCount =
  let groupSize = pi.[idx]
  let newCur = cur + groupSize
  let newIdx = (idx + 1) % pi.Length
  //Console.Error.WriteLine(rideNum)
  let newTotal = match idx = 0 && groupCount = 1 && total > int64(0) && C / rideNum > 1 with
                  | true -> let d = C / (rideNum - 1)
                            Console.Error.WriteLine(idx)
                            let total = int64(d) * total
                            Console.Error.Write("before: ")
                            Console.Error.WriteLine(rideNum)
                            Console.Error.WriteLine("{0} {1}", d, total)
                            let rideNum = d * (rideNum - 1) + 1
                            Console.Error.Write("After: ")
                            Console.Error.WriteLine(rideNum)
                            ride idx 0 total rideNum 1
                  | false ->  
                        match rideNum > C with 
                        | true -> total
                        | false -> 
                            match newCur > L || groupCount > pi.Length with 
                            | true -> ride idx 0 (total + (int64 cur)) (rideNum + 1) 1
                            | false -> ride newIdx newCur total rideNum (groupCount + 1)

  newTotal


let answer = ride 0 0 (int64 0) 1 1
printfn "%d" answer