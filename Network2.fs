(* Auto-generated code below aims at helping you parse *)
(* the standard input according to the problem statement. *)
module Network2
open System
open System.Collections.Generic

(* N: the total number of nodes in the level, including the gateways *)
(* L: the number of links *)
(* E: the number of exit gateways *)
let words = (Console.In.ReadLine()).Split [|' '|]
let N = int(words.[0])
let L = int(words.[1])
let E = int(words.[2])

let mappings = new Dictionary<int, List<int>>()

let addToMapping n1 n2 = 
        let res = match mappings.ContainsKey(n1) with
                    | true -> mappings.[n1]
                    | false ->  let nodes = new List<int>()
                                mappings.[n1] <- nodes
                                nodes
        res.Add(n2)
for i in 0 .. L - 1 do
    (* N1: N1 and N2 defines a link between these nodes *)
    let words = (Console.In.ReadLine()).Split [|' '|]
    let N1 = int(words.[0])
    let N2 = int(words.[1])

    addToMapping N1 N2
    addToMapping N2 N1
    ()

let gateways = [|for i in 0..E - 1 do yield int(Console.In.ReadLine())|]

type linkResult = {
    Node1 : int
    Dist : int
} 

let mutable visitedNodes = new Dictionary<int, int>()

let hasVistedNodeAtCloserDist node dist =
        match visitedNodes.ContainsKey(node) with
            | false ->  visitedNodes.Add(node, dist)
                        false
            | true ->   match visitedNodes.[node] <= dist with
                            | true -> true
                            | false ->  visitedNodes.[node] <- dist
                                        false
                        

let isNodeGateway n =
        Array.exists (fun x -> x = n) gateways

let writeDebug (s : string) =  
        Console.Error.WriteLine(s)

let rec travelTree node (tNodes : List<int>) dist closest exits =
            
            //Console.Error.WriteLine("This is the node:" + node.ToString())
            match hasVistedNodeAtCloserDist node dist || isNodeGateway node with
            | true -> None
            | false ->  let ajExits = Seq.where isNodeGateway mappings.[node] |> Seq.length
                        match (ajExits + exits) >= dist with
                        | true ->   writeDebug ("Node death spin:" + node.ToString())
                                    Some { Node1 = node; Dist = (node * -1) }
                        | false ->  tNodes.Add(node) 
                                    Seq.map (fun x -> (travelTree x (new List<int>(tNodes)) (dist + 1) closest (exits + ajExits))) mappings.[node] 
                                    |> Seq.minBy (fun y -> match y with | Some p -> p.Dist | None -> Int32.MaxValue)
                                    |> fun x -> match ajExits > 0 && x.IsNone with
                                                    | true -> Some { Node1 = node; Dist = dist}
                                                    | false -> x

(* game loop *)
while true do
    let SI = int(Console.In.ReadLine()) (* The index of the node on which the Skynet agent is positioned this turn *)
    (* Write an action using printfn *)
    (* To debug: Console.Error.WriteLine("Debug message") *)
    let node = travelTree SI (new List<int>()) 1 Int32.MaxValue 0 
    Console.Error.WriteLine("Success or : " + node.Value.Dist.ToString() + " NODE:" + node.Value.Node1.ToString())
    visitedNodes <- new Dictionary<int, int>()
    
    //for i in mappings do Console.Error.WriteLine(i.Key.ToString())
    let linkedExitNode = mappings.[node.Value.Node1]
                        |> Seq.find isNodeGateway
    let success = mappings.[node.Value.Node1].Remove(linkedExitNode)

    printfn "%d %d" node.Value.Node1 linkedExitNode (* Example: 3 4 are the indices of the nodes you wish to sever the link between *)
    ()
