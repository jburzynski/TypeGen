using System.Collections.Generic;

namespace TypeGen.Cli.Ui.Validation;

internal interface IGenerateValidator
{
    ValidationResult Validate(bool verbose, IReadOnlyCollection<string> projectFolderPaths, IReadOnlyCollection<string> configPaths, string outputFolder);
}