dotnet restore

dotnet build --configuration Debug
dotnet build --configuration Release

dotnet test -c Debug .\tests\TauCode.WebApi.Host.Cqrs.Tests\TauCode.WebApi.Host.Cqrs.Tests.csproj
dotnet test -c Release .\tests\TauCode.WebApi.Host.Cqrs.Tests\TauCode.WebApi.Host.Cqrs.Tests.csproj

nuget pack nuget\TauCode.WebApi.Host.Cqrs.nuspec
