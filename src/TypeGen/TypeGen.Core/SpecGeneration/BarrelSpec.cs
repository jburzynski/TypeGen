namespace TypeGen.Core.SpecGeneration
{
    internal class BarrelSpec
    {
        public string Directory { get; }
        public BarrelScope BarrelScope { get; }

        public BarrelSpec(string directory, BarrelScope barrelScope)
        {
            Directory = directory;
            BarrelScope = barrelScope;
        }
    }
}