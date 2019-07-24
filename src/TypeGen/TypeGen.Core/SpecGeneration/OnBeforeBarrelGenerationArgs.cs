using TypeGen.Core.Generator;

namespace TypeGen.Core.SpecGeneration
{
    public class OnBeforeBarrelGenerationArgs
    {
        public OnBeforeBarrelGenerationArgs(GeneratorOptions generatorOptions)
        {
            GeneratorOptions = generatorOptions;
        }

        public GeneratorOptions GeneratorOptions { get; }
    }
}