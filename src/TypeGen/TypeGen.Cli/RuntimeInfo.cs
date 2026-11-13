using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;

namespace TypeGen.Cli;

internal class RuntimeInfo
{
    public static readonly NuGetFramework CliFramework = NuGetFramework.Parse(Assembly.GetExecutingAssembly().GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName);
}
