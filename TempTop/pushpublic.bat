devenv "..\TempTop.sln" /build Release /project "TempTop.csproj"

nuget pack TempTop.csproj -Version 1.0.0.2

nuget push TempTop.1.0.0.2.nupkg -Source https://api.nuget.org/v3/index.json

pause