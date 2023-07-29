using System.Collections.Generic;
using TypeGen.Core.Generator;

namespace TypeGen.Core.SpecGeneration;

public class OnAfterGenerationArgs
{
    public GeneratorOptions GeneratorOptions { get; }
    public IEnumerable<string> GeneratedFiles { get; }

    public OnAfterGenerationArgs(GeneratorOptions generatorOptions, IEnumerable<string> generatedFiles)
    {
        GeneratorOptions = generatorOptions;
        GeneratedFiles = generatedFiles;
    }
}