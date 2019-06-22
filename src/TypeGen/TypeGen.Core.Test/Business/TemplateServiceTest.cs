using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using TypeGen.Core.Generator;
using TypeGen.Core.Generator.Services;
using TypeGen.Core.Storage;
using Xunit;

namespace TypeGen.Core.Test.Business
{
    public class TemplateServiceTest
    {
        private readonly IInternalStorage _internalStorage = Substitute.For<IInternalStorage>();

        [Fact]
        public void GetExtendsText_NameGiven_ExtendsTextReturned()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);
            
            //act
            string actual = templateService.GetExtendsText("MyName");
            
            //assert
            Assert.Equal(" extends MyName", actual);
        }

        [Fact]
        public void FillClassTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Class.tpl")
                .Returns("$tg{imports} | $tg{name} | $tg{extends} | $tg{properties} | $tg{customHead} | $tg{customBody}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actual = templateService.FillClassTemplate("a", "B", "c", "D", "e", "F");
            
            //assert
            Assert.Equal("a | B | c | D | e | F", actual);
        }

        [Fact]
        public void FillClassTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            //arrange
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = generatorOptions };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Class.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actualDoubleQuote = templateService.FillClassTemplate("", "", "", "", "", "");
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillClassTemplate("", "", "", "", "", "");

            //assert
            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }
        
        [Fact]
        public void FillClassDefaultExportTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.ClassDefaultExport.tpl")
                .Returns("$tg{imports} | $tg{name} | $tg{exportName} | $tg{extends} | $tg{properties} | $tg{customHead} | $tg{customBody}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actual = templateService.FillClassDefaultExportTemplate("a", "B", "c", "D", "e", "F", "g");
            
            //assert
            Assert.Equal("a | B | c | D | e | F | g", actual);
        }

        [Fact]
        public void FillClassDefaultExportTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            //arrange
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = generatorOptions };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.ClassDefaultExport.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actualDoubleQuote = templateService.FillClassDefaultExportTemplate("", "", "", "", "", "", "");
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillClassDefaultExportTemplate("", "", "", "", "", "", "");

            //assert
            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }

        [Fact]
        public void FillClassPropertyTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.ClassProperty.tpl")
                .Returns("$tg{modifiers} | $tg{name} | $tg{type} | $tg{defaultValue}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actual = templateService.FillClassPropertyTemplate("a", "B", "c", "D");
            
            //assert
            Assert.Equal("a | B | : c |  = D", actual);
        }

        [Fact]
        public void FillClassPropertyTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            //arrange
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = generatorOptions };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.ClassProperty.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actualDoubleQuote = templateService.FillClassPropertyTemplate("", "", "", "");
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillClassPropertyTemplate("", "", "", "");

            //assert
            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }

        [Fact]
        public void FillInterfaceTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Interface.tpl")
                .Returns("$tg{imports} | $tg{name} | $tg{extends} | $tg{properties} | $tg{customHead} | $tg{customBody}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actual = templateService.FillInterfaceTemplate("a", "B", "c", "D", "e", "F");
            
            //assert
            Assert.Equal("a | B | c | D | e | F", actual);
        }

        [Fact]
        public void FillInterfaceTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            //arrange
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = generatorOptions };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Interface.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actualDoubleQuote = templateService.FillInterfaceTemplate("", "", "", "", "", "");
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillInterfaceTemplate("", "", "", "", "", "");

            //assert
            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }
        
        [Fact]
        public void FillInterfaceDefaultExportTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.InterfaceDefaultExport.tpl")
                .Returns("$tg{imports} | $tg{name} | $tg{exportName} | $tg{extends} | $tg{properties} | $tg{customHead} | $tg{customBody}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actual = templateService.FillInterfaceDefaultExportTemplate("a", "B", "c", "D", "e", "F", "g");
            
            //assert
            Assert.Equal("a | B | c | D | e | F | g", actual);
        }

        [Fact]
        public void FillInterfaceDefaultExportTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            //arrange
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = generatorOptions };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.InterfaceDefaultExport.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actualDoubleQuote = templateService.FillInterfaceDefaultExportTemplate("", "", "", "", "", "", "");
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillInterfaceDefaultExportTemplate("", "", "", "", "", "", "");

            //assert
            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }

        [Fact]
        public void FillInterfacePropertyTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.InterfaceProperty.tpl")
                .Returns("$tg{modifiers} | $tg{name} | $tg{type}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actualOptional = templateService.FillInterfacePropertyTemplate("a", "B", "c", true);
            string actualNonOptional = templateService.FillInterfacePropertyTemplate("a", "B", "c", false);

            //assert
            Assert.Equal("a | B? | : c", actualOptional);
            Assert.Equal("a | B | : c", actualNonOptional);
        }

        [Fact]
        public void FillInterfacePropertyTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            //arrange
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = generatorOptions };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.InterfaceProperty.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actualDoubleQuote = templateService.FillInterfacePropertyTemplate("", "", "", false);
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillInterfacePropertyTemplate("", "", "", false);

            //assert
            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }

        [Fact]
        public void FillEnumTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Enum.tpl")
                .Returns("$tg{imports} | $tg{name} | $tg{values} | $tg{modifiers}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actualConst = templateService.FillEnumTemplate("a", "B", "c", true);
            string actualNotConst = templateService.FillEnumTemplate("a", "B", "c", false);

            //assert
            Assert.Equal("a | B | c |  const", actualConst);
            Assert.Equal("a | B | c | ", actualNotConst);
        }

        [Fact]
        public void FillEnumTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            //arrange
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = generatorOptions };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Enum.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actualDoubleQuote = templateService.FillEnumTemplate("", "", "", false);
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillEnumTemplate("", "", "", false);

            //assert
            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }
        
        [Fact]
        public void FillEnumDefaultExportTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.EnumDefaultExport.tpl")
                .Returns("$tg{imports} | $tg{name} | $tg{values} | $tg{modifiers}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actualConst = templateService.FillEnumDefaultExportTemplate("a", "B", "c", true);
            string actualNotConst = templateService.FillEnumDefaultExportTemplate("a", "B", "c", false);

            //assert
            Assert.Equal("a | B | c |  const", actualConst);
            Assert.Equal("a | B | c | ", actualNotConst);
        }

        [Fact]
        public void FillEnumDefaultExportTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            //arrange
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = generatorOptions };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.EnumDefaultExport.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actualDoubleQuote = templateService.FillEnumDefaultExportTemplate("", "", "", false);
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillEnumDefaultExportTemplate("", "", "", false);

            //assert
            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }

        [Fact]
        public void FillEnumValueTemplate_IntValuesGiven_TemplateFilledWithValues()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.EnumValue.tpl")
                .Returns("$tg{name} | $tg{value}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actual = templateService.FillEnumValueTemplate("a", 42);
            
            //assert
            Assert.Equal("a | 42", actual);
        }
        
        [Fact]
        public void FillEnumValueTemplate_StringValuesGiven_TemplateFilledWithValues()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.EnumValue.tpl")
                .Returns("$tg{name} | $tg{value}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actual = templateService.FillEnumValueTemplate("a", "stringValue");
            
            //assert
            Assert.Equal(@"a | ""stringValue""", actual);
        }

        [Fact]
        public void FillEnumValueTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            //arrange
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = generatorOptions };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.EnumValue.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actualDoubleQuote = templateService.FillEnumValueTemplate("", 0);
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillEnumValueTemplate("", 0);

            //assert
            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }

        [Theory]
        [InlineData("B", "a |  as B | c")]
        [InlineData("", "a |  | c")]
        public void FillImportTemplate_ValuesGiven_TemplateFilledWithValues(string typeAlias, string expectedResult)
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Import.tpl")
                .Returns("$tg{name} | $tg{aliasText} | $tg{path}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actual = templateService.FillImportTemplate("a", typeAlias, "c");
            
            //assert
            Assert.Equal(expectedResult, actual);
        }

        [Fact]
        public void FillImportTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            //arrange
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = generatorOptions };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Import.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actualDoubleQuote = templateService.FillImportTemplate("", "", "");
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillImportTemplate("", "", "");

            //assert
            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }

        [Fact]
        public void FillIndexTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Index.tpl")
                .Returns("$tg{exports}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actual = templateService.FillIndexTemplate("a");
            
            //assert
            Assert.Equal("a", actual);
        }

        [Fact]
        public void FillIndexTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            //arrange
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = generatorOptions };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Index.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actualDoubleQuote = templateService.FillIndexTemplate("");
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillIndexTemplate("");

            //assert
            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }

        [Fact]
        public void FillIndexExportTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.IndexExport.tpl")
                .Returns("$tg{filename}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actual = templateService.FillIndexExportTemplate("a");
            
            //assert
            Assert.Equal("a", actual);
        }

        [Fact]
        public void FillIndexExportTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            //arrange
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = generatorOptions };
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.IndexExport.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var templateService = new TemplateService(_internalStorage, generatorOptionsProvider);

            //act
            string actualDoubleQuote = templateService.FillIndexExportTemplate("");
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillIndexExportTemplate("");

            //assert
            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }
    }
}
