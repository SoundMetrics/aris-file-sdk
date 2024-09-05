// Print the type marshalling size.

open Aris.FileTypes
open System
open System.Runtime.InteropServices

let printTypeSize typeName =

    let typ =
        match typeName with
        | "Aris.FileTypes.ArisFileHeader" -> typedefof<ArisFileHeader>
        | "Aris.FileTypes.ArisFrameHeader" -> typedefof<ArisFrameHeader>
        | _ -> failwith (sprintf "Unexpected type: '%s'" typeName)
    let size = Marshal.SizeOf(typ)
    printfn "%s is %d bytes" typ.FullName size

[<EntryPoint>]
let main argv =

    // Ensure the assembly is loaded.
    typedefof<ArisFileHeader>.FullName |> ignore

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
