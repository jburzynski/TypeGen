using System;
using System.Threading.Tasks;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.FileContentTest.ImportType.Entities;
using TypeGen.FileContentTest.TestingUtils;
using Xunit;
using Xunit.Abstractions;

namespace TypeGen.FileContentTest.ImportType;

public class ImportTypeTest : GenerationTestBase
{
    public ImportTypeTest(ITestOutputHelper output) : base(output) { }

    [Theory]
    [InlineData(typeof(TsClass), "TypeGen.FileContentTest.ImportType.Expected.ts-class.ts", false)]
    [InlineData(typeof(TsClass), "TypeGen.FileContentTest.ImportType.Expected.ts-class-default-export.ts", true)]
    public async Task TestImportType(Type type, string expectedLocation, bool useDefaultExport)
    {
        var generationSpec = new ImportTypeGenerationSpec();
        var generatorOptions = new GeneratorOptions { UseDefaultExport = useDefaultExport, UseImportType = true };
        await TestGenerationSpec(type, expectedLocation, generationSpec, generatorOptions);
    }

    private class ImportTypeGenerationSpec : GenerationSpec
    {
        public ImportTypeGenerationSpec()
        {
            AddClass<TsClass>();
        }
    }
}