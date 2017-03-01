@echo on
powershell -Command "wget https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -OutFile ./tools/nuget/nuget.exe"
tools\nuget\nuget.exe update -self
tools\nuget\nuget.exe install Cake -OutputDirectory tools -ExcludeVersion
tools\Cake\Cake.exe build.cake

exit /b %errorlevel%