using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;

namespace TypeGen.IntegrationTest
{
    public class CliSmokeTest
    {
        [Fact]
        public void Cli_should_finish_with_success()
        {
            // arrange
            
            const string projectToGeneratePath = "../../../../TypeGen.TestWebApp";
            const string cliFileName = "TypeGen.Cli.exe";
            string[] cliPossibleDirectories = {
                "../../../../TypeGen.Cli/bin/Debug/net7.0",            
                "../../../../TypeGen.Cli/bin/Release/net7.0",            
            };
            
            var cliFilePath = GetCliDirectory(cliPossibleDirectories);
            cliFilePath = Path.Combine(cliFilePath, cliFileName);
            
            var process = new Process 
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = cliFilePath,
                    Arguments = $"generate -p {projectToGeneratePath}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            
            // act
            
            process.Start();
            
            // assert

            var outputBuilder = new StringBuilder();
            while (!process.StandardOutput.EndOfStream)
            {
                outputBuilder.AppendLine(process.StandardOutput.ReadLine());
            }

            var output = outputBuilder.ToString().TrimEnd();
            var lastOutputLine = output.Split(Environment.NewLine).Last();

            lastOutputLine.ToUpperInvariant().Should().Contain("success".ToUpperInvariant());
        }

        private string GetCliDirectory(IEnumerable<string> possibleDirectories)
            => possibleDirectories.FirstOrDefault(Directory.Exists);
    }
}