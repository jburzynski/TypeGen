using System.IO;
using TypeGen.Core.SpecGeneration;

namespace TypeGen.TestWebApp.GenerationSpecs
{
    public class TestGenerationSpec : GenerationSpec
    {
        public override void OnBeforeBarrelGeneration(OnBeforeBarrelGenerationArgs args)
        {
            string[] subdirectoryEntries = Directory.GetDirectories(args.GeneratorOptions.BaseOutputDirectory);
            var a = 0;
        }
    }
}