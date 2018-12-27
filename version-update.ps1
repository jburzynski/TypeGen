if (-not ($args.length -eq 2)) {
  Write-Host "Usage: ./version-update [old-version] [new-version]"
  exit
}

$versionRegex = "\d+\.\d+\.\d+"

$oldVersion = $args[0]
$newVersion = $args[1]

if (-not ($oldVersion -match $versionRegex) -or  -not ($newVersion -match $versionRegex)) {
  Write-Host "Wrong version format. Should be: $($versionRegex)"
  exit
}

# replace files' contents

$nuspecPath = "nuget\TypeGen.nuspec" 
if (Test-Path $nuspecPath) {
  (Get-Content $nuspecPath).Replace("<version>$($oldVersion)</version>", "<version>$($newVersion)</version>") | Set-Content $nuspecPath
}

$dotNetCliNuspecPath = "nuget-dotnetcli\TypeGen.DotNetCli.nuspec"
if (Test-Path $dotNetCliNuspecPath) {
  (Get-Content $dotNetCliNuspecPath).Replace("<version>$($oldVersion)</version>", "<version>$($newVersion)</version>") | Set-Content $dotNetCliNuspecPath
  #.Replace("id=""TypeGen"" version=""$($oldVersion)""", "id=""TypeGen"" version=""$($newVersion)""")
}

$docsConfPath = "..\TypeGenDocs\source\conf.py"
if (Test-Path $docsConfPath) {
  (Get-Content $docsConfPath).Replace("version = u'$($oldVersion)'", "version = u'$($newVersion)'") | Set-Content $docsConfPath
}

$appConfigPath = "src\TypeGen\TypeGen.Cli\AppConfig.cs"
if (Test-Path $appConfigPath) {
  (Get-Content $appConfigPath).Replace("Version => ""$($oldVersion)""", "Version => ""$($newVersion)""") | Set-Content $appConfigPath
}

$nugetUpdatePath = "nuget-update.ps1"
if (Test-Path $nugetUpdatePath) {
  (Get-Content $nugetUpdatePath).Replace("TypeGen.$($oldVersion)", "TypeGen.$($newVersion)").Replace("TypeGen.DotNetCli.$($oldVersion)", "TypeGen.DotNetCli.$($newVersion)") | Set-Content $nugetUpdatePath
}

# remove old NuGet package

$oldNupkgPath = "nuget\TypeGen.$($oldVersion).nupkg"
if (Test-Path $oldNupkgPath) {
  rm $oldNupkgPath
}

$oldDotNetCliNupkgPath = "nuget-dotnetcli\TypeGen.DotNetCli.$($oldVersion).nupkg"
if (Test-Path $oldDotNetCliNupkgPath) {
  rm $oldDotNetCliNupkgPath
}
