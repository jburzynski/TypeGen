using System.Collections.Generic;
using TypeGen.Core.Extensions;

namespace TypeGen.Cli.Ui.Validation;

internal class GenerateValidator : IGenerateValidator
{
    public ValidationResult Validate(bool verbose, IReadOnlyCollection<string> projectFolderPaths, IReadOnlyCollection<string> configPaths, string outputFolder)
    {
        var messages = new List<string>();

        if (configPaths.IsNotNullAndNotEmpty())
        {
            if (projectFolderPaths.Count != configPaths.Count)
                messages.Add("The number of project folders and config paths must be the same.");
        }

        return new ValidationResult(messages);
    }
}