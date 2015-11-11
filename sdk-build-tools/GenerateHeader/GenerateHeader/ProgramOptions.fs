module ProgramOptions

type Option = CommandLine.OptionAttribute

type Options() =

    [<Option('g', "language", Required = true)>]
    member val Language = "" with get, set

    [<Option('i', "inputfolder", Required = true, HelpText = "Input folder")>]
    member val InputFolder = "" with get, set

    [<Option('o', "output", Required = true, HelpText = "Output path")>]
    member val OutputPath = "" with get, set

    [<Option('m', "modifier")>]
    member val Modifier = "" with get, set
