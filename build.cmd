@ECHO OFF
ECHO.
ECHO.

REM ensure _Build_Output directory exists
mkdir Build 2>NUL

ECHO.
ECHO Checking Tools
cd Tools
..\.nuget\Nuget install packages.config -excludeversion
cd ..

ECHO.
ECHO.
powershell -Command "& { [Console]::WindowWidth = 150; [Console]::WindowHeight = 50; Start-Transcript -path "./Build/lastbuild.log"; Import-Module ".\Tools\psake\tools\psake.psm1"; Invoke-psake .\build-psake.ps1 %*; Stop-Transcript; }"
pause