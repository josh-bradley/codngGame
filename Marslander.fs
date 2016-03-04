(* Auto-generated code below aims at helping you parse *)
(* the standard input according to the problem statement. *)
open System
open System.Collections.Generic

let surfaceN = int(Console.In.ReadLine()) (* the number of points used to draw the surface of Mars. *)
type Position = {
                    X : int
                    Y : int
                }
let arr = [|for i in 0 .. surfaceN - 1 do
            (* landX: X coordinate of a surface point. (0 to 6999) *)
            (* landY: Y coordinate of a surface point. By linking all the points together in a sequential fashion, you form the surface of Mars. *)
            let words = (Console.In.ReadLine()).Split [|' '|]
            let landX = int(words.[0])
            let landY = int(words.[1])

            yield { X = landX; Y = landY}|]
   
let mutable startFlatPositionIdx = -1

let endFor = arr.Length - 2
for i in 0..endFor do
    if arr.[i].Y = arr.[i+1].Y then
        startFlatPositionIdx <- i

let startOfFlatPos = arr.[startFlatPositionIdx]
let endOfFlatPos = arr.[startFlatPositionIdx + 1]
let startOfFlatX = startOfFlatPos.X
let endOfFlatX = endOfFlatPos.X

(* game loop *)
while true do
    (* hSpeed: the horizontal speed (in m/s), can be negative. *)
    (* vSpeed: the vertical speed (in m/s), can be negative. *)
    (* fuel: the quantity of remaining fuel in liters. *)
    (* rotate: the rotation angle in degrees (-90 to 90). *)
    (* power: the thrust power (0 to 4). *)
    let words = (Console.In.ReadLine()).Split [|' '|]
    let X = int(words.[0])
    let Y = int(words.[1])
    let hSpeed = int(words.[2])
    let vSpeed = int(words.[3])
    let fuel = int(words.[4])
    let rotate = int(words.[5])
    let power = int(words.[6])
    
    Console.Error.WriteLine(sprintf "%d %d" startOfFlatX endOfFlatX)
    Console.Error.WriteLine(sprintf "StartFX: %d EndFX: %d" startOfFlatX endOfFlatX)
    let inXRange = X > startOfFlatX && X < endOfFlatX
    let distTillFlat = if X < startOfFlatX then startOfFlatX - X else X - endOfFlatX 
    let maxHSpeed = Math.Max(20, Math.Min(60, distTillFlat / 10))

    let tooHFast = Math.Abs(hSpeed) > maxHSpeed
    let tillGround = if vSpeed <> 0 then (Y - startOfFlatPos.Y) / vSpeed else 0;
    Console.Error.WriteLine(sprintf "Till ground %d" tillGround)
    let needToDoHCorrection = (distTillFlat < 1500 && tooHFast) || (Math.Abs(tillGround) > 5 && inXRange)

    Console.Error.WriteLine(sprintf "distTillFlat: %d maxHSpeed: %d" distTillFlat maxHSpeed)
    
    let temp =  int (float hSpeed * float 0.5)



    let rotateAdj = if X < startOfFlatX && not tooHFast then -20 elif endOfFlatX < X && not tooHFast then 20 elif not needToDoHCorrection then 0 else Math.Sign(hSpeed) * Math.Min(Math.Abs(hSpeed), 80)

 //   let rotateAdj = if X < startOfFlatX then -20 else if X > endOfFlatX then 20 elif Math.Abs(hSpeed) >= 20 then temp else 0

    Console.Error.WriteLine(sprintf "inXRange: %b" inXRange)

    let idx = Array.findIndex (fun pos -> pos.X > X) arr
    let sectionLength = arr.[idx].X - arr.[idx - 1].X
    let p = (X - arr.[idx - 1].X) * 100 / sectionLength
    let sectionStartY = arr.[idx - 1].Y
    let sectionEndY = arr.[idx].Y
    let currentLandY = (sectionStartY - sectionEndY) / 100 * p + sectionStartY

    Console.Error.WriteLine(sprintf "Current ground %d" currentLandY)
    Console.Error.WriteLine(sprintf "To Ground %d" (currentLandY - Y) )
    let needUpLift = vSpeed < 0 && currentLandY - Y < vSpeed * -8 && ((Math.Abs(hSpeed) > 20) || not inXRange || vSpeed < -39) 
    let thrustAdj = if needUpLift || not inXRange then 4 else 3
    
    (* Write an action using printfn *)
    (* To debug: Console.Error.WriteLine("Debug message") *)
    
    printfn "%d %d" rotateAdj thrustAdj (* rotate power. rotate is the desired rotation angle. power is the desired thrust power. *)
    ()