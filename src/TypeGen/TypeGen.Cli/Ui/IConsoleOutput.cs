using System.Collections.Generic;

namespace TypeGen.Cli.Ui;

internal interface IConsoleOutput
{
    void ShowCwd();
    void ShowGenerationBegin(string projectFolder);
    void ShowGeneratedFiles(List<string> generatedFiles);
    void ShowGenerationEnd(string projectFolder);
    void ShowErrors(IEnumerable<string> messages);
}