#nuget

#tools

rm -Recurse -Force nuget\tools\runtimes
copy -Recurse src\TypeGen\TypeGen.Cli\bin\Release\PublishOutput\* nuget\tools
rm nuget\tools\TypeGen.Cli.pdb
rm nuget\tools\TypeGen.Core.pdb
rm nuget\tools\TypeGen.Core.xml

#lib

#netstandard1.3
copy src\TypeGen\TypeGen.Core\bin\Release\netstandard1.3\TypeGen.Core.dll nuget\lib\netstandard1.3
copy src\TypeGen\TypeGen.Core\bin\Release\netstandard1.3\TypeGen.Core.xml nuget\lib\netstandard1.3

#netstandard2.0
copy src\TypeGen\TypeGen.Core\bin\Release\netstandard2.0\TypeGen.Core.dll nuget\lib\netstandard2.0
copy src\TypeGen\TypeGen.Core\bin\Release\netstandard2.0\TypeGen.Core.xml nuget\lib\netstandard2.0

nuget pack nuget\TypeGen.nuspec
move TypeGen.1.5.11.nupkg nuget -force

if (Test-Path "local-nuget-path.txt") {
  $localNuGetPath = Get-Content "local-nuget-path.txt"
  copy nuget\TypeGen.1.5.11.nupkg $localNuGetPath
}


#chocolatey

#rm -Recurse -Force chocolatey\runtimes
#copy -Recurse src\TypeGen\TypeGen.Cli\bin\Release\PublishOutput\* chocolatey
#rm chocolatey\TypeGen.Cli.pdb
#rm chocolatey\TypeGen.Core.pdb
#rm chocolatey\TypeGen.Core.xml

#nuget pack chocolatey\TypeGen.nuspec
#move TypeGen.1.5.11.nupkg chocolatey -force

#if (Test-Path "local-chocolatey-path.txt") {
#  $localChocolateyPath = Get-Content "local-chocolatey-path.txt"
#  #copy chocolatey\TypeGen.1.5.11.nupkg $localChocolateyPath
#}
