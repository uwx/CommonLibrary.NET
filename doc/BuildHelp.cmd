@echo off
IF EXIST "C:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" (goto 64bitSystem) else goto 32bitSystem
pause

:64bitSystem
echo Build in 64-bit system.
call "C:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" x86
if %ERRORLEVEL% NEQ 0 goto :ErrorVCVars
msbuild CommonLibrary.shfbproj
if %ERRORLEVEL% NEQ 0 goto :ErrorMSBuild
move /Y .\help\CommonLibrary.Net.chm CommonLibrary.Net.chm
if %ERRORLEVEL% NEQ 0 goto :ErrorMoveFile
goto ExitPoint

:32bitSystem
echo Build in 32-bit system.
call "C:\Program Files\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" x86
if %ERRORLEVEL% NEQ 0 goto :ErrorVCVars
msbuild CommonLibrary.shfbproj
if %ERRORLEVEL% NEQ 0 goto :ErrorMSBuild
move /Y .\help\CommonLibrary.Net.chm CommonLibrary.Net.chm
if %ERRORLEVEL% NEQ 0 goto :ErrorMoveFile
goto ExitPoint

:ErrorVCVars
echo ------------ E R R O R ------------
echo Error executing vcvarsall.bat
echo Cannot run MSBuild.
pause
goto ExitPoint

:ErrorMSBuild
echo ------------ E R R O R ------------
echo Error executing MSBuild with CommonLibrary.shfbproj.
echo Cannot create help file.
pause
goto ExitPoint

:ErrorMoveFile
echo ------------ E R R O R ------------
echo Error move CommonLibrary.Net.chm from Help dir to current dir.
echo A help file may not have been created.
pause
goto ExitPoint

:ExitPoint
exit