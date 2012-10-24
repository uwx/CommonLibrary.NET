
@echo off 

set FSHOME=C:\Dev\Kishore\Apps\ComLib\ComLib_Latest\src\Apps\ComLib.Apps.FluentScript\bin\Debug
set FSBIN=%FSHOME%\bin\Debug
set FSSCRIPTS=..\..\scripts

@echo on
%FSBIN%\fs.exe file:%FSSCRIPTS%\scripts\example_1_helloworld.js