if ($args.Length -eq 0)
{
  Write-Host "examples:
./update-version increment # increments the minor version, i.e. the middle part of the version
./update-version 2.4.9 # changes the current version to 2.4.9"
  exit
}

$versionRegex = "^\d+\.\d+\.\d+$"

$oldVersion = (Select-Xml //version "nuget\TypeGen.nuspec")[0].Node.InnerText

$oldVersionMajor = $oldVersion.Split(".")[0]
$oldVersionMinor = $oldVersion.Split(".")[1]
$newVersionMinor = ($oldVersionMinor -as [int]) + 1

$newVersion = if ($args -contains "increment") {"$($oldVersionMajor).$($newVersionMinor).0"} else {$args[0]}

if (-not ($newVersion -match $versionRegex)) {
  Write-Host "Wrong version format. Should be: $($versionRegex)"
  exit
}

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

$appConfigPath = "src\TypeGen\TypeGen.Cli\AppConfig.cs"
if (Test-Path $appConfigPath) {
  (Get-Content $appConfigPath).Replace("Version => ""$($oldVersion)""", "Version => ""$($newVersion)""") | Set-Content $appConfigPath
}

$nugetUpdatePath = "nuget-update.ps1"
if (Test-Path $nugetUpdatePath) {
  (Get-Content $nugetUpdatePath).Replace("TypeGen.$($oldVersion)", "TypeGen.$($newVersion)").Replace("dotnet-typegen.$($oldVersion)", "dotnet-typegen.$($newVersion)") | Set-Content $nugetUpdatePath
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
