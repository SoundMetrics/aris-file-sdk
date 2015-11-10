module FormattingHelp

open CodeProducer
open System
open System.IO

[<AutoOpen>]
module internal FormattingHelpImpl =

    [<Literal>]
    let TargetColumnWidth = 130

    let ws = [| ' '; '\t' |]

    let lengthUntilWhitespace (text : string) startIdx targetWidth =

        if text.Length - startIdx < targetWidth then
            text.Length - startIdx
        else
            let maxWidth = min targetWidth (text.Length - startIdx)
            let rbegin = startIdx + maxWidth - 1
            let idx = text.LastIndexOfAny(ws, rbegin, rbegin - startIdx + 1)
            let len =
                if idx < 0 then
                    let idx' = text.IndexOfAny(ws, startIdx)
                    if idx' < 0 then
                        text.Length - startIdx
                    else
                        idx' - startIdx
                else
                    idx - startIdx
            len

/// Useful for writing wrapped comments with a prefix.
let writePrefixedWrappedLines (output : TextWriter) (indent : Indent) (prefix : string) (text : string) =

    let preferedMinTargetWidth = 20
    assert (preferedMinTargetWidth <= TargetColumnWidth)

    let targetWidth = max preferedMinTargetWidth (TargetColumnWidth - indent.Width)
    let prefix' =
        if prefix.Length > 0 then
            prefix + " "
        else
            prefix

    let rec writePartial startIdx =

        if startIdx >= text.Length then
            ()
        elif Char.IsWhiteSpace(text.[startIdx]) then
            writePartial (startIdx + 1)
        else
            let wsLen = lengthUntilWhitespace text startIdx targetWidth
            let partial = text.Substring(startIdx, wsLen)
            indent.WriteIndent(output)
            output.WriteLine(sprintf "%s%s" prefix' partial)

            writePartial (startIdx + partial.Length)

    writePartial 0

let writeUnbrokenLine (output : TextWriter) (indent : Indent) (text : string) =

    indent.WriteIndent(output)
    output.WriteLine(text)
