To launch scripts:

INSTALLATION:

Required Components:

1. NAnt   - Copy from Network location to C:\Dev\Tools\Nant\0.85
2. NUnit  - Copy from Network location to C:\Dev\Tools\NUnit\2.4
3. NCover - Copy from Network location to C:\Dev\Tools\NCover\1.5.8 

Setting up system path:

1. Add location of NAnt.exe to your system path.
2. Add MSBuild ( c:\winnt\microsoft.net\3.5.xxx\ to your system path.



CONFIGURATION AND SETTINGS:

1. Open command window and go to build directory c:\dev\commonlibrary.
   c:\dev\commonlibrary\build

2. The _commonlibrary.settings.xml file is used to store settings
   for building/versioning/deploying/etc your project.
   If you have the same settings ( location of source code for example )
   then you do not need to change this file.

   To change the file:
   Change the _commonlibrary.settings.xml file
   - If you have commonlibrary installed to a different location,
     you can make a copy of this _commonlibrary.settings.file,
     such as _commonlibrary.settings.local.xml
   - Change the locations of various paths in the settings file
     to coincide with where you stored your project
   

RUNNING THE SCRIPTS:

All the currently supported tasks are as follows:

Action 			NAnt Script 	Batch file command
Clean 			clean.xml 		bq clean
Get Sources 	getSources.xml 	bq sources
Versioning 		version.xml 	bq Version
Compile 		compile.xml 	bq compile
Unit Test 		unittest.xml 	bq unittest
Fx Cop 			fxcop.xml 		bq fxcop
CodeCoverage 	coverage.xml 	bq coverage
Tag 			tag.xml 		bq label
Configure 		env.xml 		bq env
Deploy 			deploy.xml 		bq deploy
Backup 			backup.xml 		bq backup


To run a script.

1. Open command window.
2. Go to build directory ( c:\dev\commonlibrary\build )
3. Type bq <command>
   e.g. bq unittest


