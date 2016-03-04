(* Save humans, destroy zombies! *)
module Zombie1
open System
open System.Collections.Generic

let killDist = 1999
let walkDist = 1000
let zWalkDist = 400
type Human = {
    Id: int
    X: int
    Y: int
    Dist: float
}

type Zombie = {
    Id: int
    X: int
    Y: int
    NextX: int
    NextY: int
    Dist: float
}

type ZombieStat = {
    humanDistScore: int
    dist: float
}

let calculateDist X1 Y1 X2 Y2 =
                    let xD = X1 - X2
                    let yD = Y1 - Y2
                    Math.Sqrt(float(xD*xD) + float(yD*yD))

let calcZombieToHuman (zombie : Zombie) (human: Human) =
                        calculateDist zombie.X zombie.Y human.X human.Y

(* game loop *)
while true do
    let token = (Console.In.ReadLine()).Split [|' '|]
    let x = int(token.[0])
    let y = int(token.[1])
    let humanCount = int(Console.In.ReadLine())
    
    
    let humans : Human array = 
        [|for i in 0 .. humanCount - 1 do
            let token1 = (Console.In.ReadLine()).Split [|' '|]
            let humanX = int(token1.[1])
            let humanY = int(token1.[2])
            let dist = calculateDist x y humanX humanY 
            yield {Id = int(token1.[0]); X = humanX; Y = humanY; Dist = dist}|]


    let zombieCount = int(Console.In.ReadLine())
    let zombies = 
        [|for i in 0 .. zombieCount - 1 do
            let token2 = (Console.In.ReadLine()).Split [|' '|]
            let zombieId = int(token2.[0])
            let zombieX = int(token2.[1])
            let zombieY = int(token2.[2])
            let zombieXNext = int(token2.[3])
            let zombieYNext = int(token2.[4])
            let dist = calculateDist x y zombieX zombieY
            yield {Id = zombieId; X = zombieX; Y = zombieY; NextX = zombieXNext; NextY = zombieYNext; Dist = dist}|]


    // Indanger humans (humans that are closest to one or more zombies)
    let zombieToHuman = zombies 
                            |> Array.map (fun z ->  let closestHuman = humans 
                                                                            |> Array.minBy (fun h -> calcZombieToHuman z h)
                                                    let zth = calcZombieToHuman z closestHuman
                                                    z, closestHuman, zth)
                            |> Array.filter (fun zh -> let z, h, zth = zh
                                                       z.Dist > zth)
//                            |> Array.filter (fun zh ->  let z1, h1, zth = zh
//                                                        let ath = calculateDist x y h1.X h1.Y
//                                                        (zth - float(400)) / float(zWalkDist) > (ath - float(1600)) / float(walkDist))

    let cantSave = zombieToHuman
                        |> Array.filter (fun zh ->  let z, h, zth = zh
                                                    let ath = calculateDist x y h.X h.Y
                                                    let zt = Math.Ceiling(zth / float(zWalkDist))
                                                    let at = Math.Ceiling((ath - float(1600)) / float(walkDist))
                                                    Console.Error.Write("Zombie:{0}, Dist:{1} ", z.Id, zth)
                                                    Console.Error.Write("Human:{0} ", h.Id)
                                                    Console.Error.WriteLine("Ash Dist:{0}", ath)
                                                    zt < at)
                        |> Array.map (fun zh -> let _, h, _ = zh
                                                Console.Error.WriteLine(h.Id)
                                                h)

    let zombieToHuman = zombieToHuman
                        |> Array.filter(fun zh -> let _, h, _ = zh
                                                  cantSave |> Array.exists (fun h1 -> h1 = h) = false)

    let tX, tY = match zombieToHuman.Length > 0 with
                    | true -> let zombie, human, zth = zombieToHuman |> Array.minBy (fun zh -> let z, h, zth = zh
                                                                                               zth)
                              human.X, human.Y
//                              match float(walkDist + killDist) > zombie.Dist with
//                                | true ->   let toTravel = zombie.Dist - float(killDist)
//                                            let r = toTravel / zombie.Dist
//                                            let x1 = float(zombie.NextX - x) * r
//                                            let y1 = float(zombie.NextY - y) * r
//                                            int(x1) + x, int(y1) + y
//                                | false -> zombie.NextX, zombie.NextY
                    | false -> x, y
//    let zombie = zombies 
//                    |> Array.minBy (fun z -> humans 
//                                                |> Array.map (fun h -> calculateDist z.X z.Y h.X h.Y) 
//                                                |> Array.min)




    //let zombie = zombies.[0]
    // How far away is each zombie from there nearest human
    // How far is the nearest zombie to humans 
    //  - Which zombie has the most closest humans?
    // How far are the zombies/humans from my dude.
    // Where are the most humans 
    // Where are the most zombies
    // Can you get to the human in time. 

    
    (* Write an action using printfn *)
    (* To debug: Console.Error.WriteLine("Debug message") *)
    
    printfn "%d %d" tX tY (* Your destination coordinates *)
    ()