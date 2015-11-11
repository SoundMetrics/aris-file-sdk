module FSharpProducer

open CodeProducer
open FormattingHelp
open System.IO

[<Literal>]
let CommentStart = "//"

let typeMap =
    [
        FieldType.Uint16,   "UInt16"
        FieldType.Uint32,   "uint"
        FieldType.UInt64,   "UInt64"
        FieldType.Int32,    "int"
        FieldType.Float32,  "float32"
        FieldType.Float64,  "float"
        FieldType.String,   "string"
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

let produce filename (output : TextWriter) (indent : Indent) (part : FilePart) : Indent =

    let noop = indent // Return unmodified indent for no-ops.

    match part with
    | FileBegin _ ->
        writePrefixedWrappedLines output indent CommentStart filename
        output.WriteLine()

        indent

    | FileEnd _ -> noop

    | NamespaceBegin _ -> noop

    | NamespaceEnd _ -> noop

    | ModuleBegin typeInfo ->
        indent.WriteIndent(output)
        writeUnbrokenLine output indent (sprintf "module %s" typeInfo.moduleName)
        output.WriteLine()

        writeUnbrokenLine output indent
            "#nowarn \"9\" // Uses of this construct may result in the generation of unverifiable .NET IL code."
        output.WriteLine()

        writeUnbrokenLine output indent "open System"
        writeUnbrokenLine output indent "open System.Runtime.InteropServices"
        output.WriteLine()

        indent

    | ModuleEnd _ -> noop

    | TypeBegin typeInfo ->
        writeUnbrokenLine output indent "[<Struct>]"
        writeUnbrokenLine output indent
            "[<StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)>]"

        writeUnbrokenLine output indent (sprintf "type %s =" typeInfo.typeName)
        output.WriteLine()
        indent

    | TypeEnd _ -> noop

    | Fields fieldInfos ->
        let originalIndent = indent
        let indent = indent.Indent()

        for field in fieldInfos do
            if field.description.Length > 0 then
                writePrefixedWrappedLines output indent CommentStart field.description

            if field.note.Length > 0 then
                writePrefixedWrappedLines output indent CommentStart ("Note: " + field.note)

            if field.obsoleteNote.Length > 0 then
                writeUnbrokenLine output indent (sprintf "[<Obsolete(\"%s\")>]" field.obsoleteNote)

            match field.fieldCat with
            | Scalar -> ()
            | Vector size ->
                writeUnbrokenLine output indent
                    (sprintf "[<MarshalAs(%s, SizeConst = %d)>]" marshalMap.[field.typ] size)

            writeUnbrokenLine output indent
                (sprintf "val mutable %s : %s" field.name (typeMap.[field.typ]))

            output.WriteLine()

        originalIndent
