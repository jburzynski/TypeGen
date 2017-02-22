msbuild .\src\TypeGen /t:clean /verbosity:minimal
if ($lastExitCode -ne 0) { exit $lastExitCode }

msbuild .\src\TypeGen /t:restore /verbosity:minimal
if ($lastExitCode -ne 0) { exit $lastExitCode }

msbuild .\src\TypeGen /p:Configuration=Release /verbosity:minimal
if ($lastExitCode -ne 0) { exit $lastExitCode }