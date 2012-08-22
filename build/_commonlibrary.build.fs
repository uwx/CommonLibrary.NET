
	
include "${args.settings.file}"

// Initialize settings.
var dist = 
{ 
	dir: 
	{
		root: "dist",
		name: ""
	} 
}.
dist.dir.root = "dist".
dist.dir.name = "${dist.dir.root}\${app.name}_${build.version}".


// ****************************************************************************************
//	@summary: Remove the temp/current directory 
// ****************************************************************************************
step "Clean"
{
	delete file   "${dist.dir.root}\${app.name}_${build.version}.zip" 
	delete folder "${dist.dir.name}" 
	delete folders(s)
		- "${dist.dir.name}\src\Apps\ComLib.Apps.CodeGeneration\bin\debug"
		- "${dist.dir.name}\src\Apps\ComLib.Apps.CodeGeneration\obj"
		- "${app.base.dir}\src\Apps\ComLib.Apps.SampleApp\bin\debug" 
		- "${app.base.dir}\src\Apps\ComLib.Apps.SampleApp\obj" 
		- "${app.base.dir}\src\Apps\ComLib.Apps.StockMarketApp\bin\debug" 
		- "${app.base.dir}\src\Apps\ComLib.Apps.StockMarketApp\obj" 
		- "${app.base.dir}\src\Lib\CommonLibrary.NET\bin\debug" 
		- "${app.base.dir}\src\Lib\CommonLibrary.NET\obj" 
		- "${app.base.dir}\src\Tests\CommonLibrary.UnitTests\bin\debug" 
		- "${app.base.dir}\src\Tests\CommonLibrary.UnitTests\obj"
}


// ***************************************************************************************
// @summary: Secures any sensitive data.
// ***************************************************************************************
step "Secure"
{
}


// ***************************************************************************************
// @summary: This writes out the version information into all the files named "AssemblyVersion.cs" 
// in the directory.
// ***************************************************************************************
step "Version"
{
	get all files in folder "${app.base.dir}\\src\\" include: "**\AssemblyVersion.cs"
	for each file in files
	{
		print "AssemblyInfo file name : ${file.FullName}" 	
		change read attributes on file  "${file.FullName}" non-readonly
		write all text to file "${file.FullName}" contents: "using System.Reflection; ${newline}[assembly: AssemblyVersionAttribute("${build.version}")]"
	}
}


// ****************************************************************************************
//	Compile the project.
// *******************************************************************************************-->
step "Compile"
{
	print "Compiling : ${compile.compiler} ${compile.solutionOrProject} /t:${compile.target} /p:Configuration=${compile.configuration}" 		
	run program "${compile.compiler}" in_working_dir: "${compile.workingDir}" don't failonerror, 
		args: [ "${compile.solutionOrProject}", "/t:${compile.target}", "/p:Configuration=${compile.configuration}" ]
}


// ****************************************************************************************
// @summary: Packages the examples into a zip file.	
// ****************************************************************************************
step "PackageExamples" :
	print "${app.source.dir}" 
	create folder "${dist.dir.name}\src\Examples" 
	create folder "${dist.dir.name}\src\Examples\ComLib.Apps.SampleApp" 
	create folder "${dist.dir.name}\src\Examples\ComLib.Apps.StockMarketApp"
	copy   folder "${dist.dir.name}\src\Lib\CommonLibrary.NET\_Samples" 	to: "${dist.dir.name}\src\examples" include: "**/*"		
	copy   folder "${dist.dir.name}\src\Apps\ComLib.Apps.SampleApp" 	  	to: "${dist.dir.name}\src\examples\ComLib.Apps.SampleApp" include: "**/*"
	copy   folder "${dist.dir.name}\src\Apps\ComLib.Apps.StockMarketApp"    to: "${dist.dir.name}\src\examples\ComLib.Apps.StockMarketApp" include: "**/*" 		
	zip    folder "${dist.dir.name}\Src\Examples" to: "${dist.dir.root}\${app.name}_${build.version}_Examples.zip" include: "**/*"


// ****************************************************************************************
// @summary: Pacakges the binaries
// ****************************************************************************************
step "PackageBinaries"
{
	print "${app.source.dir}"
	zip folder "${app.source.dir}\Lib\CommonLibrary.NET\bin\${compile.configuration}" to: "${dist.dir.root}\${app.name}_${build.version}_Binaries.zip" include: "**/*dll" 		
}


// ****************************************************************************************
// @summary: Pacakges the documentation .chm file(s).
// ****************************************************************************************
step "PackageDocumentation"
{
	print "${app.source.dir}" 
	zip folder "${app.base.dir}\Doc" to: "${dist.dir.root}\${app.name}_${build.version}_Documentation.zip" include: "**/*.chm"
}


// ****************************************************************************************
// @summary: Pacakges the documentation .chm file(s).
// ****************************************************************************************	
step "Package" 
{
	create folder "${dist.dir.name}" 
	copy   folder "${app.base.dir}" to: "${dist.dir.name}" include:"**/*" 

	delete file(s): 
		- "${dist.dir.name}\doc\*.chm" 		        				
		- "${dist.dir.name}\src\Apps\ComLib.Apps.CodeGeneration\bin\debug\*.pdb" 
		- "${dist.dir.name}\src\Apps\ComLib.Apps.SampleApp\bin\debug\*.pdb" 
		- "${dist.dir.name}\src\Apps\ComLib.Apps.SampleApp\bin\debug\*.xml" 
		- "${dist.dir.name}\src\Apps\ComLib.Apps.StockMarketApp\bin\debug\*.pdb" 
		- "${dist.dir.name}\src\Apps\ComLib.Apps.StockMarketApp\bin\debug\*.xml" 
		- "${dist.dir.name}\src\Lib\CommonLibrary.NET\bin\debug\*.pdb" 
		- "${dist.dir.name}\src\Lib\CommonLibrary.NET\bin\debug\*.xml" 
		- "${dist.dir.name}\src\Tests\CommonLibrary.UnitTests\bin\debug\*.pdb" 
		- "${dist.dir.name}\src\Tests\CommonLibrary.UnitTests\bin\debug\*.xml" 
		- "${dist.dir.name}\dist\*.zip" 
		
	delete folder(s):		
		- "${dist.dir.name}\build\dist" 
		- "${dist.dir.name}\src\Apps\ComLib.Apps.SampleApp\obj" 
		- "${dist.dir.name}\src\Apps\ComLib.Apps.SampleApp\bin" 
		- "${dist.dir.name}\src\Apps\ComLib.Apps.CodeGeneration\obj" 
		- "${dist.dir.name}\src\Apps\ComLib.Apps.CodeGeneration\bin" 
		- "${dist.dir.name}\src\Apps\ComLib.Apps.StockMarketApp\obj" 
		- "${dist.dir.name}\src\Apps\ComLib.Apps.StockMarketApp\bin" 
		- "${dist.dir.name}\src\Lib\CommonLibrary.NET\obj" 
		- "${dist.dir.name}\src\Lib\CommonLibrary.NET\bin" 
		- "${dist.dir.name}\src\Tests\CommonLibrary.UnitTests\bin" 		
		- "${dist.dir.name}\src\Tests\CommonLibrary.UnitTests\obj"
			
	zip folder "${dist.dir.name}" to: "${dist.dir.root}\${app.name}_${build.version}_Sources.zip" include: "**/*" 
}

	
// ****************************************************************************************
// @summary: Execute the build
// ****************************************************************************************
step "Execute"
{
	run "Clean" 
	run "Version" 
	run "Secure" 
	run "Compile" 
	run "Package" 
	run "PackageBinaries" 
	run "PackageExamples"	
}