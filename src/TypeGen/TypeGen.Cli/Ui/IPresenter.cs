using System.Collections.Generic;

namespace TypeGen.Cli.Ui;

internal interface IPresenter
{
    ActionResult GetCwd();
    ActionResult Generate(bool verbose, IReadOnlyCollection<string> projectFolderPaths, IReadOnlyCollection<string> configPaths, string outputFolder);
}