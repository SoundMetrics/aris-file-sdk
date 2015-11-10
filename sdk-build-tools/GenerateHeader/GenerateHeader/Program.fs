// Parse a type definition and generate source code.

open FieldParser
open System
open System.IO
open System.Text

let dummyFieldInput = @"Name,Type,Array,Description,Note,Obsolete
F1,int,,Description of this field. 12345678901234567890,,
Field2,string,32,My F2, F2's notes and ramblings go here!!!1!,As of 1/1/2016 use F3 instead.
F3,string,32,F3!,,
"

let dummyTypeInput = @"ModuleName,TypeName,Description,Notes,FileComment
File,FileHeader,Defines the metadata at the start of an ARIS recording,,OutputFile.fs"

let dummyProducer = FSharpProducer.produce

[<EntryPoint>]
let main _argv =

    let producer = dummyProducer
    use fieldInput = new MemoryStream(Encoding.UTF8.GetBytes(dummyFieldInput))
    use typeInput = new MemoryStream(Encoding.UTF8.GetBytes(dummyTypeInput))
    processStream producer typeInput fieldInput Console.Out

    0 // return an integer exit code
