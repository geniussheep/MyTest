del *.nupkg
..\.nuget\NuGetX.exe -s http://nuget.benlailife.com:8004/nuget/arch -p .\ -b
for %%i in (*.csproj) do ( 	..\.nuget\NuGet.exe pack %%i -Symbols -Prop Configuration=Release )
for %%j in (*.symbols.nupkg) do ( 	..\.nuget\NuGet.exe push %%j 503B9B66-35DF-4322-B33D-0097A2E1B644 -Source http://nuget.benlailife.com:8004/nuget/arch )
pause