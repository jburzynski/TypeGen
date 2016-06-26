copy src\TypeGen\TypeGen.Cli\bin\Debug\TypeGen.Cli.exe nuget\tools\type-gen.exe
copy src\TypeGen\TypeGen.Core\bin\Debug\TypeGen.Core.dll nuget\tools
copy src\TypeGen\TypeGen.Types\bin\Debug\TypeGen.Types.dll nuget\tools
copy src\TypeGen\TypeGen.Types\bin\Debug\TypeGen.Types.dll nuget\lib

nuget pack nuget\TypeGen.nuspec

copy nuget\TypeGen.1.0.0.nupkg D:\OneDrive\D\Projects\custom-nuget
