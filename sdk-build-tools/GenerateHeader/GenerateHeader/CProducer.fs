module CProducer

open CodeProducer
open FormattingHelp
open System.IO

[<Literal>]
let CommentStart = "//"

let typeMap =
    [
        FieldType.Uint16,   "uint16_t"
        FieldType.Uint32,   "uint32_t"
        FieldType.UInt64,   "uint64_t"
        FieldType.Int32,    "int32_t"
        FieldType.Float32,  "float"
        FieldType.Float64,  "double"
        FieldType.String,   "char"
    ]
    |> Map.ofList


let produce filename (output : TextWriter) (modifier : string) (indent : Indent) (part : FilePart) : Indent =

    let noop = indent // Return unmodified indent for no-ops.

    let includeAll = modifier.ToUpper() <> "FIELDSONLY"
    let getSymbol (typeName : string) = sprintf "ARIS_%s_H" (typeName.ToUpper())

    match part with
    | FileBegin _ ->
        if includeAll then
            writePrefixedWrappedLines output indent CommentStart filename
            output.WriteLine()

        indent

    | FileEnd _ -> noop

    | NamespaceBegin typeInfo ->
        if includeAll then
            let symbol = getSymbol typeInfo.typeName
            writeUnbrokenLine output indent (sprintf "#ifndef %s" symbol)
            writeUnbrokenLine output indent (sprintf "#define %s" symbol)
            output.WriteLine()

            writeUnbrokenLine output indent "#include <stdint.h>"
            output.WriteLine()

        indent

    | NamespaceEnd typeInfo ->
        if includeAll then
            output.WriteLine()

            let symbol = getSymbol typeInfo.typeName
            writeUnbrokenLine output indent (sprintf "#endif // !%s" symbol)

        indent

    | ModuleBegin _ -> noop
    | ModuleEnd _ -> noop

    | TypeBegin typeInfo ->
        if includeAll then
            writeUnbrokenLine output indent "#pragma pack(push, 1)"
            output.WriteLine()

            writeUnbrokenLine output indent (sprintf "struct %s {" typeInfo.typeName)
            output.WriteLine()

        indent

    | TypeEnd _ ->
        if includeAll then
            writeUnbrokenLine output indent "};"
            output.WriteLine()

            writeUnbrokenLine output indent "#pragma pack(pop)"

        indent

    | Fields fieldInfos ->
        let originalIndent = indent
        let indent = indent.Indent()

        for field in fieldInfos do
            if field.description.Length > 0 then
                writePrefixedWrappedLines output indent CommentStart field.description

            if field.note.Length > 0 then
                writePrefixedWrappedLines output indent CommentStart ("Note: " + field.note)

            if field.obsoleteNote.Length > 0 then
                writeUnbrokenLine output indent (sprintf "// OBSOLETE: %s" field.obsoleteNote)

            let vector =
                match field.fieldCat with
                | Scalar -> ""
                | Vector size -> sprintf "[%d]" size

            writeUnbrokenLine output indent
                (sprintf "%s %s%s;" (typeMap.[field.typ]) field.name vector)

            output.WriteLine()

        originalIndent
