using System.Collections;
using System.Collections.Generic;
using TypeGen.Core.Generator;

namespace TypeGen.Core.SpecGeneration
{
    public class OnBeforeBarrelGenerationArgs
    {
        public GeneratorOptions GeneratorOptions { get; }
        public IEnumerable<string> GeneratedFiles { get; }

        public OnBeforeBarrelGenerationArgs(GeneratorOptions generatorOptions, IEnumerable<string> generatedFiles)
        {
            GeneratorOptions = generatorOptions;
            GeneratedFiles = generatedFiles;
        }
    }
}