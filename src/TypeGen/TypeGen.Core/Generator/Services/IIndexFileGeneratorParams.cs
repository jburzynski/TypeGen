using System;
using System.Collections.Generic;

namespace TypeGen.Core.Generator.Services
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
