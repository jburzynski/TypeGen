using System;
using System.Threading.Tasks;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.IntegrationTest.IgnoreBaseInterfaces.Entities;
using TypeGen.IntegrationTest.ImportType.Entities;
using TypeGen.IntegrationTest.TestingUtils;
using Xunit;

namespace TypeGen.IntegrationTest.ImportType;

public class ImportTypeTest : GenerationTestBase
{
    [Theory]
    [InlineData(typeof(TsClass), "TypeGen.IntegrationTest.ImportType.Expected.ts-class.ts", false)]
    [InlineData(typeof(TsClass), "TypeGen.IntegrationTest.ImportType.Expected.ts-class-default-export.ts", true)]
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