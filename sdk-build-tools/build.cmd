REM ---------------------------------------------------------------------------
REM Builds pieces of the ARIS File SDK
REM ---------------------------------------------------------------------------

REM ---------------------------------------------------------------------------
REM Dependencies
REM
REM This script is dependent on:
REM     MSBuild
REM     F# compiler
REM     C# compiler
REM     MSVC++ compiler
REM ---------------------------------------------------------------------------

SET GEN_HDR_SLN=.\GenerateHeader\GenerateHeader.sln
SET GEN_HDR_PATH=.\GenerateHeader\GenerateHeader\bin\Release\GenerateHeader.exe

SET COMMON_CODE_SLN=..\common-code\VisualStudio\CommonCode.sln

SET SAMPLE_CODE_SLN=..\sample-code\extract-readings\VisualStudio\VisualStudio.sln

SET TYPEDEFS_FOLDER=..\type-definitions
SET C_TYPES_FOLDER=%TYPEDEFS_FOLDER%\C
SET CSharp_TYPES_FOLDER=%TYPEDEFS_FOLDER%\CSharp

if NOT EXIST %TYPEDEFS_FOLDER% MKDIR %TYPEDEFS_FOLDER%
DEL /Y %TYPEDEFS_FOLDER%\*.*

REM ---------------------------------------------------------------------------
REM Build the tool that generates types for programming languages.
REM ---------------------------------------------------------------------------

NugetPkg\nuget.exe restore GenerateHeader\GenerateHeader.sln

msbuild %GEN_HDR_SLN% /m /t:Clean /p:Configuration="Release"
msbuild %GEN_HDR_SLN% /m /t:GenerateHeader /p:Configuration="Release"

REM ---------------------------------------------------------------------------
REM Generate the types for programming languages.
REM ---------------------------------------------------------------------------

REM FileHeader

call %GEN_HDR_PATH% -g C  -i .\GenerateHeader\FileHeader.definition -o %C_TYPES_FOLDER%\FileHeader.h
call %GEN_HDR_PATH% -g C  -i .\GenerateHeader\FileHeader.definition -o %C_TYPES_FOLDER%\FileHeaderFieldsOnly.h -m fieldsonly
call %GEN_HDR_PATH% -g C# -i .\GenerateHeader\FileHeader.definition -o %CSharp_TYPES_FOLDER%\FileHeader.cs

REM FrameHeader

call %GEN_HDR_PATH% -g C  -i .\GenerateHeader\FrameHeader.definition -o %C_TYPES_FOLDER%\FrameHeader.h
call %GEN_HDR_PATH% -g C  -i .\GenerateHeader\FrameHeader.definition -o %C_TYPES_FOLDER%\FrameHeaderFieldsOnly.h -m fieldsonly
call %GEN_HDR_PATH% -g C# -i .\GenerateHeader\FrameHeader.definition -o %CSharp_TYPES_FOLDER%\FrameHeader.cs

REM ---------------------------------------------------------------------------
REM Build code to verify correctness of generated types.
REM ---------------------------------------------------------------------------

msbuild %GEN_HDR_SLN% /m /t:TestCType:Clean /p:Configuration="Release" /p:Platform="x86"
msbuild %GEN_HDR_SLN% /m /t:TestCType /p:Configuration="Release" /p:Platform="x86"

msbuild %GEN_HDR_SLN% /m /t:TestCSharpType:Clean /p:Configuration="Release"
msbuild %GEN_HDR_SLN% /m /t:TestCSharpType /p:Configuration="Release"

REM ---------------------------------------------------------------------------
REM Verify correctness of generated types.
REM ---------------------------------------------------------------------------

call GenerateHeader\Release\TestCType.exe ArisFileHeader
call GenerateHeader\Release\TestCType.exe ArisFileHeaderFieldsOnly
call GenerateHeader\TestCSharpType\bin\Release\TestCSharpType.exe Aris.FileTypes.ArisFileHeader

call GenerateHeader\Release\TestCType.exe ArisFrameHeader
call GenerateHeader\Release\TestCType.exe ArisFrameHeaderFieldsOnly
call GenerateHeader\TestCSharpType\bin\Release\TestCSharpType.exe Aris.FileTypes.ArisFrameHeader

REM ---------------------------------------------------------------------------
REM Build common native code.
REM ---------------------------------------------------------------------------

msbuild %COMMON_CODE_SLN% /m /t:Clean /p:Configuration="Debug"   /p:Platform="Win32"
msbuild %COMMON_CODE_SLN% /m /t:Clean /p:Configuration="Release" /p:Platform="Win32"
msbuild %COMMON_CODE_SLN% /m /t:Clean /p:Configuration="Debug"   /p:Platform="x64"
msbuild %COMMON_CODE_SLN% /m /t:Clean /p:Configuration="Release" /p:Platform="x64"

msbuild %COMMON_CODE_SLN% /m /t:Build /p:Configuration="Debug"   /p:Platform="Win32"
msbuild %COMMON_CODE_SLN% /m /t:Build /p:Configuration="Release" /p:Platform="Win32"
msbuild %COMMON_CODE_SLN% /m /t:Build /p:Configuration="Debug"   /p:Platform="x64"
msbuild %COMMON_CODE_SLN% /m /t:Build /p:Configuration="Release" /p:Platform="x64"

REM ---------------------------------------------------------------------------
REM Aris.FileTypes is built as part of GenerateHeader.sln.
REM ---------------------------------------------------------------------------

REM ---------------------------------------------------------------------------
REM Build sample code.
REM ---------------------------------------------------------------------------

msbuild %SAMPLE_CODE_SLN% /m /t:Clean /p:Configuration="Release" /p:Platform="x86"
msbuild %SAMPLE_CODE_SLN% /m /t:Build /p:Configuration="Release" /p:Platform="x86"
