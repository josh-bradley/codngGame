(* Save humans, destroy zombies! *)
module ZombieW4
open System
open System.Collections.Generic

let killDist = 2000
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

let sq x = x * x

let avgZombieDist (x, y) (zombies : Zombie array) = 
                    zombies |> Array.averageBy (fun z -> calculateDist x y z.X z.Y)

let zombiesWithinReach (x, y) (zombies : Zombie array) = 
                            zombies |> Array.filter (fun z -> calculateDist x y z.X z.Y < float(killDist))
                            |> fun results -> results.Length


let calculateZInterception (z : Zombie) (x : int) (y : int) (h : Human) (zombies : Zombie array option) =
                                let ox = float(z.X - x)
                                let oy = float(z.Y - y)
                                let vx = float(z.NextX - z.X)
                                let vy = float(z.NextY - z.Y)

                                let h1 = sq vx + sq vy - sq (float(walkDist))
                                let h2 = ox * vx + oy * vy

                                let t = match h1 = 0.0 with
                                        | true -> -(pown ox 2 + pown oy 2) / float(2)*h2
                                        | false -> let minusPhalf = -h2 / h1
                                               
                                                   let discriminant = sq minusPhalf - (sq ox + sq oy) / h1
                                                   //if discriminant = 0 then Console.Error.WriteLine("Disi 0")
    //		                                        if (discriminant < 0) { // no (real) solution then...
    //			                                        return null;
    //                                              }
                                                   let root = Math.Sqrt(discriminant)
 
                                                   let t1 = float(minusPhalf) + root
                                                   let t2 = float(minusPhalf) - root

                                                   let tMin = Math.Min(t1, t2)
                                                   let tMax = Math.Max(t1, t2)

                                                   match tMin > float(0) with
                                                   | true -> tMin 
                                                   | false -> tMax // get the smaller of the two times, unless it's negative
    //		                                       if (t < 0) { // we don't want a solution in the past
    //			                                        return null;
    //		                                        }

                                match t < float(0) with
                                | true -> h.X, h.Y
                                | false -> 
                                           let zth = calcZombieToHuman z h
                                           let ztht = Math.Ceiling(float(zth) / float(zWalkDist))
                                           Console.Error.WriteLine("time:{0}, tohumantime:{1}", t, ztht)
                                           let t = Math.Min(ztht, t)
                                           let tx = int(float(z.X) + t* vx)
                                           let ty = int(float(z.Y) + t * vy)
                                           Console.Error.WriteLine("x: {0}, y:{1}", tx, ty)
//                                           let zth = calcZombieToHuman z h
//                                           let sti = calculateDist z.X z.Y tx ty
//                                           let tx, ty = match zth > sti with
//                                                        | true -> tx, ty
//                                                        | false -> let diff = sti - zth
//                                                                   let r = diff / sti
//                                                                   let x1 = float(tx - z.X) * r
//                                                                   let y1 = float(ty - z.Y) * r
//                                                                   int(x1) + z.X, int(y1) + z.Y
                                           //tx, ty
                                           let toKillSpot = calculateDist x y tx ty
                                           //match toKillSpot < float(killDist) with
                                           //| true -> x, y
                                           //| false -> 
                                           let toTravel = toKillSpot - float(killDist - 2)
                                                        // when toTravel is less than 1000 work out the best posish
                                           let r = toTravel / toKillSpot
                                           let x1 = float(tx - x) * r
                                           let y1 = float(ty - y) * r
                                           let altX, altY = Math.Max(int(x1) + x, 0), Math.Max(int(y1) + y, 0)
                                           match toTravel < float(walkDist) && zombies.IsSome with 
                                            | false -> altX, altY
                                            | true -> [|(altX, altY); (tx, ty); (z.X, z.Y); (h.X, h.Y)|] 
                                                        |> Array.maxBy (fun pos -> zombiesWithinReach pos zombies.Value)
                                                      
    //                                                                let altAvg = avgZombieDist altX altY zombies.Value 
    //                                                                let avg = avgZombieDist tx ty zombies.Value
    //                                                                match avg > altAvg with
    //                                                                | false -> Math.Max(tx, 0), Math.Max(ty, 0)
    //                                                                | true -> altX, altY


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
                                                    let xi, yi = calculateZInterception z x y h None
                                                    let zti = calculateDist xi yi z.X z.Y
                                                    let ath = calculateDist x y xi yi
                                                    let zt = Math.Ceiling(zth / float(zWalkDist))
                                                    let at = Math.Ceiling(ath / float(walkDist))
                                                    Console.Error.Write("Zombie:{0}, Dist:{1} ", z.Id, zth)
                                                    Console.Error.Write("Human:{0} ", h.Id)
                                                    Console.Error.WriteLine("Ash Dist:{0}", ath)
                                                    Console.Error.WriteLine("zt: {0}, at:{1}", zt, at)
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
                              

                              let ztht = zth / float(zWalkDist)

                              let x1, x2 = calculateZInterception zombie x y human (Some zombies)
                              let dist = calculateDist x y x1 x2
                              match dist / float(walkDist) < 1.0 && ztht > 1.0 && humans.Length = 1 with
                              | true -> x, y
                              | false -> x1, x2
                              //human.X, human.Y
//                              match float(walkDist + killDist) > zombie.Dist with
//                                | true ->   let toTravel = zombie.Dist - float(killDist)
//                                            let r = toTravel / zombie.Dist
//                                            let x1 = float(zombie.NextX - x) * r
//                                            let y1 = float(zombie.NextY - y) * r
//                                            int(x1) + x, int(y1) + y
//                                | false -> zombie.NextX, zombie.NextY
                    | false ->  zombies |> Array.minBy (fun z -> calculateDist x y z.X z.Y)
                                        |> fun z -> z.X, z.Y
//    let zombie = zombies 
//                    |> Array.minBy (fun z -> humans 
//                                                |> Array.map (fun h -> calculateDist z.X z.Y h.X h.Y) 
//                                                |> Array.min)



//    let toTravel = calculateDist x y tX tY
//    let tX, tY = match toTravel > float(killDist) with
//                | true -> tX, tY
//                | false -> x, y
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
    let tX = Math.Max(tX, 0)
    let tY = Math.Max(tY, 0)
    printfn "%d %d" tX tY (* Your destination coordinates *)
    ()