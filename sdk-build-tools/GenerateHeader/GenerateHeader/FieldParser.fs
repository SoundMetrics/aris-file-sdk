module FieldParser

open CodeProducer
open FSharp.Data
open System

let typeMap =
    [ 
        "uint16",   FieldType.Uint16
        "uint",     FieldType.Uint32
        "uint64",   FieldType.UInt64
        "int",      FieldType.Int32
        "float",    FieldType.Float32
        "double",   FieldType.Float64
        "byte",     FieldType.Char8
    ]
    |> Map.ofList


let private getType (row : CsvRow) =

    let typ = typeMap.[row.["Type"]]
    let arr = row.["Array"]
    let cat =   if String.IsNullOrWhiteSpace(arr) then
                    Scalar
                else
                    Vector (Int32.Parse(arr))
    typ, cat

/// Parses field information as read from the CSV file.
let parseField (row : CsvRow) =

    let typ, cat = getType row

    {   name =      row.["Name"]
        typ =       typ
        fieldCat =  cat
        description = row.["Description"]
        note =      row.["Note"]
    }
