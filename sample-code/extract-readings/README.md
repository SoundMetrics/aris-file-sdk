## Extract C Program

This sample program extracts a few header fields from frames in a `.aris` file
(a '`.aris` file' being a recording of consecutive ARIS images). The values are extracted into a comma-delimited format (or CSV).

### Program Usage

    extract <input-path> <output-path>

### C Type Definitions

There are two header files, `FileHeader.h` and `FrameHeader.h`, which can be found in
https://github.com/SoundMetrics/aris-file-sdk/tree/master/type_definitions -- these files
are pre-generated for ease of use. These headers define structs for use in interpreting the data
in a `.aris` file.

### C Code

The code in this sample is fairly strict C code in order to keep things simple.
Modern C++ could have been used just as well.

### Other Languages

C# source files defining the `FileHeader` and `FrameHeader` are also generated at the time of this writing, for
use with any .NET or CLR programming language.

### ARIS Sample Recording
A sample recording is provided in https://github.com/SoundMetrics/aris-file-sdk/tree/master/sample-code .
