param (
	[Parameter(Mandatory=$true)]
	[ValidatePattern("\d\.\d\.\d\.\d")]
	[string]
	$ReleaseVersionNumber,

	[switch]$Push
)

$ErrorActionPreference = "Stop"

$PSScriptFilePath = (Get-Item $MyInvocation.MyCommand.Path).FullName

$SolutionRoot = Split-Path -Path $PSScriptFilePath -Parent
$NuGetExe = Join-Path $SolutionRoot -ChildPath ".nuget/nuget.exe"

# Build the NuGet package
$ProjectPath = Join-Path -Path $SolutionRoot -ChildPath "System.IO.Abstractions"
& dnu pack $ProjectPath --configuration Release --out $SolutionRoot
if (-not $?)
{
	throw "The NuGet process returned an error code."
}

$ProjectPath = Join-Path -Path $SolutionRoot -ChildPath "TestingHelpers"
& dnu pack $ProjectPath --configuration Release --out $SolutionRoot
if (-not $?)
{
	throw "The NuGet process returned an error code."
}

# Upload the NuGet package
if ($Push)
{
	$NuPkgPath = Join-Path -Path $SolutionRoot -ChildPath "Release/System.IO.Abstractions.$ReleaseVersionNumber.nupkg"
	& $NuGetExe push $NuPkgPath
	if (-not $?)
	{
		throw "The NuGet process returned an error code."
	}

	$NuPkgPath = Join-Path -Path $SolutionRoot -ChildPath "Release/System.IO.Abstractions.TestingHelpers.$ReleaseVersionNumber.nupkg"
	& $NuGetExe push $NuPkgPath
	if (-not $?)
	{
		throw "The NuGet process returned an error code."
	}
}