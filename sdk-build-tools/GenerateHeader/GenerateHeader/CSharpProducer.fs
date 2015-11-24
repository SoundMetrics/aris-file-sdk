module CSharpProducer

open CodeProducer
open FormattingHelp
open System.IO

[<Literal>]
let CommentStart = "//"

let typeMap =
    [
        FieldType.Uint16,   "UInt16"
        FieldType.Uint32,   "UInt32"
        FieldType.UInt64,   "UInt64"
        FieldType.Int32,    "int"
        FieldType.Float32,  "float"
        FieldType.Float64,  "double"
        FieldType.String,   "string" // CharSet.Ansi
    ]
    |> Map.ofList

let marshalMap =
    [
        FieldType.Uint16,   "UnmanagedType.ByValArray"
        FieldType.Uint32,   "UnmanagedType.ByValArray"
        FieldType.UInt64,   "UnmanagedType.ByValArray"
        FieldType.Int32,    "UnmanagedType.ByValArray"
        FieldType.Float32,  "UnmanagedType.ByValArray"
        FieldType.Float64,  "UnmanagedType.ByValArray"
        FieldType.String,   "UnmanagedType.ByValTStr"
    ]
    |> Map.ofList

let arraySuffixMap =
    [
        FieldType.Uint16,   "[]"
        FieldType.Uint32,   "[]"
        FieldType.UInt64,   "[]"
        FieldType.Int32,    "[]"
        FieldType.Float32,  "[]"
        FieldType.Float64,  "[]"
        FieldType.String,   ""
    ]
    |> Map.ofList

let produce filename (output : TextWriter) (_modifier : string) (indent : Indent) (part : FilePart) : Indent =

    let noop = indent // Return unmodified indent for no-ops.

    match part with
    | FileBegin _ ->
        writePrefixedWrappedLines output indent CommentStart filename
        output.WriteLine()

        indent

    | FileEnd _ -> noop

    | NamespaceBegin _ ->
        writeUnbrokenLine output indent "namespace Aris.FileTypes"
        writeUnbrokenLine output indent "{"
        indent.Indent()

    | NamespaceEnd _ ->
        let newIndent = indent.Unindent()
        writeUnbrokenLine output newIndent "}"
        newIndent

    | ModuleBegin _ ->
        indent.WriteIndent(output)
        output.WriteLine()

        writeUnbrokenLine output indent "using System;"
        writeUnbrokenLine output indent "using System.Runtime.InteropServices;"
        output.WriteLine()

        indent

    | ModuleEnd _ -> noop

    | TypeBegin typeInfo ->
        writeUnbrokenLine output indent
            "[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]"

        writeUnbrokenLine output indent (sprintf "public struct %s" typeInfo.typeName)
        writeUnbrokenLine output indent "{"

        writeUnbrokenLine output (indent.Indent()) "public static int ArisFileSignature =  0x05464444;"
        writeUnbrokenLine output (indent.Indent()) "public static int ArisFrameSignature = 0x05464444;"
        output.WriteLine()

        indent

    | TypeEnd _ ->
        writeUnbrokenLine output indent "}"
        indent

    | Fields fieldInfos ->
        let originalIndent = indent
        let indent = indent.Indent()
        let mutable storageSize = 0

        let generateField field =
            if field.description.Length > 0 then
                writePrefixedWrappedLines output indent CommentStart field.description

            if field.note.Length > 0 then
                writePrefixedWrappedLines output indent CommentStart ("Note: " + field.note)

            if field.obsoleteNote.Length > 0 then
                writeUnbrokenLine output indent (sprintf "[Obsolete(\"%s\")]" field.obsoleteNote)

            match field.fieldCat with
            | Scalar -> ()
            | Vector size ->
                writeUnbrokenLine output indent
                    (sprintf "[MarshalAs(%s, SizeConst = %d)]" marshalMap.[field.typ] size)

            let arraySuffix =
                match field.fieldCat with
                | Scalar -> ""
                | Vector _ -> arraySuffixMap.[field.typ]

            writeUnbrokenLine output indent
                (sprintf "public %s%s %s;" (typeMap.[field.typ]) arraySuffix field.name)

            output.WriteLine()

        for field in fieldInfos do
            storageSize <- storageSize + field.StorageSize
            generateField field

        let paddingSize = 1024 - storageSize
        generateField { name = "padding"
                        typ = String
                        fieldCat = Vector paddingSize
                        description = "Padding to fill out to 1024 bytes"
                        note = ""
                        obsoleteNote = "" }

        originalIndent
