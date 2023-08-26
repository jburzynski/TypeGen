if ($args.Length -eq 0)
{
  Write-Host "examples:
./update-version 2.4.9 # changes the current version to 2.4.9"
  exit
}

$versionRegex = "^\d+\.\d+\.\d+$"
$newVersion = $args[0]

if (-not ($newVersion -match $versionRegex)) {
  Write-Host "Wrong version format. Should be: $($versionRegex)"
  exit
}

$oldVersion = (Select-Xml //version "nuget\TypeGen.nuspec")[0].Node.InnerText
$oldVersionMajor = $oldVersion.Split(".")[0]
$oldVersionMinor = $oldVersion.Split(".")[1]

$newVersionMajor = $newVersion.Split(".")[0]
$newVersionMinor = $newVersion.Split(".")[1]

$assemblyOldVersion = "$($oldVersionMajor).$($oldVersionMinor).0.0"
$assemblyNewVersion = "$($newVersionMajor).$($newVersionMinor).0.0"

# replace files' contents

$nuspecPath = "nuget\TypeGen.nuspec" 
if (Test-Path $nuspecPath) {
  (Get-Content $nuspecPath).Replace("<version>$($oldVersion)</version>", "<version>$($newVersion)</version>") | Set-Content $nuspecPath
}

$dotNetCliNuspecPath = "nuget-dotnetcli\dotnet-typegen.nuspec"
if (Test-Path $dotNetCliNuspecPath) {
  (Get-Content $dotNetCliNuspecPath).Replace("<version>$($oldVersion)</version>", "<version>$($newVersion)</version>") | Set-Content $dotNetCliNuspecPath
  #.Replace("id=""TypeGen"" version=""$($oldVersion)""", "id=""TypeGen"" version=""$($newVersion)""")
}

#$docsConfPath = "..\TypeGenDocs\source\conf.py"
#if (Test-Path $docsConfPath) {
#  (Get-Content $docsConfPath).Replace("version = u'$($oldVersion)'", "version = u'$($newVersion)'") | Set-Content $docsConfPath
#}

$appConfigPath = "src\TypeGen\TypeGen.Cli\ApplicationConfig.cs"
if (Test-Path $appConfigPath) {
  (Get-Content $appConfigPath).Replace("Version => ""$($oldVersion)""", "Version => ""$($newVersion)""") | Set-Content $appConfigPath
}

$nugetUpdatePath = "nuget-update.ps1"
if (Test-Path $nugetUpdatePath) {
  (Get-Content $nugetUpdatePath).Replace("TypeGen.$($oldVersion)", "TypeGen.$($newVersion)").Replace("dotnet-typegen.$($oldVersion)", "dotnet-typegen.$($newVersion)") | Set-Content $nugetUpdatePath
}

$typeGenCliCsprojPath = "src\TypeGen\TypeGen.Cli\TypeGen.Cli.csproj"
if (Test-Path $typeGenCliCsprojPath) {
	(Get-Content $typeGenCliCsprojPath).Replace("<AssemblyVersion>$($assemblyOldVersion)</AssemblyVersion>", "<AssemblyVersion>$($assemblyNewVersion)</AssemblyVersion>").Replace("<FileVersion>$($assemblyOldVersion)</FileVersion>", "<FileVersion>$($assemblyNewVersion)</FileVersion>") | Set-Content $typeGenCliCsprojPath
}

$typeGenCoreCsprojPath = "src\TypeGen\TypeGen.Core\TypeGen.Core.csproj"
if (Test-Path $typeGenCoreCsprojPath) {
	(Get-Content $typeGenCoreCsprojPath).Replace("<AssemblyVersion>$($assemblyOldVersion)</AssemblyVersion>", "<AssemblyVersion>$($assemblyNewVersion)</AssemblyVersion>").Replace("<FileVersion>$($assemblyOldVersion)</FileVersion>", "<FileVersion>$($assemblyNewVersion)</FileVersion>") | Set-Content $typeGenCoreCsprojPath
}

# remove old NuGet package

$oldNupkgPath = "nuget\TypeGen.$($oldVersion).nupkg"
if (Test-Path $oldNupkgPath) {
  rm $oldNupkgPath
}

$oldDotNetCliNupkgPath = "nuget-dotnetcli\dotnet-typegen.$($oldVersion).nupkg"
if (Test-Path $oldDotNetCliNupkgPath) {
  rm $oldDotNetCliNupkgPath
}
