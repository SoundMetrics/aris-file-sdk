// FrameHeader.fs

module ArisFrame

#nowarn "9" // Uses of this construct may result in the generation of unverifiable .NET IL code.

open System
open System.Runtime.InteropServices

[<Struct>]
[<StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)>]
type ArisFrameHeader =

    // Description of this field. 12345678901234567890
    val mutable F1 : int

    // My F2
    // Note: F2's notes and ramblings go here!!!1!
    [<Obsolete("As of 1/1/2016 use F3 instead.")>]
    [<MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)>]
    val mutable Field2 : string

    // F3!
    [<MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)>]
    val mutable F3 : string

