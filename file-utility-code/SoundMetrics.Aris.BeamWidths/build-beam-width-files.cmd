CALL %~dp0\..\..\init-dev-environment.cmd

SET PROJ_DIR=%~dp0
SET SOL_DIR=%PROJ_DIR%\..\

ECHO PROJ_DIR=[%PROJ_DIR%]
ECHO SOL_DIR=[%SOL_DIR%]

IF NOT EXIST "%PROJ_DIR%\generated" MKDIR "%PROJ_DIR%\generated"

for %%f in (BeamWidths_ARIS_Telephoto_48 BeamWidths_ARIS_Telephoto_96 BeamWidths_ARIS1800_96 BeamWidths_ARIS1800_1200_48 BeamWidths_ARIS3000_64 BeamWidths_ARIS3000_128) do ^
call %SOL_DIR%\run-fsi.cmd "%PROJ_DIR%ParseBeamWidthDefs.fsx" -- "%SOL_DIR%..\beam-width-metrics\%%f.h" "%PROJ_DIR%generated\%%f.cs"

dir "%PROJ_DIR%\generated\"
