@ECHO OFF
REM ---------------------------------------------------------------------------
REM Builds packages for the ARIS File SDK
REM ---------------------------------------------------------------------------

SETLOCAL

REM ---------------------------------------------------------------------------
REM Check usage
REM ---------------------------------------------------------------------------

IF "%1" == "" GOTO Usage


SET VERSION_STRING=%1


REM ---------------------------------------------------------------------------
REM Do the work
REM ---------------------------------------------------------------------------


SET NUGETPKGDIR=.\NugetPkg
SET NUGET=%NUGETPKGDIR%\nuget.exe

%NUGET% pack -Version "%VERSION_STRING%" %NUGETPKGDIR%\Aris.FileSDK.Managed.nuspec -OutputDirectory %NUGETPKGDIR%
%NUGET% pack -Version "%VERSION_STRING%" %NUGETPKGDIR%\Aris.FileSDK.Native.nuspec  -OutputDirectory %NUGETPKGDIR%

ENDLOCAL

GOTO :EOF

REM ---------------------------------------------------------------------------
REM Show usage
REM ---------------------------------------------------------------------------

:Usage
ECHO USAGE: build_packages.cmd (version)
ECHO          e.g., build_packages 1.2.1.8127

GOTO :EOF
