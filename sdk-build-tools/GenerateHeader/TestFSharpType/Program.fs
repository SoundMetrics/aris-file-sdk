// Print the type marshalling size.

open System
open System.Runtime.InteropServices

let printTypeSize typeName =

    let typ = Type.GetType(typeName)
    let size = Marshal.SizeOf(typ)
    printfn "%s is %d bytes" typ.FullName size

[<EntryPoint>]
let main argv =

    try
        match argv with
        | [| typeName |] -> printTypeSize typeName
                            0
        | [||] -> Console.Error.WriteLine("Too few arguments")
                  -1
        | _ -> Console.Error.WriteLine("Too many arguments")
               -2
    with
        ex -> Console.Error.WriteLine(sprintf "An error occurred: %s" ex.Message)
              -3
