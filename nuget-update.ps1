#nuget

#tools

rm -Recurse -Force nuget\tools\runtimes
copy -Recurse src\TypeGen\TypeGen.Cli\bin\Release\netcoreapp2.1\publish\* nuget\tools

#lib

#netstandard1.3
copy src\TypeGen\TypeGen.Core\bin\Release\netstandard1.3\TypeGen.Core.dll nuget\lib\netstandard1.3
copy src\TypeGen\TypeGen.Core\bin\Release\netstandard1.3\TypeGen.Core.xml nuget\lib\netstandard1.3

#netstandard2.0
copy src\TypeGen\TypeGen.Core\bin\Release\netstandard2.0\TypeGen.Core.dll nuget\lib\netstandard2.0
copy src\TypeGen\TypeGen.Core\bin\Release\netstandard2.0\TypeGen.Core.xml nuget\lib\netstandard2.0

nuget pack nuget\TypeGen.nuspec
move TypeGen.2.3.1.nupkg nuget -force

if (Test-Path "local-nuget-path.txt") {
  $localNuGetPath = Get-Content "local-nuget-path.txt"
  copy nuget\TypeGen.2.3.1.nupkg $localNuGetPath
}


#nuget - dotnetcli


rm -Recurse -Force nuget-dotnetcli\tools\netcoreapp2.1\any\runtimes
copy -Recurse src\TypeGen\TypeGen.Cli\bin\Release\netcoreapp2.1\publish\* nuget-dotnetcli\tools\netcoreapp2.1\any

nuget pack nuget-dotnetcli\TypeGen.DotNetCli.nuspec
move TypeGen.DotNetCli.2.3.1.nupkg nuget-dotnetcli -force

if (Test-Path "local-nuget-path.txt") {
  $localNuGetPath = Get-Content "local-nuget-path.txt"
  copy nuget-dotnetcli\TypeGen.DotNetCli.2.3.1.nupkg $localNuGetPath
}
