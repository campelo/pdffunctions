@echo off
setlocal enabledelayedexpansion
set TEST_NAME=AnnotationTest
for /f %%i in ('dotnet --version') do set ver=%%i
set net=0
echo %ver% | findstr /r "7\..*" > NUL
if %ERRORLEVEL% EQU 0 set net=net7.0
echo %ver% | findstr /r "6\..*" > NUL
if %ERRORLEVEL% EQU 0 set net=net6.0
echo %ver% | findstr /r "5\..*"  > NUL
if %ERRORLEVEL% EQU 0 set net=net5.0
echo %ver% | findstr /r "3\..*" > NUL
if %ERRORLEVEL% EQU 0 set net=netcoreapp2.1
echo %ver% | findstr /r "2\..*" > NUL
if %ERRORLEVEL% EQU 0 set net=netcoreapp2.1

set "textfile=%TEST_NAME%.csproj"
set "tempfile=%TEST_NAME%.csproj.txt"
(for /f "delims=" %%i in (%textfile%) do (
    set "line=%%i"
    setlocal enabledelayedexpansion
    set "line=!line:<TargetFramework>netcoreapp2.1</TargetFramework>=<TargetFramework>%net%</TargetFramework>!"
    set "line=!line:<TargetFramework>net5.0</TargetFramework>=<TargetFramework>%net%</TargetFramework>!"
    set "line=!line:<TargetFramework>net6.0</TargetFramework>=<TargetFramework>%net%</TargetFramework>!"
    set "line=!line:<TargetFramework>net7.0</TargetFramework>=<TargetFramework>%net%</TargetFramework>!"
    echo(!line!
))>"%tempfile%"
del %textfile%
rename %tempfile%  %textfile%
call dotnet run
exit /b %errorlevel%
endlocal