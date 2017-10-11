using System;
using System.Collections.Generic;
using System.Text;
using NuGet.Configuration;

namespace TypeGen.Cli
{
    internal class CliSettings
    {
        static CliSettings()
        {
            ISettings settings = Settings.LoadDefaultSettings(null);
            GlobalPackagesPath = SettingsUtility.GetGlobalPackagesFolder(settings);
        }

        public static string GlobalPackagesPath { get; private set; }
    }
}
