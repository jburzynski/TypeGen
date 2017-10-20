#get TypeGen.Cli.dll path
$scriptPath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptPath
$dir = Resolve-Path $dir
$cliPath = Join-Path $dir "TypeGen.Cli.dll"

#get arguments
$arguments = $MyInvocation.Line.Remove(0, $MyInvocation.InvocationName.Length)

#invoke
Invoke-Expression "dotnet ""$($cliPath)""$($arguments)"