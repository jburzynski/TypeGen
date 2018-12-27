namespace TypeGen.Core.Extensions
{
    internal static class EnumExtensions
    {
        public static bool IsGenerationSpecGenerationType(this GenerationType generationType)
        {
            return generationType == GenerationType.GenerationSpecType || generationType == GenerationType.GenerationSpecAssembly;
        }
    }
}