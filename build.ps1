# solution-wide

msbuild .\src\TypeGen /t:clean /verbosity:minimal
if ($lastExitCode -ne 0) { exit $lastExitCode }

msbuild .\src\TypeGen /t:restore /p:TargetFramework=net46 /verbosity:minimal
if ($lastExitCode -ne 0) { exit $lastExitCode }

msbuild .\src\TypeGen /p:TargetFramework=net46 /p:Configuration=Release /verbosity:minimal
if ($lastExitCode -ne 0) { exit $lastExitCode }

# restore

msbuild .\src\TypeGen\TypeGen.Cli\TypeGen.Cli.csproj /t:restore /p:TargetFramework=net46 /verbosity:minimal
if ($lastExitCode -ne 0) { exit $lastExitCode }

msbuild .\src\TypeGen\TypeGen.Core\TypeGen.Core.csproj /t:restore /p:TargetFramework=netstandard1.3 /verbosity:minimal
if ($lastExitCode -ne 0) { exit $lastExitCode }

msbuild .\src\TypeGen\TypeGen.Test\TypeGen.Test.csproj /t:restore /p:TargetFramework=net46 /verbosity:minimal
if ($lastExitCode -ne 0) { exit $lastExitCode }

# build

msbuild .\src\TypeGen\TypeGen.Cli\TypeGen.Cli.csproj /p:TargetFramework=net46 /p:Configuration=Release /verbosity:minimal
if ($lastExitCode -ne 0) { exit $lastExitCode }

msbuild .\src\TypeGen\TypeGen.Core\TypeGen.Core.csproj /p:TargetFramework=netstandard1.3 /p:Configuration=Release /verbosity:minimal
if ($lastExitCode -ne 0) { exit $lastExitCode }

msbuild .\src\TypeGen\TypeGen.Test\TypeGen.Test.csproj /p:TargetFramework=net46 /p:Configuration=Release /verbosity:minimal
if ($lastExitCode -ne 0) { exit $lastExitCode }