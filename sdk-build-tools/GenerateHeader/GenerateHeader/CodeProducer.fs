module CodeProducer

open FSharp.Data
open System.IO

/// Tracks indent level and writes an indent to a StreamWriter.
type Indent = {
    level : int
}
with
    member i.Indent() = { level = i.level + 1 }
    member i.Unindent() =
        let newLevel = i.level - 1
        if newLevel < 0 then
            failwith "Unindent caused a negative indent"
        { level = newLevel }

    member i.WriteIndent(stream : StreamWriter) =

        for i = 0 to i.level - 1 do
            stream.Write("    ")

/// Common arguments for code producer calls.
type ProducerBasics = {
    indent : Indent
    stream : StreamWriter
}

/// Information about the type being generated.
type TypeInfo = {
    name : string
    description : string
    notes : string
}

/// Indicates whether the field is a scalar or vector;
/// vectors only have one dimension.
type FieldCategory =
| Scalar
| Vector of size : int

/// Types recognized by the code generator.
type FieldType =
| Uint16
| Uint32
| UInt64
| Int32
| Float32
| Float64
| Char8

/// Describes a field.
type FieldInfo = {
    name : string
    typ : FieldType
    fieldCat : FieldCategory
    description : string
    note : string
}

/// Defines functions that are called on code producers to output the
/// appropriate code.
type ICodeProducer =

    abstract member WriteTypeComments   : ProducerBasics -> TypeInfo -> unit
    abstract member WriteTypePreface    : ProducerBasics -> TypeInfo -> unit
    abstract member WriteTypeBegin      : ProducerBasics -> TypeInfo -> unit
    abstract member WriteTypeEnd        : ProducerBasics -> TypeInfo -> unit

    abstract member WritFieldComments   : ProducerBasics -> FieldInfo -> unit
    abstract member WritFieldPreface    : ProducerBasics -> FieldInfo -> unit
    abstract member WritField           : ProducerBasics -> FieldInfo -> unit
