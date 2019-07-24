using TypeGen.Core.Generator;

namespace TypeGen.Core.SpecGeneration
{
    public class OnBeforeGenerationArgs
    {
        public OnBeforeGenerationArgs(GeneratorOptions generatorOptions)
        {
            GeneratorOptions = generatorOptions;
        }

        public GeneratorOptions GeneratorOptions { get; }
    }
}