properties { 

	$signAssembliesEnabled = $false   # global override for signing
	$signKeyPath = "D:\Development\Releases\newtonsoft.snk"  #path for whomever can sign the assemblies

	$buildDocumentation = $false
	$treatWarningsAsErrors = $false

	$baseDir  = resolve-path "."
	$buildDir = "$baseDir\Build"
	$sourceDir = "$baseDir"
	$toolsDir = "$baseDir\Tools"
	$testsDir = "$baseDir\Build\Testing"
	$docDir = "$baseDir\Doc"
	$workingDir = "$baseDir\Build\Working"
	$nugetBaseDir = "$baseDir\Build\NuGet"
  
	$versionInfo = Load-VersionInfo -path "$sourceDir\AssemblyVersion_Master.cs"
  
	$nuget_executible = "$sourceDir\.nuget\NuGet.exe"

   $builds = @(

	@{Project = "System.IO.Abstractions.csproj";Tests = ""; 						Constants=""; FinalDir="Net"; NuGetDir = ""; Framework="net-4.0"; Sign=$false}
	@{Project = "TestingHelpers.csproj"; 		Tests = "TestHelpers.Tests.csproj"; Constants=""; FinalDir="Net"; NuGetDir = ""; Framework="net-4.0"; Sign=$false}
    
	#when some 4.0 specific things come about:
	#@{Project = "System.IO.Abstractions.net40.csproj"; Tests = "";					Constants=""; FinalDir="Net40"; NuGetDir = "net40"; Framework="net-4.0"; Sign=$false}
	#@{Project = "TestingHelpers.net40.csproj"; 		Tests = "TestHelpers.Tests.csproj"; Constants=""; FinalDir="Net40"; NuGetDir = "net40"; Framework="net-4.0"; Sign=$false}
	
	#@{Project = "Newtonsoft.Json"; TestsName = "Newtonsoft.Json.Tests"; Constants=""; FinalDir="Net40"; NuGetDir = "net40"; Framework="net-4.0"; Sign=$true},
    #@{Project = "Newtonsoft.Json.Portable"; TestsName = "Newtonsoft.Json.Tests.Portable"; Constants="PORTABLE"; FinalDir="Portable"; NuGetDir = "portable-net40+sl4+wp7+win8"; Framework="net-4.0"; Sign=$true},
    #@{Project = "Newtonsoft.Json.WinRT"; TestsName = $null; Constants="NETFX_CORE"; FinalDir="WinRT"; NuGetDir = "winrt45"; Framework="net-4.5"; Sign=$true},
    #@{Project = "Newtonsoft.Json.WindowsPhone"; TestsName = $null; Constants="SILVERLIGHT;WINDOWS_PHONE"; FinalDir="WindowsPhone"; NuGetDir = "sl3-wp,sl4-windowsphone71"; Framework="net-4.0"; Sign=$true},
    #@{Project = "Newtonsoft.Json.Silverlight"; TestsName = "Newtonsoft.Json.Tests.Silverlight"; Constants="SILVERLIGHT"; FinalDir="Silverlight"; NuGetDir = "sl4"; Framework="net-4.0"; Sign=$true},
    #@{Project = "Newtonsoft.Json.Net35"; TestsName = "Newtonsoft.Json.Tests.Net35"; Constants="NET35"; FinalDir="Net35"; NuGetDir = "net35"; Framework="net-2.0"; Sign=$true},
    #@{Project = "Newtonsoft.Json.Net20"; TestsName = "Newtonsoft.Json.Tests.Net20"; Constants="NET20"; FinalDir="Net20"; NuGetDir = "net20"; Framework="net-2.0"; Sign=$true}
   )
}

$framework = '4.0x86'

# Set up the default task, when calling build-psake with no -task parameter
# 
task default -depends Test
#additional tasks that simply combine actual tasks can go here:
#task DebugBuild -depends SetConfigDebug, Build
#task DebugTest -depends SetConfigDebug, Build, Test

# Ensure a clean working directory
task Clean {
  Set-Location $baseDir
  
  Write-Verbose "-Working Directory:	$workingDir"
  if (Test-Path -path $workingDir)
  {
    del $workingDir -Recurse -Force
    Write-Host -ForegroundColor Green "-Deleted Working Directory: $workingDir"
  }
  
  Write-Verbose "-Testing Directory:	$workingDir"
  if (Test-Path -path $testsDir)
  {
	del $testsDir -Recurse -Force
	Write-Host -ForegroundColor Green "-Deleted Testing directory: $testsDir"
  }
  
  Write-Verbose "-NuGet Directory:	$workingDir"
  if (Test-Path -path $nugetBaseDir)
  {
	del $nugetBaseDir -Recurse -Force
	Write-Host -ForegroundColor Green "-Deleted NuGet base directory: $nugetBaseDir"
  }
  
}

# Build each solution, optionally signed
task Build -depends Clean,UpdateAssemblyInfoVersions { 

	Write-Verbose "-Ensure Working Directory exists: $workingDir"
	New-Item -Path $workingDir -ItemType Directory | Out-Null

	foreach ($build in $builds)
	{
		$nameInfo = new-object System.IO.FileInfo([string]$build.Project)
		$projname = $nameInfo.Name.Replace($nameInfo.Extension,"")  # chop off the extension
		if ($nameInfo.Extension -eq ".sln") { 
			$name = $nameInfo.Name
		} else {
			$name = (Join-Path $projname $nameInfo.Name)            # gives [name]\[name.csproj]
		}
		
		$finalDir = $build.FinalDir
		$sign = ($build.Sign -and $signAssembliesEnabled)

		$outputPath = "$workingDir\$projname\bin\Release\$finalDir"
		Write-Host -ForegroundColor DarkCyan "-Building	" $name
		Write-Host -ForegroundColor DarkCyan "-Signed		" $sign
		exec { msbuild /v:m "/t:Clean;Rebuild" /p:Configuration=Release "/p:Platform=Any CPU" "/p:OutputPath=$outputPath\" /p:AssemblyOriginatorKeyFile=$signKeyPath "/p:SignAssembly=$sign" "/p:TreatWarningsAsErrors=$treatWarningsAsErrors" (GetConstants $build.Constants $sign) "$sourceDir\$name" } "Error building $name"
		Write-Host -ForegroundColor Green "-MSBuild succeeded for"  $build.Project #"->" $outputPath
	}
}

task UpdateAssemblyInfoVersions {

	#sanity check
	$assemblyVer = new-object Version($versionInfo.MajorMinorRevision + '.0')
	$fileVer = new-object Version($versionInfo.FullString)

	Write-Host -ForegroundColor DarkCyan "-Updating assembly versions:"
	Write-Host -ForegroundColor DarkCyan "-AssemblyVersion:     $assemblyVer"
	Write-Host -ForegroundColor DarkCyan "-AssemblyFileVersion: $fileVer"
	Update-AssemblyInfoFiles $sourceDir $assemblyVer $fileVer
}

# Run tests on deployed files
#task Test -depends Deploy {
task Test -depends Build {

	Write-Verbose "Ensuring Testing Directory exists: $testsDir"
	if ((Test-Path -path $testsDir) -eq $false)
	{
		New-Item -Path $testsDir -ItemType Directory | Out-Null
	}

	foreach ($build in $builds)
	{
		if ([System.String]::IsNullOrEmpty($build.Tests) -ne $true )
		{
			$nameInfo = new-object System.IO.FileInfo([string]$build.Tests)
			$projname = $nameInfo.Name.Replace($nameInfo.Extension,"")  # chop off the extension
			if ($nameInfo.Extension -eq ".sln") { 
				$name = $nameInfo.Name
			} else { #if ($nameInfo.Extension -eq ".csproj") {
				$name = (Join-Path $projname $nameInfo.Name)            # gives [name]\[name.csproj]
			}
		
			$finalDir = $build.FinalDir
			$frameworkDirs = $build.Framework.Split(",")

			foreach ($frameworkDir in $frameworkDirs)
			{
				$outputPath = "$testsDir\$projname\bin\$frameworkDir"
				Write-Host -ForegroundColor DarkCyan "-Building tests assembly $name for $frameworkDir"
				Write-Host -ForegroundColor DarkCyan "-OutputPath=$testsDir\$projname\bin\$frameworkDir"
				Write-Host
				exec { & msbuild /v:m /p:Configuration=Release "/p:OutputPath=$outputPath\" "$sourceDir\$name" }
				Write-Host -ForegroundColor Green "-MSBuild succeeded for"  $build.Project #"->" $outputPath

				Write-Host -ForegroundColor DarkCyan "-Running tests $projname for $frameworkDir"
				exec { & .\Tools\NUnit.Runners\tools\nunit-console.exe "$testsDir\$projname\bin\$frameworkDir\$projname.dll" /xml:$workingDir\$projname.xml } "Error running $name tests"
				Write-Host -ForegroundColor Green "-Finished tests. TestResults: $workingDir\$projname.xml"
			}
		}
	}
}

# Optional build documentation, add files to final zip
task Package -depends Test {
  foreach ($build in $builds)
  {
    $name = $build.TestsName
    $finalDir = $build.FinalDir
    
    #robocopy "$sourceDir\Google.Maps\bin\Release\$finalDir" $workingDir\Package\Bin\$finalDir /NP /XO /XF *.pri | Out-Default
  }
  
#  if ($buildDocumentation)
#  {
#    $mainBuild = $builds | where { $_.Name -eq "Newtonsoft.Json" } | select -first 1
#    $mainBuildFinalDir = $mainBuild.FinalDir
#    $documentationSourcePath = "$workingDir\Package\Bin\$mainBuildFinalDir"
#    Write-Host -ForegroundColor DarkCyan "Building documentation from $documentationSourcePath"
#
#    # Sandcastle has issues when compiling with .NET 4 MSBuild - http://shfb.codeplex.com/Thread/View.aspx?ThreadId=50652
#    exec { msbuild "/t:Clean;Rebuild" /p:Configuration=Release "/p:DocumentationSourcePath=$documentationSourcePath" $docDir\doc.shfbproj | Out-Default } "Error building documentation. Check that you have Sandcastle, Sandcastle Help File Builder and HTML Help Workshop installed."
#    
#    move -Path $workingDir\Documentation\LastBuild.log -Destination $workingDir\Documentation.log
#  }
#
#  Copy-Item -Path $docDir\readme.txt -Destination $workingDir\Package\
#  Copy-Item -Path $docDir\versions.txt -Destination $workingDir\Package\Bin\
#
#  robocopy $sourceDir $workingDir\Package\Source\Src /MIR /NP /XD .svn bin obj TestResults AppPackages /XF *.suo *.user | Out-Default
#  robocopy $buildDir $workingDir\Package\Source\Build /MIR /NP /XD .svn | Out-Default
#  robocopy $docDir $workingDir\Package\Source\Doc /MIR /NP /XD .svn | Out-Default
#  robocopy $toolsDir $workingDir\Package\Source\Tools /MIR /NP /XD .svn | Out-Default
  
#  exec { .\Tools\7-zip\7za.exe a -tzip $workingDir\$zipFileName $workingDir\Package\* | Out-Default } "Error zipping"
}

# Unzip package to a location
task Deploy -depends Package {
  exec { .\Tools\7-zip\7za.exe x -y "-o$workingDir\Deployed" $workingDir\$zipFileName | Out-Default } "Error unzipping"
}

task PrepareNuspecFiles {
	
	Write-Verbose "-Ensure Nuget directory exists: $nugetBaseDir"
	New-Item -Path $nugetBaseDir -ItemType Directory -force | Out-Null

	foreach ($build in $builds) {
	
		if($build.NuGetDir -ne $null) {
			$nameInfo = new-object System.IO.FileInfo([string]$build.Project)
			$projname = $nameInfo.Name.Replace($nameInfo.Extension,"")  # chop off the extension
			$name = (Join-Path $projname $nameInfo.Name)                # gives [name]\[name.csproj]
			
			Get-ChildItem -Path (Join-Path $sourceDir $projname) -filter "*.nuspec" | Foreach-Object {
		
				$nugetPackDir = Join-Path $nugetBaseDir $projname
			
				Write-Verbose "-Ensure Directory: $nugetPackDir"
				New-Item -Path $nugetPackDir -ItemType Directory -force | Out-Null

				$nuspecFile = $_.Name
				Write-Host -ForegroundColor DarkCyan "-Copy $nuspecFile into pack directory: $nugetPackDir"
				Copy-Item -Path (Join-Path ($_.Directory.ToString()) ($_.Name.ToString())) -Destination $nugetPackDir
		
				$filename = "$nugetPackDir\$nuspecFile"

				$versionStr = ($versionInfo.MajorMinorRevision + ".0")
				#$versionNodePattern1 = "<version>[0-9]+(\.([0-9]+|\*)){1,3}</version>"
				$versionNodePattern1 = '<version>$version$</version>'
				$versionNodeOut = "<version>" + $versionStr + "</version>"
				$versionAttrPattern1 = 'version="$version$"'
				$versionAttrOut = 'version="' + $versionStr + '"'
				
				(Get-Content $filename) | Foreach-Object {
					% { $_.Replace($versionNodePattern1, $versionNodeOut) } |
					% { $_.Replace($versionAttrPattern1, $versionAttrOut) }
				} | Set-Content $filename

				Write-Host -ForegroundColor Green "-Updated version(s) in $filename"
			}
		}
	}
}

task NugetPackage -depends Test,PrepareNuspecFiles {
    
	Write-Verbose "-Ensure Nuget directory exists: $nugetBaseDir"
	New-Item -Path $nugetBaseDir -ItemType Directory -force | Out-Null
    
    foreach ($build in $builds)
    {
      if ($build.NuGetDir -ne $null)
	{
		$nameInfo = new-object System.IO.FileInfo([string]$build.Project)
		$projname = $nameInfo.Name.Replace($nameInfo.Extension,"")  # chop off the extension
		$name = (Join-Path $projname $nameInfo.Name)                # gives [name]\[name.(csproj|sln)]

		$nugetPackDir = "$nugetBaseDir\$projname"

		$finalDir = $build.FinalDir
		$frameworkDirs = $build.NuGetDir.Split(",")

		foreach ($frameworkDir in $frameworkDirs)
		{
			Write-Host -ForegroundColor DarkCyan "-Copy files into $nugetPackDir\lib\$frameworkDir"
			robocopy "$workingDir\$projname\bin\Release\$finalDir" "$nugetPackDir\lib\$frameworkDir" /NP /XO /XF *.pri | Out-Null
		}
		
		Get-ChildItem -Path $nugetPackDir -Filter *.nuspec | Foreach-Object {
			$nuspecFile = (Join-Path $nugetPackDir $_.Name)

			Write-Host -ForegroundColor DarkCyan "-NuGet pack $_"
			exec { & $nuget_executible pack $nuspecFile -Symbols -OutputDirectory $nugetPackDir } "Failed during NuGet Pack phase."
			Write-Host -ForegroundColor Green "-NuGet pack succeeded. $nuspecFile"
		}
		
		Write-Host -ForegroundColor DarkCyan "-Copy nupkg files into Nuget base: $nugetBaseDir"
		copy -Path $nugetPackDir\*.nupkg -Destination $nugetBaseDir
		}
	}
	
	Write-Host -ForegroundColor Green "-NuGet Packages Built:"
	Get-ChildItem -Path $nugetBaseDir -Filter *.nupkg | Foreach-Object {
		Write-Host -ForegroundColor Green "-Package: " $_.Name
	}
  
}

task NugetPackageDeploy -depends NugetPackage
{
	Write-Output "NotImplementedException"
}

#########################################################
#########################################################
#########################################################

function GetConstants($constants, $includeSigned)
{
  $signed = switch($includeSigned) { $true { ";SIGNED" } default { "" } }

  return "/p:DefineConstants=`"CODE_ANALYSIS;TRACE;$constants$signed`""
}

# Gets an integer revision number based on the date
function Get-BuildTiming()
{
    $now = [DateTime]::Now
	[TimeSpan]$span = $now - (new-object DateTime($now.Year,1,1))
    
    Return [int]$span.TotalMinutes
}

function Load-VersionInfo([string] $path)
{
	[hashtable]$Return = @{}

	$Return.MajorMinor = "1.4"
	$Return.MajorMinorRevision = "1.4.1"

	$Return.Build = Get-BuildTiming
	$Return.FullString = [String]::Concat($Return.MajorMinorRevision,".",[string]$Return.Build)
	
	Return $Return
}

# Update any (recursively found under $sourceDir) AssemblyInfo.cs files
function Update-AssemblyInfoFiles ([string] $sourceDir, [string] $assemblyVersionNumber, [string] $fileVersionNumber)
{
    $assemblyVersionPattern = 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $fileVersionPattern = 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $assemblyVersion = 'AssemblyVersion("' + $assemblyVersionNumber + '")';
    $fileVersion = 'AssemblyFileVersion("' + $fileVersionNumber + '")';
    
    Get-ChildItem -Path $sourceDir -r -filter AssemblyInfo.cs | ForEach-Object {
        $filename = $_.Directory.ToString() + '\' + $_.Name
    
        (Get-Content $filename) | ForEach-Object {
            % {$_ -replace $assemblyVersionPattern, $assemblyVersion } |
            % {$_ -replace $fileVersionPattern, $fileVersion }
        } | Set-Content $filename
		
		Write-Host -ForegroundColor Green "-Updated versions in:" $filename
    }
}