dotnet clean .\src\TypeGen\TypeGen.Core
dotnet restore .\src\TypeGen\TypeGen.Core
dotnet build .\src\TypeGen\TypeGen.Core -f netstandard1.3 -c Release
dotnet build .\src\TypeGen\TypeGen.Core -f netstandard2.0 -c Release

dotnet clean .\src\TypeGen\TypeGen.Cli
dotnet restore .\src\TypeGen\TypeGen.Cli
dotnet publish .\src\TypeGen\TypeGen.Cli -c Release -f netcoreapp2.2