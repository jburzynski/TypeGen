using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using TypeGen.Core.Business;
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
            var templateService = new TemplateService(_internalStorage);
            string actual = templateService.GetExtendsText("MyName");
            Assert.Equal(" extends MyName", actual);
        }

        [Fact]
        public void FillClassTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Class.tpl")
                .Returns("$tg{imports} | $tg{name} | $tg{extends} | $tg{properties} | $tg{customHead} | $tg{customBody}");
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = new GeneratorOptions() };

            string actual = templateService.FillClassTemplate("a", "B", "c", "D", "e", "F");
            Assert.Equal("a | B | c | D | e | F", actual);
        }

        [Fact]
        public void FillClassTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Class.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = generatorOptions };

            string actualDoubleQuote = templateService.FillClassTemplate("", "", "", "", "", "");
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillClassTemplate("", "", "", "", "", "");

            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }
        
        [Fact]
        public void FillClassDefaultExportTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.ClassDefaultExport.tpl")
                .Returns("$tg{imports} | $tg{name} | $tg{exportName} | $tg{extends} | $tg{properties} | $tg{customHead} | $tg{customBody}");
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = new GeneratorOptions() };

            string actual = templateService.FillClassDefaultExportTemplate("a", "B", "c", "D", "e", "F", "g");
            Assert.Equal("a | B | c | D | e | F | g", actual);
        }

        [Fact]
        public void FillClassDefaultExportTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.ClassDefaultExport.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = generatorOptions };

            string actualDoubleQuote = templateService.FillClassDefaultExportTemplate("", "", "", "", "", "", "");
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillClassDefaultExportTemplate("", "", "", "", "", "", "");

            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }

        [Fact]
        public void FillClassPropertyTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.ClassProperty.tpl")
                .Returns("$tg{modifiers} | $tg{name} | $tg{type} | $tg{defaultValue}");
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = new GeneratorOptions() };

            string actual = templateService.FillClassPropertyTemplate("a", "B", "c", "D");
            Assert.Equal("a | B | : c |  = D", actual);
        }

        [Fact]
        public void FillClassPropertyTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.ClassProperty.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = generatorOptions };

            string actualDoubleQuote = templateService.FillClassPropertyTemplate("", "", "", "");
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillClassPropertyTemplate("", "", "", "");

            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }

        [Fact]
        public void FillInterfaceTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Interface.tpl")
                .Returns("$tg{imports} | $tg{name} | $tg{extends} | $tg{properties} | $tg{customHead} | $tg{customBody}");
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = new GeneratorOptions() };

            string actual = templateService.FillInterfaceTemplate("a", "B", "c", "D", "e", "F");
            Assert.Equal("a | B | c | D | e | F", actual);
        }

        [Fact]
        public void FillInterfaceTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Interface.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = generatorOptions };

            string actualDoubleQuote = templateService.FillInterfaceTemplate("", "", "", "", "", "");
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillInterfaceTemplate("", "", "", "", "", "");

            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }
        
        [Fact]
        public void FillInterfaceDefaultExportTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.InterfaceDefaultExport.tpl")
                .Returns("$tg{imports} | $tg{name} | $tg{exportName} | $tg{extends} | $tg{properties} | $tg{customHead} | $tg{customBody}");
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = new GeneratorOptions() };

            string actual = templateService.FillInterfaceDefaultExportTemplate("a", "B", "c", "D", "e", "F", "g");
            Assert.Equal("a | B | c | D | e | F | g", actual);
        }

        [Fact]
        public void FillInterfaceDefaultExportTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.InterfaceDefaultExport.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = generatorOptions };

            string actualDoubleQuote = templateService.FillInterfaceDefaultExportTemplate("", "", "", "", "", "", "");
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillInterfaceDefaultExportTemplate("", "", "", "", "", "", "");

            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }

        [Fact]
        public void FillInterfacePropertyTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.InterfaceProperty.tpl")
                .Returns("$tg{modifiers} | $tg{name} | $tg{type}");
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = new GeneratorOptions() };

            string actualOptional = templateService.FillInterfacePropertyTemplate("a", "B", "c", true);
            string actualNonOptional = templateService.FillInterfacePropertyTemplate("a", "B", "c", false);

            Assert.Equal("a | B? | : c", actualOptional);
            Assert.Equal("a | B | : c", actualNonOptional);
        }

        [Fact]
        public void FillInterfacePropertyTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.InterfaceProperty.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = generatorOptions };

            string actualDoubleQuote = templateService.FillInterfacePropertyTemplate("", "", "", false);
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillInterfacePropertyTemplate("", "", "", false);

            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }

        [Fact]
        public void FillEnumTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Enum.tpl")
                .Returns("$tg{imports} | $tg{name} | $tg{values} | $tg{modifiers}");
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = new GeneratorOptions() };

            string actualConst = templateService.FillEnumTemplate("a", "B", "c", true);
            string actualNotConst = templateService.FillEnumTemplate("a", "B", "c", false);

            Assert.Equal("a | B | c |  const", actualConst);
            Assert.Equal("a | B | c | ", actualNotConst);
        }

        [Fact]
        public void FillEnumTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Enum.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = generatorOptions };

            string actualDoubleQuote = templateService.FillEnumTemplate("", "", "", false);
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillEnumTemplate("", "", "", false);

            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }
        
        [Fact]
        public void FillEnumDefaultExportTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.EnumDefaultExport.tpl")
                .Returns("$tg{imports} | $tg{name} | $tg{values} | $tg{modifiers}");
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = new GeneratorOptions() };

            string actualConst = templateService.FillEnumDefaultExportTemplate("a", "B", "c", true);
            string actualNotConst = templateService.FillEnumDefaultExportTemplate("a", "B", "c", false);

            Assert.Equal("a | B | c |  const", actualConst);
            Assert.Equal("a | B | c | ", actualNotConst);
        }

        [Fact]
        public void FillEnumDefaultExportTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.EnumDefaultExport.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = generatorOptions };

            string actualDoubleQuote = templateService.FillEnumDefaultExportTemplate("", "", "", false);
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillEnumDefaultExportTemplate("", "", "", false);

            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }

        [Fact]
        public void FillEnumValueTemplate_IntValuesGiven_TemplateFilledWithValues()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.EnumValue.tpl")
                .Returns("$tg{name} | $tg{value}");
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = new GeneratorOptions() };

            string actual = templateService.FillEnumValueTemplate("a", 42);
            Assert.Equal("a | 42", actual);
        }
        
        [Fact]
        public void FillEnumValueTemplate_StringValuesGiven_TemplateFilledWithValues()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.EnumValue.tpl")
                .Returns("$tg{name} | $tg{value}");
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = new GeneratorOptions() };

            string actual = templateService.FillEnumValueTemplate("a", "stringValue");
            Assert.Equal(@"a | ""stringValue""", actual);
        }

        [Fact]
        public void FillEnumValueTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.EnumValue.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = generatorOptions };

            string actualDoubleQuote = templateService.FillEnumValueTemplate("", 0);
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillEnumValueTemplate("", 0);

            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }

        [Theory]
        [InlineData("B", "a |  as B | c")]
        [InlineData("", "a |  | c")]
        public void FillImportTemplate_ValuesGiven_TemplateFilledWithValues(string typeAlias, string expectedResult)
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Import.tpl")
                .Returns("$tg{name} | $tg{aliasText} | $tg{path}");
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = new GeneratorOptions() };

            string actual = templateService.FillImportTemplate("a", typeAlias, "c");
            Assert.Equal(expectedResult, actual);
        }

        [Fact]
        public void FillImportTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Import.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = generatorOptions };

            string actualDoubleQuote = templateService.FillImportTemplate("", "", "");
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillImportTemplate("", "", "");

            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }

        [Fact]
        public void FillIndexTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Index.tpl")
                .Returns("$tg{exports}");
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = new GeneratorOptions() };

            string actual = templateService.FillIndexTemplate("a");
            Assert.Equal("a", actual);
        }

        [Fact]
        public void FillIndexTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Index.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = generatorOptions };

            string actualDoubleQuote = templateService.FillIndexTemplate("");
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillIndexTemplate("");

            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }

        [Fact]
        public void FillIndexExportTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.IndexExport.tpl")
                .Returns("$tg{filename}");
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = new GeneratorOptions() };

            string actual = templateService.FillIndexExportTemplate("a");
            Assert.Equal("a", actual);
        }

        [Fact]
        public void FillIndexExportTemplate_SpecialCharsPresent_SpecialCharsReplaced()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.IndexExport.tpl")
                .Returns("$tg{tab} | $tg{quot}");
            var generatorOptions = new GeneratorOptions { TabLength = 3 };
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = generatorOptions };

            string actualDoubleQuote = templateService.FillIndexExportTemplate("");
            generatorOptions.SingleQuotes = true;
            string actualSingleQuote = templateService.FillIndexExportTemplate("");

            Assert.Equal("    | \"", actualDoubleQuote);
            Assert.Equal("    | '", actualSingleQuote);
        }
    }
}
