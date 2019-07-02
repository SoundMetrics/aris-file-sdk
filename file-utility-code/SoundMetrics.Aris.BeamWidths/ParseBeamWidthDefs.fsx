open System
open System.IO
open System.Text.RegularExpressions

let writeTypePrefix ns typeName (output: TextWriter) =
    fprintfn output "namespace %s" ns
    fprintfn output "{"
    fprintfn output "    internal static class %s" typeName
    fprintfn output "    {"
    fprintfn output "        internal static readonly BeamInfo[] BeamWidths ="
    fprintfn output "        {"

let writeTypeSuffix _ns _typeName (output: TextWriter) =
    fprintfn output "        };"
    fprintfn output "    }"
    fprintfn output "}"

type TrimmedString = TrimmedString of string

type Unknown = Unknown of string

type Definition = {
    BeamNumber: uint32
    Center:     float32
    Left:       float32
    Right:      float32
}
with
    override me.ToString() =
        sprintf
            "            new BeamInfo { BeamNumber = %u, Center = %ff, Left = %ff, Right = %ff },"
            me.BeamNumber
            me.Center
            me.Left
            me.Right

let (|Whitespace|_|) (TrimmedString s) =

    if String.IsNullOrWhiteSpace(s) then
        Some ()
    else
        None

let (|Comment|_|) (TrimmedString s) =

    if s.StartsWith("//") then
        Some s
    else
        None

let (|Int|_|) (g: Group) =
    match UInt32.TryParse(g.Value) with
    | true, value -> Some value
    | _ -> None

let (|Single|_|) (g: Group) =
    match Single.TryParse(g.Value) with
    | true, value -> Some value
    | _ -> None

let pattern =
    "DEFINE_BEAMWIDTH3\s*\("
    + "\s*(?<num>\d+)\s*,"
    + "\s*(?<center>[+-]?\d+(?:\.\d*)?)\s*,"
    + "\s*(?<left>[+-]?\d+(?:\.\d*)?)\s*,"
    + "\s*(?<right>[+-]?\d+(?:\.\d*)?)"
    + "\s*\)"

let regex = Regex(pattern)

let (|Definition|_|) (TrimmedString s) =

    let m = regex.Match(s)
    if m.Success then
        let g = m.Groups
        match g.["num"], g.["center"], g.["left"], g.["right"] with
        | Int num, Single center, Single left, Single right ->
            Some {
                BeamNumber = num
                Center = center
                Left = left
                Right = right
            }
        | _ -> None
    else
        None

let isComment = function
    | Comment _ -> true
    | _ -> false

let isDefinition = function
    | Definition _ -> true
    | _ -> false

let evalLine (line: string) =
    let trimmedLine = TrimmedString (line.Trim())

    match trimmedLine with
    | Comment comment -> comment
    | Whitespace _ -> ""
    | Definition quad -> quad.ToString()
    | _ -> sprintf "Unknown: %s" line

let run (myArgs: string array) =

    let expectedArgs = 2
    if myArgs.Length <> expectedArgs then
        let scriptName = fsi.CommandLineArgs.[0]
        eprintfn ""
        eprintfn "ERROR: Expected %d arguments" expectedArgs
        eprintfn "       Found %d: %A" myArgs.Length myArgs
        eprintfn "USAGE: fsi.exe %s -- <input-path> <output-path>" scriptName
        Environment.Exit(1)

    let inputPath = myArgs.[0]
    let outputPath = myArgs.[1]
    let useStdOut = outputPath = "-"

    let ns = "SoundMetrics.Aris.BeamWidths"
    let typeName = inputPath |> IO.Path.GetFileNameWithoutExtension

    let allLines = File.ReadLines(inputPath)
                    |> Seq.map TrimmedString
                    |> Seq.toArray
    let initialCommentLines = allLines |> Seq.takeWhile isComment
    let definitions = allLines |> Seq.filter isDefinition

    let writer =
        if useStdOut then
            Console.Out
        else
            new StreamWriter(outputPath) :> TextWriter

    try

        let processLines lines =
            for (TrimmedString line) in lines do
                fprintfn writer "%s" (evalLine line)

        processLines initialCommentLines
        fprintfn writer ""
        writeTypePrefix ns typeName writer
        processLines definitions
        writeTypeSuffix ns typeName writer
    finally
        if not useStdOut then
            writer.Dispose()

    0 // return an integer exit code


let remainingArgs =
    let a = fsi.CommandLineArgs |> Array.skipWhile (fun s -> s <> "--")
    if a.Length = 0 then
        a
    else
        a |> Array.skip 1

run remainingArgs
