
# *************************************************************************************
# @summary: FluentScript based build script for commonlibrary.net
# @desc:	Automates the commonlibrary.net build by cleaning out the directories,
#			updating version numbers, compiling and then packaging the binaries,
#			source code, documentation, and examples
# @author:  kishore reddy
# @date:    Sep 2nd, 2012
# @version: 1.0
# @fs-version: 0.9.8.8
# 
# args: name  |	type	|  example,							  | desc
# [ 
# 		task,	string,   [ Clean, Secure, Execute 			],	'Which task to run'
# 		log,	bool,	  [ yes, no, true, false 			],	'Whether or not to create a build log'
# 		file,	string,   [ 'env.home\log.txt', 'c:\log.txt'],	'The path to the build log file'
# ]
#
# @remarks: 
# [	
#	This has been converted to fluentscript from NAnt.
# 	This is also a live test of the fluentscript language.
# ]
# *************************************************************************************

# 1. Constants ( version, base dir etc )
version   = 0.9.8.8
base_dir  = C:\DEV\business\CommonLibrary.NET\CommonLibraryNet_LATEST
build_ver = version.text


# 2. Application specific settigns
app = 	{
			name:    'CommonLibraryNET'
			basedir: base_dir
			srcdir : @base_dir\src
			pubdir : @base_dir\dist
			testdir: 'src\tests'
		}

# 3. Build dir		
build.dir.public = "" 
dist.dir.root = "dist" 
dist.dir = "${dist.dir.root}\${app.name}_${build.version}"


# 4. Register words  e.g. bin = 'bin'
@words ( bin, obj )


# 5. Functions

	# @summary: Removes temp files and directories created as part of the last build.
	# @args: none
	# @example: [ Clean, Clean() ]
	def Clean
	{
		delete file @{dist.dir.root}\@{app.name}_@{build.version}.zip 
		delete dir  @distdir
		
		# dirs.deleteByName
		delete dirs by name @{app.base.dir}\src\Apps, 	names: [ bin, obj ], recurse
		delete dirs by name @{app.base.dir}\src\Lib, 	names: [ bin, obj ], recurse
		delete dirs by name @{app.base.dir}\src\Tests, 	names: [ bin, obj ], recurse
	}


	
	# @summary: Secures any files.
	# @args: none
	# @example: [ Clean, Clean() ]
	def Secure
	{
	}


	
	# @summary: This writes out the version information into all the files named "AssemblyVersion.cs" in the directory.
	# @args: none
	# @example: [ Clean, Clean() ]
	def Version
	{
		files = find files 'AssemblyVersion.cs' in: @{app.base.dir}\src		
		for file in files
			write file file.fullname, content: "${build.verion}", overwrite: yes
	}

	

	# @summary	Compile the project.
	# @args: none
	# @example: [ Clean, Clean() ]
	def Compile
		print Compiling : ${compile.compiler} ${compile.solutionOrProject} /t:${compile.target} /p:Configuration=${compile.configuration} 		
		
		exec program: "${compile.compiler}",
			 workingdir: "${compile.workingDir}",
			 failonerror: false, 
			 args: 
			 [
				"${compile.solutionOrProject}", 
				"/t:${compile.target}", 
				"/p:Configuration=${compile.configuration}" 
			 ]
	}	


	# Compile the project.
	# @args: none
	# @example: [ Clean, Clean() ]
	def PackageExamples
		print "${app.source.dir}" 
		make dir @distdir\src\Examples                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
		make dir @distdir\src\Examples\ComLib.Apps.SampleApp
		make dir @distdir\src\Examples\ComLib.Apps.StockMarketApp		
		
		copy dir @distdir\src\Lib\CommonLibrary.NET\_Samples   to: @distdir\src\examples
		copy dir @distdir\src\Apps\ComLib.Apps.SampleApp       to: @distdir\src\examples\ComLib.Apps.SampleApp
		copy dir @distdir\src\Apps\ComLib.Apps.StockMarketApp  to: @distdir\src\examples\ComLib.Apps.StockMarketApp		
		zip  dir @distdir\Src\Examples  					   to: @{dist.dir.root}\${app.name}_${build.version}_Examples.zip
	}


	#	Packages the binaries
	# @args: none
	# @example: [ Clean, Clean() ]
	def PackageBinaries
		print "${app.source.dir}" 
		zip dir "${app.source.dir}\Lib\CommonLibrary.NET\bin\${compile.configuration}",
			to: "${dist.dir.root}\${app.name}_${build.version}_Binaries.zip",
			include: [ *.html, *.doc, *.chm ]
	}


	#	Packages the documentation (.chm) file(s).
	# @args: none
	# @example: [ Clean, Clean() ]
	def PackageDocumentation
		print "${app.source.dir}" 
		zip dir "${app.base.dir}\Doc",
			to: "${dist.dir.root}\${app.name}_${build.version}_Documentation.zip",
			include: [ *.html, *.doc, *.chm ]		
	}


	#	Packages the sources.
	# @args: none
	# @example: [ Clean, Clean() ]
	def Package	
	{
		# make the dist dir
		make dir @distdir
		
		# copy the whole source directory to the dist dir
		copy dir "${app.base.dir}", to: @distdir, exclude: [ *.svn ]

		# delete directories with specified file extensions
		delete files @distdir\doc, 			[ .chm       ]
		delete files @distdir\src\Apps, 	[ .pdb, .xml ]
		delete files @distdir\src\Lib, 		[ .pdb, .xml ]
		delete files @distdir\src\Tests, 	[ .pdb, .xml ]
		delete files @distdir\dist
		
		# remove all the obj/bin directories.
		delete dir		  	@distdir\build\dist 
		delete dirs by name @distdir\src\Apps, 		named: [ bin, obj ], recurse: yes
		delete dirs by name @distdir\src\Lib, 		named: [ bin, obj ], recurse: yes
		delete dirs by name @distdir\src\Tests, 	named: [ bin, obj ], recurse: yes
		
		# zip up base dir for sources distribution
		zip folder "@distdir", file: @{dist.dir.root}\${app.name}_${build.version}_Sources.zip
	}


	# @args: none
	# @example: [ Clean, Clean() ]
	def CheckBaseDir
	{
		if ! file "${compile.solutionOrProject}" exists
			fail "${app.base.dir} does NOT EXIST!!!"			
	}

		
	# @summary: Executes the full build process
	# @args: none
	# @example: [ Clean, Clean() ]
	def Execute
	{
		Clean	 
		Version 
		Secure 
		Compile 
		Package 
		Package Binaries 
		Package Examples 
	}


