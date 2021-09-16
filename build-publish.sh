#!/bin/bash

dotnet clean ./src/TypeGen/TypeGen.Core
dotnet restore ./src/TypeGen/TypeGen.Core
dotnet build ./src/TypeGen/TypeGen.Core -f net5.0 -c Release

dotnet clean ./src/TypeGen/TypeGen.Cli
dotnet restore ./src/TypeGen/TypeGen.Cli
dotnet publish ./src/TypeGen/TypeGen.Cli -c Release -f net5.0