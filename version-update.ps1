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

$paths = #"nuget-update.ps1",
         "nuget\TypeGen.nuspec",
         #"src\TypeGen\TypeGen.Cli\AppConfig.cs",
         "src\TypeGen\TypeGen.Cli\TypeGen.Cli.csproj",
         "..\TypeGenDocs\source\conf.py"

foreach ($path in $paths) {
  if (Test-Path $path) {
    (Get-Content $path).Replace($oldVersion, $newVersion) | Set-Content $path
  }
}

if (Test-Path "src\TypeGen\TypeGen.Cli\AppConfig.cs") {
  (Get-Content "src\TypeGen\TypeGen.Cli\AppConfig.cs".Replace("CoreVersion => $($oldVersion)", "CoreVersion => $($newVersion)") | Set-Content "src\TypeGen\TypeGen.Cli\AppConfig.cs"
}

if (Test-Path "nuget-update.ps1") {
  (Get-Content "nuget-update.ps1".Replace("TypeGen.$($oldVersion)", "TypeGen.$($newVersion)") | Set-Content "nuget-update.ps1"
}

if (Test-Path "nuget-dotnetcli\TypeGen.DotNetCli.nuspec") {
  (Get-Content "nuget-dotnetcli\TypeGen.DotNetCli.nuspec".Replace("id=""TypeGen"" version=""$($oldVersion)""", "id=""TypeGen"" version=""$($newVersion)""") | Set-Content "nuget-dotnetcli\TypeGen.DotNetCli.nuspec"
}

# remove old NuGet package

if (Test-Path "nuget\TypeGen.$($oldVersion).nupkg") {
  rm "nuget\TypeGen.$($oldVersion).nupkg"
}
