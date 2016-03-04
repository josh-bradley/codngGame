(* Auto-generated code below aims at helping you parse *)
(* the standard input according to the problem statement. *)
open System

type Racer = { x: int; y: int; vx: int; vy: int; angle: int; nextCheckPointId: int}

let laps = int(Console.In.ReadLine())
let checkpointCount = int(Console.In.ReadLine())
for i in 0 .. checkpointCount - 1 do
    let token = (Console.In.ReadLine()).Split [|' '|]
    let checkpointX = int(token.[0])
    let checkpointY = int(token.[1])
    ()
    
let log msg = Console.Error.WriteLine(msg)
let readRacerFromConsole =  let token1 = (Console.In.ReadLine()).Split [|' '|]
                            {   
                                x = int(token1.[0]);
                                y = int(token1.[1]);
                                vx = int(token1.[2]);
                                vy = int(token1.[3]);
                                angle = int(token1.[4]);
                                nextCheckPointId = int(token1.[5]);
                            }

(* game loop *)
while true do
    for i in 0 .. 2 - 1 do
        let token1 = (Console.In.ReadLine()).Split [|' '|]
        let x = int(token1.[0])
        let y = int(token1.[1])
        let vx = int(token1.[2])
        let vy = int(token1.[3])
        let angle = int(token1.[4])
        let nextCheckPointId = int(token1.[5])
        ()

    for i in 0 .. 2 - 1 do
        let token2 = (Console.In.ReadLine()).Split [|' '|]
        let x = int(token2.[0])
        let y = int(token2.[1])
        let vx = int(token2.[2])
        let vy = int(token2.[3])
        let angle = int(token2.[4])
        let nextCheckPointId = int(token2.[5])
        ()

    
    (* Write an action using printfn *)
    (* To debug: Console.Error.WriteLine("Debug message") *)
    
    printfn "8000 4500 100"
    printfn "8000 4500 100"
    ()
