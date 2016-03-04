module Play
open System

let n = int(Console.In.ReadLine())
let words = [|for i in 0 .. n - 1 do yield Console.In.ReadLine()|]
let letters = Console.In.ReadLine().ToCharArray()

//let words = [|
//               "some"
//               "first"
//               "potsie"
//               "day"
//               "could"
//               "postie"
//               "from"
//               "have"
//               "back"
//               "this"
//            |]
//
//let letters = "sopitez".ToCharArray()
let getLetterVal (letter : char) = 
    match letter with
    | 'a' | 'e' | 'i' | 'o' | 'n' | 'r' | 't' | 'l' | 's' | 'u' -> 1
    | 'd' | 'g' -> 2
    | 'b' | 'c' | 'm' | 'p'  -> 3
    | 'f' | 'h' | 'v' | 'w' | 'y'  -> 4
    | 'k' -> 5
    | 'j' | 'x' -> 8 
    | 'q' | 'z' -> 10



let canBuild (word : string) = 
    let used = letters |> Array.copy
    word.ToCharArray() 
    |> Array.forall (fun x -> Array.tryFindIndex (fun y -> y = x) used 
                                |> (fun y -> match y with 
                                                | None -> false
                                                | Some idx ->   used.[idx] <- '-'
                                                                true))


//let canBuild (word : string) =
//    let words = word.ToCharArray() 
//    letters 
//    |> Array.filter (fun x -> Array.exists (fun y -> y = x) letters)


let getWordPoints (word : string) =
    word.ToCharArray()  
    |> Array.sumBy getLetterVal

let (points, maxWord, i) = words 
                            |> Array.filter canBuild
                            |> Array.mapi (fun i word -> (getWordPoints word, word, i))
                            |> Array.maxBy (fun (points, word, i) -> points)
printf "%s" maxWord