
@echo off 

REM ********************************************
REM Setup variables for various paths
REM ********************************************
set FSHOME=C:\Dev\Kishore\Apps\ComLib\ComLib_Latest\src\Apps\ComLib.Apps.FluentScript\
set FSBIN=%FSHOME%\scripts


REM ********************************************
REM Move to bin directory
REM ********************************************
c:
cd %FSBIN%


REM ********************************************
REM Execute
REM ********************************************
fs.exe file:example_2_datatypes.js


REM ********************************************
REM Go back to scripts folder
REM ********************************************
cd %FSHOME%\scripts