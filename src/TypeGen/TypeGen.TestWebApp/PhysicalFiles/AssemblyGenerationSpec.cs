using TypeGen.Core.SpecGeneration;

namespace TypeGen.TestWebApp.PhysicalFiles
{
    public class AssemblyGenerationSpec : GenerationSpec
    {
        public AssemblyGenerationSpec()
        {
            ForAssembly(GetType().Assembly)
                .AddClasses(@"asdfasdf")
                .AddEnums(@"TypeGen\.TestWebApp\.PhysicalFiles\.(.+)")
                .AddInterfaces("asdfasdf");
        }
    }
}