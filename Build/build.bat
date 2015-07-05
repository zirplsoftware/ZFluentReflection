@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)

set version=
if not "%PackageVersion%" == "" (
   set version=-Version %PackageVersion%
)

echo Build
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild Zirpl.FluentReflection.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false
if not "%errorlevel%"=="0" goto failure

echo Unit tests
call %nuget% install NUnit.Runners -Version 2.6.4 -OutputDirectory packages
packages\NUnit.Runners.2.6.4\tools\nunit-console.exe /config:%config% /framework:net-4.5 Zirpl.FluentReflection.Tests\bin\%config%\Zirpl.FluentReflection.Tests.dll
if not "%errorlevel%"=="0" goto failure

echo Package
mkdir .nuget\bin\%config%\
call %nuget% pack ".nuget\Zirpl.FluentReflection.nuspec" -symbols -o .nuget\bin\%config%\ -p Configuration=%config% %version%
if not "%errorlevel%"=="0" goto failure

:success
exit 0

:failure
exit -1