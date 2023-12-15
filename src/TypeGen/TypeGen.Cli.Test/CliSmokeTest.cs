using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace TypeGen.Cli.Test
{
    public class CliSmokeTest
    {
        private readonly ITestOutputHelper logger;

        public CliSmokeTest(ITestOutputHelper output) => this.logger = output;

        [Fact]
        public void Cli_should_finish_with_success()
        {
            // arrange
            
            const string projectToGeneratePath = "../../../../TypeGen.FileContentTest";
            const string cliFileName = "TypeGen.Cli.exe";
            string[] cliPossibleDirectories = {
                "../../../../TypeGen.Cli/bin/Debug/net8.0",            
                "../../../../TypeGen.Cli/bin/Release/net8.0",            
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
                var line = process.StandardOutput.ReadLine();
                logger.WriteLine(line);
                outputBuilder.AppendLine(line);
            }

            var output = outputBuilder.ToString().TrimEnd();
            var lastOutputLine = output.Split(Environment.NewLine).Last();

            lastOutputLine.ToUpperInvariant().Should().Contain("success".ToUpperInvariant());
        }

        private string GetCliDirectory(IEnumerable<string> possibleDirectories)
            => possibleDirectories.FirstOrDefault(Directory.Exists);
    }
}