# builds/publishes projects and creates NuGet packages
#
# usage: ./publish [-nobuild]
# -nobuild: does NOT build TypeGen.Core - useful for build servers so that TypeGen.Core is not built twice

if (-not $args -contains "-nobuild")
{
  # build TypeGen.Core
  dotnet clean .\src\TypeGen\TypeGen.Core
  dotnet restore .\src\TypeGen\TypeGen.Core
  dotnet build .\src\TypeGen\TypeGen.Core -f netstandard2.0 -c Release
  dotnet build .\src\TypeGen\TypeGen.Core -f net9.0 -c Release
}


# publish TypeGen Cli
dotnet clean .\src\TypeGen\TypeGen.Cli
dotnet restore .\src\TypeGen\TypeGen.Cli
dotnet publish .\src\TypeGen\TypeGen.Cli -c Release -f net9.0


# create TypeGen NuGet package

#tools

if (test-path nuget\tools)
{
  rm -Recurse -Force nuget\tools
}

new-item -Force -Path nuget\tools -ItemType Directory
$binFolder = if (test-path "src\TypeGen\TypeGen.Cli\bin\Any CPU\Release\net9.0\publish") {"bin\Any CPU"} else {"bin"}
copy -Recurse "src\TypeGen\TypeGen.Cli\$($binFolder)\Release\net9.0\publish\*" nuget\tools
mv nuget\tools\TypeGen.Cli.exe nuget\tools\TypeGen.exe


#lib

if (test-path nuget\lib)
{
  rm -Recurse -Force nuget\lib
}

#netstandard2.0
new-item -Force -Path nuget\lib\netstandard2.0 -ItemType Directory
$binFolder = if (test-path "src\TypeGen\TypeGen.Core\bin\Any CPU\Release\netstandard2.0") {"bin\Any CPU"} else {"bin"}
copy "src\TypeGen\TypeGen.Core\$($binFolder)\Release\netstandard2.0\TypeGen.Core.dll" nuget\lib\netstandard2.0
copy "src\TypeGen\TypeGen.Core\$($binFolder)\Release\netstandard2.0\TypeGen.Core.xml" nuget\lib\netstandard2.0

#net9.0
new-item -Force -Path nuget\lib\net9.0 -ItemType Directory
$binFolder = if (test-path "src\TypeGen\TypeGen.Core\bin\Any CPU\Release\net9.0") {"bin\Any CPU"} else {"bin"}
copy "src\TypeGen\TypeGen.Core\$($binFolder)\Release\net9.0\TypeGen.Core.dll" nuget\lib\net9.0
copy "src\TypeGen\TypeGen.Core\$($binFolder)\Release\net9.0\TypeGen.Core.xml" nuget\lib\net9.0

nuget pack nuget\TypeGen.nuspec


# create dotnet-typegen NuGet package

if (test-path nuget-dotnetcli\tools)
{
  rm -Recurse -Force nuget-dotnetcli\tools
}

new-item -Force -Path nuget-dotnetcli\tools\net9.0\any -ItemType Directory
$binFolder = if (test-path "src\TypeGen\TypeGen.Cli\bin\Any CPU\Release\net9.0\publish") {"bin\Any CPU"} else {"bin"}
copy -Recurse "src\TypeGen\TypeGen.Cli\$($binFolder)\Release\net9.0\publish\*" nuget-dotnetcli\tools\net9.0\any

New-Item nuget-dotnetcli\tools\net9.0\any\DotnetToolSettings.xml
set-content nuget-dotnetcli\tools\net9.0\any\DotnetToolSettings.xml '<?xml version="1.0" encoding="utf-8"?>
<DotNetCliTool Version="1">
  <Commands>
    <Command Name="dotnet-typegen" EntryPoint="TypeGen.Cli.dll" Runner="dotnet" />
  </Commands>
</DotNetCliTool>'

nuget pack nuget-dotnetcli\dotnet-typegen.nuspec


# cleanup

rm -Recurse -Force nuget\tools
rm -Recurse -Force nuget\lib
rm -Recurse -Force nuget-dotnetcli\tools
