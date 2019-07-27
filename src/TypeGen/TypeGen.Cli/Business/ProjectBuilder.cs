using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TypeGen.Core.Logging;
using TypeGen.Core.Storage;

namespace TypeGen.Cli.Business
{
    internal class ProjectBuilder
    {
        private readonly ILogger _logger;

        public ProjectBuilder(ILogger logger)
        {
            _logger = logger;
        }

        public void Build(string projectFolder)
        {
            _logger.Log($"Building project '{projectFolder}'...");
            
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
                        _logger.Log(e.Data);
                    }
                };
                process.BeginOutputReadLine();
                process.WaitForExit();
            }
            
            _logger.Log($"Finished building project '{projectFolder}'");
        }
    }
}