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

$paths = "nuget-update.ps1",
         "nuget\TypeGen.nuspec",
		 "chocolatey\TypeGen.nuspec",
         "src\TypeGen\TypeGen.Cli\AppConfig.cs",
         "..\TypeGenDocs\source\conf.py"

foreach ($path in $paths) {
  if (Test-Path $path) {
    (Get-Content $path).Replace($oldVersion, $newVersion) | Set-Content $path
  }
}

# remove old NuGet and chocolatey package

if (Test-Path "nuget\TypeGen.$($oldVersion).nupkg") {
  rm "nuget\TypeGen.$($oldVersion).nupkg"
}

if (Test-Path "chocolatey\TypeGen.$($oldVersion).nupkg") {
  rm "chocolatey\TypeGen.$($oldVersion).nupkg"
}