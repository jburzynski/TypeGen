dotnet clean .\src\TypeGen\TypeGen.Core
dotnet restore .\src\TypeGen\TypeGen.Core
dotnet build .\src\TypeGen\TypeGen.Core -f netstandard1.3 -c Release
dotnet build .\src\TypeGen\TypeGen.Core -f netstandard2.0 -c Release

dotnet clean .\src\TypeGen\TypeGen.Cli
dotnet restore .\src\TypeGen\TypeGen.Cli
dotnet publish .\src\TypeGen\TypeGen.Cli -c Release -f netcoreapp2.1
dotnet publish .\src\TypeGen\TypeGen.Cli -c Release -f netcoreapp2.2
dotnet publish .\src\TypeGen\TypeGen.Cli -c Release -f netcoreapp3.0
dotnet publish .\src\TypeGen\TypeGen.Cli -c Release -f netcoreapp3.1
dotnet publish .\src\TypeGen\TypeGen.Cli -c Release -f net5.0
