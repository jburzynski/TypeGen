if (-not ($args.length -eq 2)) {
  Write-Host "Usage: ./version-update-dotnetcli [old-version] [new-version]"
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

$path = #"nuget-update.ps1",
         "nuget-dotnetcli\TypeGen.DotNetCli.nuspec"
         #"src\TypeGen\TypeGen.Cli\AppConfig.cs",
         #"..\TypeGenDocs\source\conf.py"

#foreach ($path in $paths) {
  if (Test-Path $path) {
    (Get-Content $path).Replace($oldVersion, $newVersion) | Set-Content $path
  }
#}

if (Test-Path "nuget-update.ps1") {
  (Get-Content "nuget-update.ps1".Replace("TypeGen.DotNetCli.$($oldVersion)", "TypeGen.DotNetCli.$($newVersion)") | Set-Content "nuget-update.ps1"
}

if (Test-Path "src\TypeGen\TypeGen.Cli\AppConfig.cs") {
  (Get-Content "src\TypeGen\TypeGen.Cli\AppConfig.cs".Replace("CliVersion => $($oldVersion)", "CliVersion => $($newVersion)") | Set-Content "src\TypeGen\TypeGen.Cli\AppConfig.cs"
}

# remove old NuGet package

if (Test-Path "nuget-dotnetcli\TypeGen.DotNetCli.$($oldVersion).nupkg") {
  rm "nuget-dotnetcli\TypeGen.DotNetCli.$($oldVersion).nupkg"
}
