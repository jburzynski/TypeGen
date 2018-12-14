using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NSubstitute;
using TypeGen.Core.Business;
using TypeGen.Core.Converters;
using TypeGen.Core.TypeAnnotations;
using Xunit;

namespace TypeGen.Core.Test.Business
{
    public class TsContentGeneratorTest
    {
        private readonly ITypeDependencyService _typeDependencyService = Substitute.For<ITypeDependencyService>();
        private readonly ITypeService _typeService = Substitute.For<ITypeService>();
        private readonly ITemplateService _templateService = Substitute.For<ITemplateService>();
        private readonly ITsContentParser _tsContentParser = Substitute.For<ITsContentParser>();
        
        /// <summary>
        /// this needs to be changed to use mocked MetadataReader
        /// </summary>
        private readonly IMetadataReader _metadataReader = new MetadataReader();

        #region GetImportsText

        [Fact]
        public void GetImportsText_TypeNull_ExceptionThrown()
        {
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, _typeService, _templateService, _tsContentParser, _metadataReader);
            Assert.Throws<ArgumentNullException>(() => tsContentGenerator.GetImportsText(null, "asdf", new TypeNameConverterCollection(), new TypeNameConverterCollection()));
        }
        
        [Fact]
        public void GetImportsText_FileNameConvertersNull_ExceptionThrown()
        {
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, _typeService, _templateService, _tsContentParser, _metadataReader);
            Assert.Throws<ArgumentNullException>(() => tsContentGenerator.GetImportsText(typeof(string), "asdf", null, new TypeNameConverterCollection()));
        }
        
        [Fact]
        public void GetImportsText_TypeNameConvertersNull_ExceptionThrown()
        {
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, _typeService, _templateService, _tsContentParser, _metadataReader);
            Assert.Throws<ArgumentNullException>(() => tsContentGenerator.GetImportsText(typeof(string), "asdf", new TypeNameConverterCollection(), null));
        }
        
        [Theory]
        [MemberData(nameof(GetImportsText_TestCases))]
        public void GetImportsText_TypeGiven_ImportsTextGenerated(Type type,
            string outputDir,
            TypeNameConverterCollection fileNameConverters,
            TypeNameConverterCollection typeNameConverters,
            IEnumerable<object> typeDependencies,
            IEnumerable<MemberInfo> tsExportableMembers,
                string expectedOutput)
        {
            _typeDependencyService.GetTypeDependencies(Arg.Any<Type>()).Returns(typeDependencies);
            _typeService.GetTsExportableMembers(Arg.Any<Type>()).Returns(tsExportableMembers);
            _templateService.FillImportTemplate(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(i => $"{i.ArgAt<string>(0)} | {i.ArgAt<string>(1)} | {i.ArgAt<string>(2)};");
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, _typeService, _templateService, _tsContentParser, _metadataReader);

            string actualOutput = tsContentGenerator.GetImportsText(type, outputDir, fileNameConverters, typeNameConverters);

            Assert.Equal(expectedOutput, actualOutput);
        }

        public static readonly IEnumerable<object[]> GetImportsText_TestCases = new[]
        {
            new object[] { typeof(GetImportsText_TestData.Parent), null, new TypeNameConverterCollection(new PascalCaseToKebabCaseConverter()), new TypeNameConverterCollection(),
                GetImportsText_TestData.ParentTypeDependencies,
                GetImportsText_TestData.ParentMemberInfos,
                "Base |  | ./base;" +
                "DataType1 |  | ./data-type1;" +
                "DataType2 |  | ./data-type2;" +
                "Child1 |  | ./child1;" +
                "Child2 |  | ./child/dir/child2dir/child2;" +
                "Child3 |  | ./child/dir/child3dir/child3;" +
                "Child4 |  | ./child4/default/output/dir/child4;" +
                "ChildEnum |  | ./child/dir/child-enum-dir/child-enum;" +
                "CustomType |  | custom/directory/custom-type;" +
                "OtherType | OT | other/directory/other-type;\r\n"
            },

            new object[] { typeof(GetImportsText_TestData.Parent), "./child/dir/", new TypeNameConverterCollection(new PascalCaseToKebabCaseConverter()), new TypeNameConverterCollection(),
                GetImportsText_TestData.ParentTypeDependencies,
                GetImportsText_TestData.ParentMemberInfos,
                "Base |  | ./base;" +
                "DataType1 |  | ./data-type1;" +
                "DataType2 |  | ./data-type2;" +
                "Child1 |  | ./child1;" +
                "Child2 |  | ./child2dir/child2;" +
                "Child3 |  | ./child3dir/child3;" +
                "Child4 |  | ../../child4/default/output/dir/child4;" +
                "ChildEnum |  | ./child-enum-dir/child-enum;" +
                "CustomType |  | custom/directory/custom-type;" +
                "OtherType | OT | other/directory/other-type;\r\n"
            },
            
            new object[] { typeof(GetImportsText_TestData.ParentCustomBase), null, new TypeNameConverterCollection(new PascalCaseToKebabCaseConverter()), new TypeNameConverterCollection(),
                new TypeDependencyInfo[] {},
                new MemberInfo[] {},
                "Base |  | base/directory/base;\r\n"
            },
            
            new object[] { typeof(GetImportsText_TestData.ParentCustomBaseAlias), null, new TypeNameConverterCollection(new PascalCaseToKebabCaseConverter()), new TypeNameConverterCollection(),
                new TypeDependencyInfo[] {},
                new MemberInfo[] {},
                "Base | B | other/directory/base;\r\n"
            },
        };

        private class GetImportsText_TestData
        {
            public class DataType1 { }
            public class DataType2 { }

            public class Base<T, U> { }

            [TsCustomBase("B", "other/directory/base", "Base")]
            public class ParentCustomBaseAlias
            {
            }

            [TsCustomBase("Base", "base/directory/base")]
            public class ParentCustomBase
            {
            }

            public static readonly IEnumerable<TypeDependencyInfo> ParentTypeDependencies = new[]
            {
                new TypeDependencyInfo(typeof(Base<,>)),
                new TypeDependencyInfo(typeof(DataType1)),
                new TypeDependencyInfo(typeof(DataType2)),
                new TypeDependencyInfo(typeof(Child1)),
                new TypeDependencyInfo(typeof(Child2)),
                new TypeDependencyInfo(typeof(Child3)),
                new TypeDependencyInfo(typeof(Child4), new Attribute[] { new TsDefaultTypeOutputAttribute("child4/default/output/dir") }),
                new TypeDependencyInfo(typeof(ChildEnum))
            };

            public static readonly IEnumerable<MemberInfo> ParentMemberInfos = new MemberInfo[]
            {
                typeof(Parent).GetProperty("PropertyChild1"),
                typeof(Parent).GetProperty("PropertyChild2"),
                typeof(Parent).GetField("FieldChild3"),
                typeof(Parent).GetProperty("PropertyChild4"),
                typeof(Parent).GetProperty("PropertyChildEnum"),
                typeof(Parent).GetProperty("CustomTypeProperty"),
                typeof(Parent).GetProperty("CustomTypePropertyDuplicate"),
                typeof(Parent).GetProperty("CustomTypeAliasProperty"),
                typeof(Parent).GetProperty("CustomTypeAliasPropertyDuplicate")
            };

            public class Parent : Base<DataType1, DataType2>
            {
                public Child1 PropertyChild1 { get; set; }
                public Child2 PropertyChild2 { get; set; }
                public Child3 FieldChild3;

                [TsDefaultTypeOutput("child4/default/output/dir")]
                public Child4 PropertyChild4 { get; set; }

                public ChildEnum PropertyChildEnum { get; set; }

                [TsType("CustomType", "custom/directory/custom-type")]
                public string CustomTypeProperty { get; set; }

                [TsType("CustomType", "custom/directory/custom-type")]
                public string CustomTypePropertyDuplicate { get; set; }

                [TsType("OT", "other/directory/other-type", "OtherType")]
                public string CustomTypeAliasProperty { get; set; }

                [TsType("OT", "other/directory/other-type", "OtherType")]
                public string CustomTypeAliasPropertyDuplicate { get; set; }
            }

            public class Child1 { }

            [ExportTsClass(OutputDir = "child/dir/child2dir")]
            public class Child2 { }

            [ExportTsInterface(OutputDir = "child/dir/child3dir")]
            public class Child3 { }

            public class Child4 { }

            [ExportTsEnum(OutputDir = "child/dir/child-enum-dir")]
            public enum ChildEnum { }
        }
        
        #endregion

        [Fact]
        public void GetExtendsText_TypeNull_ExceptionThrown()
        {
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, _typeService, _templateService, _tsContentParser, _metadataReader);
            Assert.Throws<ArgumentNullException>(() => tsContentGenerator.GetExtendsText(null, new TypeNameConverterCollection()));
        }
        
        [Fact]
        public void GetExtendsText_TypeNameConvertersNull_ExceptionThrown()
        {
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, _typeService, _templateService, _tsContentParser, _metadataReader);
            Assert.Throws<ArgumentNullException>(() => tsContentGenerator.GetExtendsText(typeof(string), null));
        }
        
        [Fact]
        public void GetCustomBody_FilePathNull_ExceptionThrown()
        {
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, _typeService, _templateService, _tsContentParser, _metadataReader);
            Assert.Throws<ArgumentNullException>(() => tsContentGenerator.GetCustomBody(null, 0));
        }
        
        [Fact]
        public void GetCustomHead_FilePathNull_ExceptionThrown()
        {
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, _typeService, _templateService, _tsContentParser, _metadataReader);
            Assert.Throws<ArgumentNullException>(() => tsContentGenerator.GetCustomHead(null));
        }
    }
}
