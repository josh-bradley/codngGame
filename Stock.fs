(* Auto-generated code below aims at helping you parse *)
(* the standard input according to the problem statement. *)
open System

let n = int(Console.ReadLine())
let vs = [|for s in Console.ReadLine().Split([|' '|]) do yield int(s)|]


let rec findLowest start (arr: int[]) idx lowest = 
    let newIndex = idx + 1
    if newIndex >= arr.Length then
        lowest
    elif start <= arr.[newIndex] then 
        findLowest arr.[newIndex] arr newIndex lowest 
    else
        let thisDiff = arr.[newIndex] - start
        if lowest > thisDiff then thisDiff else lowest
        |> findLowest start arr newIndex
        
let result = findLowest vs.[0] vs 0 0

printfn "%d" result