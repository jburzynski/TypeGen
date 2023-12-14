using System;
using System.Collections.Generic;
using TypeGen.Core.Storage;
using static TypeGen.Core.Utils.ConsoleUtils;

namespace TypeGen.Cli.Ui;

internal class ConsoleOutput : IConsoleOutput
{
    private readonly IFileSystem _fileSystem;

    public ConsoleOutput(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public void ShowCwd()
    {
        var cwd = _fileSystem.GetCurrentDirectory();
        Console.WriteLine($"Current working directory is: {cwd}");
    }

    public void ShowGenerationBegin(string projectFolder)
    {
        Console.WriteLine($"Generating files for project \"{projectFolder}\"...");
    }

    public void ShowGeneratedFiles(List<string> generatedFiles)
    {
        foreach (var file in generatedFiles)
            Console.WriteLine($"Generated {file}");
    }

    public void ShowGenerationEnd(string projectFolder)
    {
        Console.WriteLine($"Files for project \"{projectFolder}\" generated successfully.{Environment.NewLine}");
    }

    public void ShowErrors(IEnumerable<string> messages)
    {
        WithColor(ConsoleColor.Red, () =>
        {
            foreach (var message in messages) Console.WriteLine(message);
        });
    }
}