using System;
using System.Diagnostics;
using TypeGen.Core.Logging;

namespace TypeGen.Cli.Build
{
    internal class ProjectBuild
    {
        private readonly ILogger _logger;

        public ProjectBuild(ILogger logger)
        {
            _logger = logger;
        }

        public void Build(string projectFolder)
        {
            _logger.Log($"Building project '{projectFolder}'...", LogLevel.Info);
            
            // Create the process info
            var startInfo = new ProcessStartInfo("dotnet", "build")
            {
                WorkingDirectory = projectFolder,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            // Execute the process
            using (Process process = Process.Start(startInfo))
            {
                process.OutputDataReceived += (_, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                    {
                        Console.WriteLine(e.Data);
                    }
                };
                process.BeginOutputReadLine();
                process.WaitForExit();
            }
            
            _logger.Log($"Finished building project '{projectFolder}'", LogLevel.Info);
        }
    }
}