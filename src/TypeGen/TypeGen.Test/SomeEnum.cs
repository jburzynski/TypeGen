using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Test
{
    [ExportTsEnum(OutputDir = "./my/enums/.././enums/project")]
    public enum SomeEnum
    {
        ENUM_VALUE = 1,
        ANOTHER_VALUE = 2
    }
}
