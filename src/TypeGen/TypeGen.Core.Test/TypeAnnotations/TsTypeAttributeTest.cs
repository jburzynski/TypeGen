using TypeGen.Core.TypeAnnotations;
using Xunit;

namespace TypeGen.Core.Test.TypeAnnotations
{
    public class TsTypeAttributeTest
    {
        [Theory]
        [InlineData(TsType.Any, "any")]
        [InlineData(TsType.Boolean, "boolean")]
        [InlineData(TsType.Date, "Date")]
        [InlineData(TsType.Number, "number")]
        [InlineData(TsType.Object, "Object")]
        [InlineData(TsType.String, "string")]
        public void Constructor_TsTypeGiven_MapsToCorrectTypeName(TsType type, string expectedTypeName)
        {
            var attribute = new TsTypeAttribute(type);
            Assert.Equal(expectedTypeName, attribute.TypeName);
        }

        [Theory]
        [InlineData("MyClass<string>", "MyClass")]
        [InlineData("MyClass<string, int>", "MyClass")]
        [InlineData("MyClass<string, int, bool>", "MyClass")]
        [InlineData("MyClass[]", "MyClass")]
        [InlineData("MyClass[][]", "MyClass")]
        [InlineData("MyClass[][][][]", "MyClass")]
        public void FlatTypeName_TypeNameGiven_ReturnsCorrectFlatTypeName(string originalTypeName, string expectedFlatTypeName)
        {
            var attribute = new TsTypeAttribute(originalTypeName);
            Assert.Equal(expectedFlatTypeName, attribute.FlatTypeName);
        }
    }
}
