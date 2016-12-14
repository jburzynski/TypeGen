using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Test.TestEntities
{
    [ExportTsEnum(OutputDir = "test-enums")]
    internal enum TestEnum
    {
        A
    }
}