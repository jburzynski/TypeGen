using TypeGen.Core.Generator;

namespace TypeGen.Core.SpecGeneration
{
    public class OnBeforeGenerationArgs
    {
        public GeneratorOptions GeneratorOptions { get; }
        
        public OnBeforeGenerationArgs(GeneratorOptions generatorOptions)
        {
            GeneratorOptions = generatorOptions;
        }
    }
}