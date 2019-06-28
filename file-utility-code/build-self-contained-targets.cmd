@ECHO OFF
REM ---------------------------------------------------------------------------
REM Builds the file utility nuget packages.
REM ---------------------------------------------------------------------------

SETLOCAL

SET BUILD_NUMBER=%1
IF "%BUILD_NUMBER%" == "" SET BUILD_NUMBER=5555
ECHO BUILD_NUMBER=%BUILD_NUMBER%

ECHO Building self-contained dotnet targets

SET PUBLISHCMD=dotnet publish -c Release
ECHO PUBLISHCMD=%PUBLISHCMD%

%PUBLISHCMD% .\CheckArisFile\ -o .\self-contained\CheckArisFile-win-x86\   --runtime win-x86
%PUBLISHCMD% .\CheckArisFile\ -o .\self-contained\CheckArisFile-linux-x64\ --runtime linux-x64

ENDLOCAL
