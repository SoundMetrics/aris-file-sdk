module FieldParser

open CodeProducer
open FSharp.Data
open System
open System.IO

let typeMap =
    [ 
        "uint16",   FieldType.Uint16
        "uint",     FieldType.Uint32
        "uint64",   FieldType.UInt64
        "int",      FieldType.Int32
        "float",    FieldType.Float32
        "double",   FieldType.Float64
        "string",   FieldType.String
    ]
    |> Map.ofList


let private getType (row : CsvRow) =

    let typ = typeMap.[row.["Type"].Trim()]
    let arr = row.["Array"].Trim()
    let cat =   if String.IsNullOrWhiteSpace(arr) then
                    Scalar
                else
                    Vector (Int32.Parse(arr))
    typ, cat

/// Parses field information as read from the CSV file.
let parseField (row : CsvRow) =

    let typ, cat = getType row

    {   name =      row.["Name"].Trim()
        typ =       typ
        fieldCat =  cat
        description = row.["Description"].Trim()
        note =      row.["Note"].Trim()
        obsoleteNote = row.["Obsolete"].Trim() }

let parseType (row : CsvRow) =

    {   moduleName = row.["ModuleName"].Trim()
        typeName = row.["TypeName"].Trim()
        description = row.["Description"].Trim()
        notes = row.["Notes"].Trim()
        fileComment = row.["FileComment"].Trim() }

let processStream (produce : CodeProducer) (typeInput : Stream)
                  (fieldInput : Stream) (output : TextWriter) =

    let typeInfo = parseType (CsvFile.Load(typeInput).Rows |> Seq.head)
    let fields = CsvFile.Load(fieldInput).Rows |> Seq.map parseField |> Seq.toArray

    // The code producer function returns a new indent level.
    // Using 'fold' here carries forward changes to the indent level.
    [
        FileBegin typeInfo
        NamespaceBegin typeInfo
        ModuleBegin typeInfo
        TypeBegin typeInfo
        Fields fields
        TypeEnd typeInfo
        ModuleEnd typeInfo
        NamespaceEnd typeInfo
        FileEnd typeInfo
    ]
    |> List.fold (produce output) { level = 0 }
    |> ignore

let processFile (producer : CodeProducer) typeInputPath fieldInputPath (output : TextWriter) =

    use typeInput = File.Open(typeInputPath, FileMode.Open, FileAccess.Read, FileShare.Read)
    use fieldInput = File.Open(fieldInputPath, FileMode.Open, FileAccess.Read, FileShare.Read)
    processStream producer typeInput fieldInput output
