SET SOL_DIR=%1
SET ALT_SOL_DIR=..\
IF "%SOL_DIR%" == "" SET SOL_DIR=%ALT_SOL_DIR%
IF "%SOL_DIR%" == "*Undefined*" SET SOL_DIR=%ALT_SOL_DIR%
echo SOL_DIR is %SOL_DIR%

SET PROJ_DIR=%2
SET ALT_PROJ_DIR=%SOL_DIR%\SoundMetrics.Aris.BeamWidths\
IF "%PROJ_DIR%" == "" SET PROJ_DIR=%ALT_PROJ_DIR%
IF "%PROJ_DIR%" == "*Undefined*" SET PROJ_DIR=%ALT_PROJ_DIR%
echo PROJ_DIR is %PROJ_DIR%

IF NOT EXIST "%PROJ_DIR%\generated" MKDIR "%PROJ_DIR%\generated"

for %%f in (BeamWidths_ARIS_Telephoto_48 BeamWidths_ARIS_Telephoto_96 BeamWidths_ARIS1800_96 BeamWidths_ARIS1800_1200_48 BeamWidths_ARIS3000_64 BeamWidths_ARIS3000_128) do ^
call %SOL_DIR%\run-fsi.cmd "%PROJ_DIR%ParseBeamWidthDefs.fsx" -- "%SOL_DIR%..\beam-width-metrics\%%f.h" "%PROJ_DIR%generated\%%f.cs"

dir "%PROJ_DIR%\generated\"
