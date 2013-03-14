@ECHO OFF
ECHO.
ECHO.

REM ensure _Build_Output directory exists
mkdir Build 2>NUL

ECHO Checking Tools
cd Tools
Nuget install packages.config -excludeversion
cd ..

ECHO.
ECHO.
powershell -Command "& { Start-Transcript -path "./Build/lastbuild.log"; Import-Module ".\Tools\psake\tools\psake.psm1"; Invoke-psake .\build-psake.ps1 %*; Stop-Transcript; }"
pause