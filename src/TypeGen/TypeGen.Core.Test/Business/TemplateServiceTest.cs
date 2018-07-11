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
        private readonly IInternalStorage _internalStorage;

        public TemplateServiceTest()
        {
            _internalStorage = Substitute.For<IInternalStorage>();
        }

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
        public void FillClassPropertyWithDefaultValueTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.ClassPropertyWithDefaultValue.tpl")
                .Returns("$tg{accessor} | $tg{name} | $tg{type} | $tg{defaultValue}");
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = new GeneratorOptions() };

            string actual = templateService.FillClassPropertyWithDefaultValueTemplate("a", "B", "c", "D");
            Assert.Equal("a | B | c | D", actual);
        }

        [Fact]
        public void FillClassPropertyTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.ClassProperty.tpl")
                .Returns("$tg{accessor} | $tg{name} | $tg{type}");
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = new GeneratorOptions() };

            string actual = templateService.FillClassPropertyTemplate("a", "B", "c");
            Assert.Equal("a | B | c", actual);
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
        public void FillInterfacePropertyTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.InterfaceProperty.tpl")
                .Returns("$tg{name} | $tg{modifier} | $tg{type}");
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = new GeneratorOptions() };

            string actualOptional = templateService.FillInterfacePropertyTemplate("a", "B", true);
            string actualNonOptional = templateService.FillInterfacePropertyTemplate("a", "B", false);

            Assert.Equal("a | ? | B", actualOptional);
            Assert.Equal("a |  | B", actualNonOptional);
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
        public void FillEnumValueTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.EnumValue.tpl")
                .Returns("$tg{name} | $tg{number}");
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = new GeneratorOptions() };

            string actual = templateService.FillEnumValueTemplate("a", 42);
            Assert.Equal("a | 42", actual);
        }

        [Fact]
        public void FillImportTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Import.tpl")
                .Returns("$tg{name} | $tg{asAlias} | $tg{path}");
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = new GeneratorOptions() };

            string actual = templateService.FillImportTemplate("a", "B", "c");
            Assert.Equal("a | B | c", actual);
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
        public void FillIndexExportTemplate_ValuesGiven_TemplateFilledWithValues()
        {
            _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.IndexExport.tpl")
                .Returns("$tg{filename}");
            var templateService = new TemplateService(_internalStorage) { GeneratorOptions = new GeneratorOptions() };

            string actual = templateService.FillIndexExportTemplate("a");
            Assert.Equal("a", actual);
        }
    }
}
