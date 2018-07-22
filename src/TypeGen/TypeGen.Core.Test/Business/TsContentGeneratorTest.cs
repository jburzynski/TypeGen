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
        private readonly ITypeDependencyService _typeDependencyService;
        private readonly ITypeService _typeService;
        private readonly ITemplateService _templateService;
        private readonly ITsContentParser _tsContentParser;

        public TsContentGeneratorTest()
        {
            _typeDependencyService = Substitute.For<ITypeDependencyService>();
            _typeService = Substitute.For<ITypeService>();
            _templateService = Substitute.For<ITemplateService>();
            _tsContentParser = Substitute.For<ITsContentParser>();
        }

        [Theory]
        [MemberData(nameof(GetImportsText_TestCases))]
        public void GetImportsText_TypeGiven_ImportsTextGenerated(Type type,
            string outputDir,
            TypeNameConverterCollection fileNameConverters,
            TypeNameConverterCollection typeNameConverters,
            string expectedOutput)
        {
            _typeDependencyService.GetTypeDependencies(Arg.Any<Type>()).Returns(GetImportsText_TestData.ParentTypeDependencies);
            _typeService.GetTsExportableMembers(Arg.Any<Type>()).Returns(GetImportsText_TestData.ParentMemberInfos);
            _templateService.FillImportTemplate(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(i => $"{i.ArgAt<string>(0)} | {i.ArgAt<string>(1)} | {i.ArgAt<string>(2)};");
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, _typeService, _templateService, _tsContentParser);

            string actualOutput = tsContentGenerator.GetImportsText(type, outputDir, fileNameConverters, typeNameConverters);

            Assert.Equal(expectedOutput, actualOutput);
        }

        public static readonly IEnumerable<object[]> GetImportsText_TestCases = new[]
        {
            new object[] { typeof(GetImportsText_TestData.Parent), null, new TypeNameConverterCollection(new PascalCaseToKebabCaseConverter()), new TypeNameConverterCollection(),
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

            new object[] { typeof(GetImportsText_TestData.Parent), "./child/dir", new TypeNameConverterCollection(new PascalCaseToKebabCaseConverter()), new TypeNameConverterCollection(),
                "Base |  | ./base;" +
                "DataType1 |  | ./data-type1;" +
                "DataType2 |  | ./data-type2;" +
                "Child1 |  | ./child1;" +
                "Child2 |  | ./child2dir/child2;" +
                "Child3 |  | ./child3dir/child3;" +
                "Child4 |  | ./child4/default/output/dir/child4;" +
                "ChildEnum |  | ./child-enum-dir/child-enum;" +
                "CustomType |  | custom/directory/custom-type;" +
                "OtherType | OT | other/directory/other-type;\r\n"
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
                new TypeDependencyInfo { Type = typeof(Base<,>) },
                new TypeDependencyInfo { Type = typeof(DataType1) },
                new TypeDependencyInfo { Type = typeof(DataType2) },
                new TypeDependencyInfo { Type = typeof(Child1) },
                new TypeDependencyInfo { Type = typeof(Child2) },
                new TypeDependencyInfo { Type = typeof(Child3) },
                new TypeDependencyInfo { Type = typeof(Child4), MemberAttributes = new Attribute[] { new TsDefaultTypeOutputAttribute("child4/default/output/dir") }},
                new TypeDependencyInfo { Type = typeof(ChildEnum) }
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
    }
}
