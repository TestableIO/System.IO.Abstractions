param (
	[Parameter(Mandatory=$true)]
	[ValidatePattern("\d\.\d\.\d\.\d")]
	[string]
	$ReleaseVersionNumber,

	[switch]$Push
)
Import-Module -Name .\Invoke-MsBuild.psm1

function Update-BuildNumbers([string] $VersionNumber) {
	$assemblyPattern = "[0-9]+(\.([0-9]+|\*)){1,3}"
  $assemblyVersionPattern = 'AssemblyVersion\("([0-9]+(\.([0-9]+\*)){1,3})"\)'

  $foundFiles = get-childitem .\AssemblyFileVersion.cs

  foreach ($file in $foundFiles) {
  	(Get-Content $file) | ForEach-Object {
  		% {$_ -replace $assemblyPattern, $VersionNumber } 
  	} | Set-Content $file
  }
}

$ErrorActionPreference = "Stop"

$PSScriptFilePath = (Get-Item $MyInvocation.MyCommand.Path).FullName

$SolutionRoot = Split-Path -Path $PSScriptFilePath -Parent
$NuGetExe = Join-Path $SolutionRoot -ChildPath ".nuget/nuget.exe"
Update-BuildNumbers($ReleaseVersionNumber)

# Build System.IO.Abstractions for each Framework
$ProjectPath = Join-Path -Path $SolutionRoot -ChildPath "System.IO.Abstractions\System.IO.Abstractions.csproj"
Invoke-MsBuild -Path $ProjectPath -MsBuildParameters "/p:Configuration=Release;TargetFrameworkVersion=v3.5 /t:clean;rebuild /v:Quiet " 
Invoke-MsBuild -Path $ProjectPath -MsBuildParameters "/p:Configuration=Release;TargetFrameworkVersion=v4.0 /t:clean;rebuild /v:Quiet " 
Invoke-MsBuild -Path $ProjectPath -MsBuildParameters "/p:Configuration=Release;TargetFrameworkVersion=v4.5 /t:clean;rebuild /v:Quiet " 

$SpecFile = Join-Path -Path $SolutionRoot -ChildPath "System.IO.Abstractions\System.IO.Abstractions.nuspec"
& $NuGetExe pack $SpecFile -Version $ReleaseVersionNumber
if (-not $?)
{
	throw "The NuGet process returned an error code."
}

# Build TestingHelpers for each Framework
$ProjectPath = Join-Path -Path $SolutionRoot -ChildPath "TestingHelpers\TestingHelpers.csproj"
Invoke-MsBuild -Path $ProjectPath -MsBuildParameters "/p:Configuration=Release;TargetFrameworkVersion=v3.5 /t:clean;rebuild /v:Quiet "
Invoke-MsBuild -Path $ProjectPath -MsBuildParameters "/p:Configuration=Release;TargetFrameworkVersion=v4.0 /t:clean;rebuild /v:Quiet "
Invoke-MsBuild -Path $ProjectPath -MsBuildParameters "/p:Configuration=Release;TargetFrameworkVersion=v4.5 /t:clean;rebuild /v:Quiet "

$SpecFile = Join-Path -Path $SolutionRoot -ChildPath "TestingHelpers\TestingHelpers.nuspec"
& $NuGetExe pack $SpecFile -Version $ReleaseVersionNumber
if (-not $?)
{
	throw "The NuGet process returned an error code."
}

 Upload the NuGet package
if ($Push)
{
	$NuPkgPath = Join-Path -Path $SolutionRoot -ChildPath "System.IO.Abstractions.$ReleaseVersionNumber.nupkg"
	& $NuGetExe push $NuPkgPath
	if (-not $?)
	{
		throw "The NuGet process returned an error code."
	}

	$NuPkgPath = Join-Path -Path $SolutionRoot -ChildPath "System.IO.Abstractions.TestingHelpers.$ReleaseVersionNumber.nupkg"
	& $NuGetExe push $NuPkgPath
	if (-not $?)
	{
		throw "The NuGet process returned an error code."
	}
}
