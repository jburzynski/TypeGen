using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Test
{
    [TsEnum(OutputDir = "./my/enums/.././enums/project")]
    public enum SomeEnum
    {
        EnumValue = 1,
        AnotherValue = 2
    }
}
