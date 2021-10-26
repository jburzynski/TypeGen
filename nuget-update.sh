#!/bin/bash
#nuget

#lib

#net5.0
cp src/TypeGen/TypeGen.Core/bin/Release/net5.0/TypeGen.Core.dll nuget/lib/net5.0
cp src/TypeGen/TypeGen.Core/bin/Release/net5.0/TypeGen.Core.xml nuget/lib/net5.0

dotnet pack nuget/TypeGen.nuspec
mv TypeGen.2.4.9.1.nupkg nuget 