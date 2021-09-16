#!/bin/bash
#nuget

#tools

# rm nuget\tools\TypeGen.exe
# rm -Recurse -Force nuget\tools\runtimes
# copy -Recurse src\TypeGen\TypeGen.Cli\bin\Release\netcoreapp3.1\publish\* nuget\tools
# mv nuget\tools\TypeGen.Cli.exe nuget\tools\TypeGen.exe
# 


#lib

#net5.0
cp src/TypeGen/TypeGen.Core/bin/Release/net5.0/TypeGen.Core.dll nuget/lib/net5.0
cp src/TypeGen/TypeGen.Core/bin/Release/net5.0/TypeGen.Core.xml nuget/lib/net5.0

dotnet pack nuget/TypeGen.nuspec
mv TypeGen.2.4.9.nupkg nuget 

#cp nuget/TypeGen.2.4.9.1.nupkg $localNuGetPath


#nuget - dotnetcli


# rm -Recurse -Force nuget-dotnetcli\tools\netcoreapp2.1\any\runtimes
# copy -Recurse src\TypeGen\TypeGen.Cli\bin\Release\netcoreapp2.1\publish\* nuget-dotnetcli\tools\netcoreapp2.1\any
# 
# rm -Recurse -Force nuget-dotnetcli\tools\netcoreapp2.2\any\runtimes
# copy -Recurse src\TypeGen\TypeGen.Cli\bin\Release\netcoreapp2.2\publish\* nuget-dotnetcli\tools\netcoreapp2.2\any
# 
# rm -Recurse -Force nuget-dotnetcli\tools\netcoreapp3.0\any\runtimes
# copy -Recurse src\TypeGen\TypeGen.Cli\bin\Release\netcoreapp3.0\publish\* nuget-dotnetcli\tools\netcoreapp3.0\any
# 
# rm -Recurse -Force nuget-dotnetcli\tools\netcoreapp3.1\any\runtimes
# copy -Recurse src\TypeGen\TypeGen.Cli\bin\Release\netcoreapp3.1\publish\* nuget-dotnetcli\tools\netcoreapp3.1\any
# 
# nuget pack nuget-dotnetcli\dotnet-typegen.nuspec
# move dotnet-typegen.2.4.9.nupkg nuget-dotnetcli -force
# 
# if (Test-Path "local-nuget-path.txt") {
#   $localNuGetPath = Get-Content "local-nuget-path.txt"
#   copy nuget-dotnetcli\dotnet-typegen.2.4.9.nupkg $localNuGetPath
# }
