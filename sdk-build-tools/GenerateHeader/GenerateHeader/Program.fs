// Parse a type definition and generate source code.

open FieldParser
open System
open System.IO
open System.Text

let validateInputs (options : ProgramOptions.Options) =

    let checkInputFolder () =
        if not (Directory.Exists(options.InputFolder)) then
            failwith (sprintf "Input folder '%s' does not exist" options.InputFolder)

    let checkOutputPath () =
        let folder = Path.GetDirectoryName(options.OutputPath)
        try
            Directory.CreateDirectory(folder) |> ignore
        with
            _ -> ()

        if not (Directory.Exists(folder)) then
            failwith (sprintf "Output folder '%s' could not be created" folder)

    checkInputFolder()
    checkOutputPath()

let openFiles folder =
    let openFile path =
        File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read)

    let fieldInput = openFile (Path.Combine(folder, "FieldInput.txt"))
    let typeInput = openFile (Path.Combine(folder, "TypeInput.txt"))

    fieldInput, typeInput

let getProducer (language : string) =

    match language.ToUpper() with
    | "C" -> CProducer.produce
    | "C#" -> CSharpProducer.produce
    | lang -> failwith (sprintf "Unexpected language requested: '%s'" lang)

[<EntryPoint>]
let main argv =

    let options = ProgramOptions.Options()
    if CommandLine.Parser.Default.ParseArguments(argv, options) then
        try
            validateInputs options

            printfn "Input folder: %s" (Path.GetFullPath(options.InputFolder))
            printfn "Output file:  %s" (Path.GetFullPath(options.OutputPath))
            printfn "Language:     %s" options.Language

            let filename = Path.GetFileName(options.OutputPath)
            let producer = getProducer options.Language
            let fieldInput, typeInput = openFiles options.InputFolder
            use output = new StreamWriter(
                            File.Open(options.OutputPath, FileMode.Create, FileAccess.Write, FileShare.None))

            processStream filename options.Modifier producer typeInput fieldInput output

            0
        with
            ex -> Console.Error.WriteLine(sprintf "An error occurred: %s" ex.Message)
                  -1
    else
        Console.Error.WriteLine("Invalid options.")
        -2
