using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeGen.Cli;

internal static class Constants
{
    public static NuGetFramework CliFramework = NuGetFramework.Parse("net11.0");
}
