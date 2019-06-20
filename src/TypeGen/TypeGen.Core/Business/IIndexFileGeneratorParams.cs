using System;
using System.Collections.Generic;
using System.Text;

namespace TypeGen.Core.Business
{
    public interface IIndexFileGeneratorParams
    {
        IEnumerable<string> GeneratedFiles { get; }
        GeneratorOptions GeneratorOptions { get; }
        string FillIndexTemplate(string exports);
        string FillIndexExportTemplate(string filename);
        EventHandler<FileContentGeneratedArgs> FileContentGenerated { get; }
    }
}
