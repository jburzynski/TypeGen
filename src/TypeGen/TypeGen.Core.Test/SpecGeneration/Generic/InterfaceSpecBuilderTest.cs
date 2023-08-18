using System;
using System.Linq;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.TypeAnnotations;
using Xunit;

namespace TypeGen.Core.Test.SpecGeneration.Builders.Generic
{
    public class InterfaceSpecBuilderTest
    {
        private class ExportedClass
        {
            public string member;
        }
        
        [Fact]
        public void Member_NameGiven_MemberAddedToSpec()
        {
            const string member = "member";
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.Member(member);

            Assert.True(spec.MemberAttributes.ContainsKey(member));
        }
        
        [Fact]
        public void Member_AttributesSpecifiedForMember_AttributesAddedToCorrectMember()
        {
            const string member1 = "member1";
            const string member2 = "member2";
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.Member(member1).Ignore().Member(member2).Null();

            Attribute attribute1 = spec.MemberAttributes[member1].FirstOrDefault();
            Attribute attribute2 = spec.MemberAttributes[member2].FirstOrDefault();
            Assert.IsType<TsIgnoreAttribute>(attribute1);
            Assert.IsType<TsNullAttribute>(attribute2);
        }
        
        [Fact]
        public void Member_FuncGiven_MemberAddedToSpec()
        {
            const string member = "member";
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.Member(x => nameof(x.member));

            Assert.True(spec.MemberAttributes.ContainsKey(member));
        }
        
        [Fact]
        public void Member_Invoked_MemberAddedToSpec()
        {
            const string member = "member";
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.Member(member);

            Assert.True(spec.MemberAttributes.ContainsKey(member));
        }
        
        [Fact]
        public void CustomBase_Invoked_SpecUpdated()
        {
            const string @base = "base";
            const string importPath = "importPath";
            const string originalTypeName = "originalTypeName";
            const bool isDefaultExport = true;
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.CustomBase(@base, importPath, originalTypeName, isDefaultExport);

            Attribute attribute = spec.AdditionalAttributes.FirstOrDefault();
            Assert.IsType<TsCustomBaseAttribute>(attribute);
            Assert.Equal(@base, ((TsCustomBaseAttribute)attribute).Base);
            Assert.Equal(importPath, ((TsCustomBaseAttribute)attribute).ImportPath);
            Assert.Equal(originalTypeName, ((TsCustomBaseAttribute)attribute).OriginalTypeName);
            Assert.Equal(isDefaultExport, ((TsCustomBaseAttribute)attribute).IsDefaultExport);
        }
        
        [Fact]
        public void CustomHeader_Invoked_SpecUpdated()
        {
            const string header = "header";
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.CustomHeader(header);

            var attribute = spec.ExportAttribute;
            Assert.Equal(header, attribute.CustomHeader);
        }
        
        [Fact]
        public void CustomBody_Invoked_SpecUpdated()
        {
            const string body = "body";
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.CustomBody(body);

            var attribute = spec.ExportAttribute;
            Assert.Equal(body, attribute.CustomBody);
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void DefaultExport_Invoked_SpecUpdated(bool enabled)
        {
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.DefaultExport(enabled);

            Attribute attribute = spec.AdditionalAttributes.FirstOrDefault();
            Assert.IsType<TsDefaultExportAttribute>(attribute);
            Assert.Equal(enabled, ((TsDefaultExportAttribute)attribute).Enabled);
        }
        
        [Fact]
        public void DefaultTypeOutput_Invoked_SpecUpdated()
        {
            const string member = "member";
            const string outputDir = "outputDir";
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.Member(member).DefaultTypeOutput(outputDir);

            Attribute attribute = spec.MemberAttributes[member].FirstOrDefault();
            Assert.IsType<TsDefaultTypeOutputAttribute>(attribute);
            Assert.Equal(outputDir, ((TsDefaultTypeOutputAttribute)attribute).OutputDir);
        }
        
        [Fact]
        public void DefaultValue_Invoked_SpecUpdated()
        {
            const string member = "member";
            const string defaultValue = "defaultValue";
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.Member(member).DefaultValue(defaultValue);

            Attribute attribute = spec.MemberAttributes[member].FirstOrDefault();
            Assert.IsType<TsDefaultValueAttribute>(attribute);
            Assert.Equal(defaultValue, ((TsDefaultValueAttribute)attribute).DefaultValue);
        }
        
        [Fact]
        public void Ignore_Invoked_SpecUpdated()
        {
            const string member = "member";
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.Member(member).Ignore();

            Attribute attribute = spec.MemberAttributes[member].FirstOrDefault();
            Assert.IsType<TsIgnoreAttribute>(attribute);
        }
        
        [Fact]
        public void IgnoreBase_Invoked_SpecUpdated()
        {
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.IgnoreBase();

            Attribute attribute = spec.AdditionalAttributes.FirstOrDefault();
            Assert.IsType<TsIgnoreBaseAttribute>(attribute);
        }
        
        [Fact]
        public void MemberName_Invoked_SpecUpdated()
        {
            const string member = "member";
            const string name = "name";
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.Member(member).MemberName(name);

            Attribute attribute = spec.MemberAttributes[member].FirstOrDefault();
            Assert.IsType<TsMemberNameAttribute>(attribute);
            Assert.Equal(name, ((TsMemberNameAttribute)attribute).Name);
        }
        
        [Fact]
        public void NotNull_Invoked_SpecUpdated()
        {
            const string member = "member";
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.Member(member).NotNull();

            Attribute attribute = spec.MemberAttributes[member].FirstOrDefault();
            Assert.IsType<TsNotNullAttribute>(attribute);
        }
        
        [Fact]
        public void NotUndefined_Invoked_SpecUpdated()
        {
            const string member = "member";
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.Member(member).NotUndefined();

            Attribute attribute = spec.MemberAttributes[member].FirstOrDefault();
            Assert.IsType<TsNotUndefinedAttribute>(attribute);
        }
        
        [Fact]
        public void Null_Invoked_SpecUpdated()
        {
            const string member = "member";
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.Member(member).Null();

            Attribute attribute = spec.MemberAttributes[member].FirstOrDefault();
            Assert.IsType<TsNullAttribute>(attribute);
        }
        
        [Fact]
        public void Optional_Invoked_SpecUpdated()
        {
            const string member = "member";
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.Member(member).Optional();

            Attribute attribute = spec.MemberAttributes[member].FirstOrDefault();
            Assert.IsType<TsOptionalAttribute>(attribute);
        }
        
        [Fact]
        public void Type_Invoked_SpecUpdated()
        {
            const string member = "member";
            const string typeName = "typeName";
            const string importPath = "importPath";
            const string originalTypeName = "originalTypeName";
            const bool isDefaultExport = true;
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.Member(member).Type(typeName, importPath, originalTypeName, isDefaultExport);

            Attribute attribute = spec.MemberAttributes[member].FirstOrDefault();
            Assert.IsType<TsTypeAttribute>(attribute);
            Assert.Equal(typeName, ((TsTypeAttribute)attribute).TypeName);
            Assert.Equal(importPath, ((TsTypeAttribute)attribute).ImportPath);
            Assert.Equal(originalTypeName, ((TsTypeAttribute)attribute).OriginalTypeName);
            Assert.Equal(isDefaultExport, ((TsTypeAttribute)attribute).IsDefaultExport);
        }
        
        [Fact]
        public void TypeUnions_Invoked_SpecUpdated()
        {
            const string member = "member";
            string[] typeUnions = { "null", "undefined" };
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.Member(member).TypeUnions(typeUnions);

            Attribute attribute = spec.MemberAttributes[member].FirstOrDefault();
            Assert.IsType<TsTypeUnionsAttribute>(attribute);
            Assert.Same(typeUnions, ((TsTypeUnionsAttribute)attribute).TypeUnions);
        }
        
        [Fact]
        public void Undefined_Invoked_SpecUpdated()
        {
            const string member = "member";
            var spec = new TypeSpec(new ExportTsInterfaceAttribute());
            var builder = new TypeGen.Core.SpecGeneration.Builders.Generic.InterfaceSpecBuilder<ExportedClass>(spec);

            builder.Member(member).Undefined();

            Attribute attribute = spec.MemberAttributes[member].FirstOrDefault();
            Assert.IsType<TsUndefinedAttribute>(attribute);
        }
    }
}
