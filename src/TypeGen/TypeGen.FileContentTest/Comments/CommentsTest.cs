using System;
using System.Threading.Tasks;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.FileContentTest.Comments.Entities;
using TypeGen.FileContentTest.TestingUtils;
using Xunit;

namespace TypeGen.FileContentTest.Comments;

public class CommentsTest : GenerationTestBase
{
    [Theory]
    [InlineData(typeof(TsClass), "TypeGen.FileContentTest.Comments.Expected.ts-class.ts", false)]
    [InlineData(typeof(TsClass), "TypeGen.FileContentTest.Comments.Expected.ts-class-default-export.ts", true)]
    [InlineData(typeof(TsGeneric<,>), "TypeGen.FileContentTest.Comments.Expected.ts-generic.ts", false)]
    [InlineData(typeof(TsGeneric<,>), "TypeGen.FileContentTest.Comments.Expected.ts-generic-default-export.ts", true)]
    [InlineData(typeof(ITsInterface), "TypeGen.FileContentTest.Comments.Expected.i-ts-interface.ts", false)]
    [InlineData(typeof(ITsInterface), "TypeGen.FileContentTest.Comments.Expected.i-ts-interface-default-export.ts", true)]
    [InlineData(typeof(TsEnum), "TypeGen.FileContentTest.Comments.Expected.ts-enum.ts", false)]
    [InlineData(typeof(TsEnum), "TypeGen.FileContentTest.Comments.Expected.ts-enum-default-export.ts", true)]
    [InlineData(typeof(TsEnumUnion), "TypeGen.FileContentTest.Comments.Expected.ts-enum-union.ts", false)]
    [InlineData(typeof(TsEnumUnion), "TypeGen.FileContentTest.Comments.Expected.ts-enum-union-default-export.ts", true)]
    public async Task TestCommentsGenerationSpec(Type type, string expectedLocation, bool useDefaultExport)
    {
        var generationSpec = new CommentsGenerationSpec();
        var generatorOptions = new GeneratorOptions { UseDefaultExport = useDefaultExport };
        await TestGenerationSpec(type, expectedLocation, generationSpec, generatorOptions);
    }
    
    private class CommentsGenerationSpec : GenerationSpec
    {
        public CommentsGenerationSpec()
        {
            AddClass<TsClass>();
            AddClass(typeof(TsGeneric<,>));
            AddInterface<ITsInterface>();
            AddEnum<TsEnum>();
            AddEnum<TsEnumUnion>(asUnionType: true);
        }
    }
}