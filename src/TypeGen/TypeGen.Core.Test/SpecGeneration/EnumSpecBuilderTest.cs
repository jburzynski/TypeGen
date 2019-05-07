using System;
using System.Linq;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.TypeAnnotations;
using Xunit;

namespace TypeGen.Core.Test.SpecGeneration
{
    public class EnumSpecBuilderTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void DefaultExport_Invoked_SpecUpdated(bool enabled)
        {
            var spec = new TypeSpec(new ExportTsEnumAttribute());
            var builder = new EnumSpecBuilder(spec);

            builder.DefaultExport(enabled);

            Attribute attribute = spec.AdditionalAttributes.FirstOrDefault();
            Assert.IsType<TsDefaultExportAttribute>(attribute);
            Assert.Equal(enabled, ((TsDefaultExportAttribute)attribute).Enabled);
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void StringInitializers_Invoked_SpecUpdated(bool enabled)
        {
            var spec = new TypeSpec(new ExportTsEnumAttribute());
            var builder = new EnumSpecBuilder(spec);

            builder.StringInitializers(enabled);

            Attribute attribute = spec.AdditionalAttributes.FirstOrDefault();
            Assert.IsType<TsStringInitializersAttribute>(attribute);
            Assert.Equal(enabled, ((TsStringInitializersAttribute)attribute).Enabled);
        }
    }
}