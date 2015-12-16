SETLOCAL

SET SOLUTION_DIR=%1
SET PROJECT_DIR=%2

ECHO SOLUTION_DIR=[%SOLUTION_DIR%]
ECHO PROJECT_DIR= [%PROJECT_DIR%]

REM Update Paket
"%SOLUTION_DIR%\.paket\paket.bootstrapper.exe"

REM Install dependencies
"%SOLUTION_DIR%\.paket\paket.exe" install

SET BUILDINFO="%SOLUTION_DIR%\paket-files\soundmetrics\build-tools\binaries\BuildInfo.exe"

%BUILDINFO% --namespace Aris.FileTypes --output-type CSharp --output-path "%PROJECT_DIR%\Properties\AssemblyInfo2.cs" --version-file "%SOLUTION_DIR%\ver.txt" --build-number "%BUILD_NUMBER%"

ENDLOCAL
