@ECHO OFF

REM TeamCity chokes on the COMPONENTSVERSION processing here, it thinks
REM COMPONENTSVERSION is required going into the build step when we would
REM define it in the build step.

SETLOCAL

REM ---------------------------------------------------------------------------
REM Check usage
REM ---------------------------------------------------------------------------

IF "%1" == "" GOTO Usage


SET BUILD_NUMBER=%1


REM ---------------------------------------------------------------------------
REM Build packages
REM ---------------------------------------------------------------------------

SET /P COMPONENTSVERSION=<ver.txt
ECHO COMPONENTSVERSION='%COMPONENTSVERSION%'

call build-packages.cmd %COMPONENTSVERSION%.%BUILD_NUMBER%

GOTO :EOF

:Usage
ECHO USAGE: build_packages_teamcity.cmd (build-number)

EXIT 1

GOTO :EOF

ENDLOCAL
