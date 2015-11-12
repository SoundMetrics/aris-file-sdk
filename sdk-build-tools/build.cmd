REM ---------------------------------------------------------------------------
REM Builds pieces of the ARIS File SDK
REM ---------------------------------------------------------------------------

REM ---------------------------------------------------------------------------
REM Dependencies
REM
REM This script is dependent on:
REM     MSBuild
REM     F# compiler
REM     MSVC++ compiler
REM ---------------------------------------------------------------------------

SET GEN_HDR_SLN=.\GenerateHeader\GenerateHeader.sln
SET GEN_HDR_PATH=.\GenerateHeader\GenerateHeader\bin\Release\GenerateHeader.exe
SET TYPEDEFS_FOLDER=..\type_definitions

if NOT EXIST %TYPEDEFS_FOLDER% MKDIR %TYPEDEFS_FOLDER%
DEL /Y %TYPEDEFS_FOLDER%\*.*

REM ---------------------------------------------------------------------------
REM Build the tool that generates types for programming languages.
REM ---------------------------------------------------------------------------

msbuild %GEN_HDR_SLN% /m /t:Clean /p:Configuration="Release"
msbuild %GEN_HDR_SLN% /m /t:Build /p:Configuration="Release"

REM ---------------------------------------------------------------------------
REM Generate the types for programming languages.
REM ---------------------------------------------------------------------------

REM FileHeader

call %GEN_HDR_PATH% -g C  -i .\GenerateHeader\FileHeader.definition -o %TYPEDEFS_FOLDER%\FileHeader.h
call %GEN_HDR_PATH% -g C  -i .\GenerateHeader\FileHeader.definition -o %TYPEDEFS_FOLDER%\FileHeaderFieldsOnly.h -m fieldsonly
call %GEN_HDR_PATH% -g F# -i .\GenerateHeader\FileHeader.definition -o %TYPEDEFS_FOLDER%\FileHeader.fs

REM FrameHeader

call %GEN_HDR_PATH% -g C  -i .\GenerateHeader\FrameHeader.definition -o %TYPEDEFS_FOLDER%\FrameHeader.h
call %GEN_HDR_PATH% -g C  -i .\GenerateHeader\FrameHeader.definition -o %TYPEDEFS_FOLDER%\FrameHeaderFieldsOnly.h -m fieldsonly
call %GEN_HDR_PATH% -g F# -i .\GenerateHeader\FrameHeader.definition -o %TYPEDEFS_FOLDER%\FrameHeader.fs

REM ---------------------------------------------------------------------------
REM Build code to verify correctness of generated types.
REM ---------------------------------------------------------------------------

msbuild %GEN_HDR_SLN% /m /t:TestCType:Clean /p:Configuration="Release" /p:Platform="x86"
msbuild %GEN_HDR_SLN% /m /t:TestCType /p:Configuration="Release" /p:Platform="x86"

msbuild %GEN_HDR_SLN% /m /t:TestFSharpType:Clean /p:Configuration="Release"
msbuild %GEN_HDR_SLN% /m /t:TestFSharpType /p:Configuration="Release"

REM ---------------------------------------------------------------------------
REM Verify correctness of generated types.
REM ---------------------------------------------------------------------------

call GenerateHeader\Release\TestCType.exe ArisFileHeader
call GenerateHeader\Release\TestCType.exe ArisFileHeaderFieldsOnly
call GenerateHeader\TestFSharpType\bin\Release\TestFSharpType.exe ArisFile+ArisFileHeader

call GenerateHeader\Release\TestCType.exe ArisFrameHeader
call GenerateHeader\Release\TestCType.exe ArisFrameHeaderFieldsOnly
call GenerateHeader\TestFSharpType\bin\Release\TestFSharpType.exe ArisFrame+ArisFrameHeader
