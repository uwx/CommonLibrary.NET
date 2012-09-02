
# file methods
def find_file
{

}


def write_file
{

}


	# <loadtasks assembly="..\lib\External\NAntContrib\0.85\NAnt.Contrib.Tasks.dll"  -->
	
	build.dir.public = "" 
	dist.dir.root = "dist" 
	dist.dir = "${dist.dir.root}\${app.name}_${build.version}"
		
		
	# @summary: Removes temp files and directories created as part of the last build.
	def Clean
		delete file "${dist.dir.root}\${app.name}_${build.version}.zip" 
		delete dir "${dist.dir}" 
		
		delete named dirs @{app.base.dir}\src\Apps, 	named: [ 'bin', 'obj' ], recurse: yes
		delete named dirs @{app.base.dir}\src\Lib, 		named: [ 'bin', 'obj' ], recurse: yes
		delete named dirs @{app.base.dir}\src\Tests, 	named: [ 'bin', 'obj' ], recurse: yes
	}
			
	
	# @summary: Secures any files.
	def Secure
	{
	}
	
	
	# @summary: This writes out the version information into all the files named "AssemblyVersion.cs" in the directory.
	def Version
	{
		files = find files 'AssemblyVersion.cs' in: @{app.base.dir}\src		
		for file in files
			write file file.fullname, "${build.verion}"
	}
	
	
	# @summary	Compile the project.
	def Compile
		print "Compiling : ${compile.compiler} ${compile.solutionOrProject} /t:${compile.target} /p:Configuration=${compile.configuration}" 		
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
	
	
	# ****************************************************************************************
		Compile the project.
	*******************************************************************************************-->
	def PackageExamples
		print "${app.source.dir}" 
		make dir "${dist.dir}\src\Examples" 
		make dir "${dist.dir}\src\Examples\ComLib.Apps.SampleApp" 
		make dir "${dist.dir}\src\Examples\ComLib.Apps.StockMarketApp" 
		
		
		<copy todir="${dist.dir}\src\examples
		    <fileset basedir="${dist.dir}\src\Lib\CommonLibrary.NET\_Samples
		        <include name="**/*" 
		    </fileset>
		</copy>
		<copy todir="${dist.dir}\src\examples\ComLib.Apps.SampleApp
			<fileset basedir="${dist.dir}\src\Apps\ComLib.Apps.SampleApp
		        <include name="**/*" 
		    </fileset>
		</copy>
		<copy todir="${dist.dir}\src\examples\ComLib.Apps.StockMarketApp
			<fileset basedir="${dist.dir}\src\Apps\ComLib.Apps.StockMarketApp
		        <include name="**/*" 
		    </fileset>
		</copy>
		
		<zip zipfile="${dist.dir.root}\${app.name}_${build.version}_Examples.zip
		    <!--<fileset basedir="${dist.dir}\Src\Lib\CommonLibrary.NET\_Samples
		        <include name="**/*" 
		    </fileset>-->
			<fileset basedir="${dist.dir}\Src\Examples
		        <include name="**/*" 
		    </fileset>
		</zip>
	}
	
	
	# ****************************************************************************************
		Packages the binaries
	*******************************************************************************************-->
	def PackageBinaries
		<echo message="${app.source.dir}" 
		<zip zipfile="${dist.dir.root}\${app.name}_${build.version}_Binaries.zip
		    <fileset basedir="${app.source.dir}\Lib\CommonLibrary.NET\bin\${compile.configuration}
		        <include name="**/*dll" 
		    </fileset>
		</zip>
	}
	
	
	# ****************************************************************************************
		Packages the documentation (.chm) file(s).
	*******************************************************************************************-->
	def PackageDocumentation
		<echo message="${app.source.dir}" 
		<zip zipfile="${dist.dir.root}\${app.name}_${build.version}_Documentation.zip
		    <fileset basedir="${app.base.dir}\Doc
		        <include name="**/*.chm" 
		    </fileset>
		</zip>
	}
	
	
	# ****************************************************************************************
		Packages the sources.
	*******************************************************************************************-->
	def Package	
	{
		# make the dist dir
		make dir "${dist.dir}"
		
		# copy the whole source directory to the dist dir
		copy dir "${app.base.dir}", to: "${dist.dir}", exclude: [ *.svn ]

		# delete directories with specified file extensions
		delete dir @{dist.dir}\doc, 		[ .chm       ]
		delete dir @{dist.dir}\src\Apps, 	[ .pdb, .xml ]
		delete dir @{dist.dir}\src\Lib, 	[ .pdb, .xml ]
		delete dir @{dist.dir}\src\Tests, 	[ .pdb, .xml ]
		delete dir @{dist.dir}\dist
		
		# remove all the obj/bin directories.
		delete named dirs @{dist.dir}\build\dist 
		delete named dirs @{dist.dir}\src\Apps, 	named: [ 'bin', 'obj' ], recurse: yes
		delete named dirs @{dist.dir}\src\Lib, 		named: [ 'bin', 'obj' ], recurse: yes
		delete named dirs @{dist.dir}\src\Tests, 	named: [ 'bin', 'obj' ], recurse: yes
		
		# zip up base dir for sources distribution
		zip folder "${dist.dir}", file: @{dist.dir.root}\${app.name}_${build.version}_Sources.zip
	}
	

	def CheckBaseDir
	{
		if ! file "${compile.solutionOrProject}" exists
			fail "${app.base.dir} does NOT EXIST!!!"			
	}
	
		
	# @summary: Executes the full build process
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