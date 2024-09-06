REM Initializes the environment to build projects/solutions with Visual Studio tools.
REM USAGE:
REM   call init-dev-environment.cmd

REM The BuildTools location is the latest official localtion. The new 'winget configure'
REM tooling should enable this.

IF EXIST "C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\Common7\Tools\VsDevCmd.bat" (
	CALL "C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\Common7\Tools\VsDevCmd.bat"
) ELSE (
	IF EXIST "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\Tools\VsDevCmd.bat" (
		CALL "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\Tools\VsDevCmd.bat"
	) ELSE (
		IF EXIST "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\Tools\VsDevCmd.bat" (
			CALL "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\Tools\VsDevCmd.bat"
		) ELSE (
			IF EXIST "C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\Tools\VsDevCmd.bat" (
				CALL "C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\Tools\VsDevCmd.bat"
			)
		)
	)
)
