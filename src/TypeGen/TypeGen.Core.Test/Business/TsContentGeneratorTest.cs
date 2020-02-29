using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NSubstitute;
using TypeGen.Core.Converters;
using TypeGen.Core.Generator;
using TypeGen.Core.Generator.Services;
using TypeGen.Core.Metadata;
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
        private readonly IMetadataReaderFactory _metadataReaderFactory;

        public TsContentGeneratorTest()
        {
            // this needs to be changed to use mocked MetadataReader
            
            _metadataReaderFactory = Substitute.For<IMetadataReaderFactory>();
            _metadataReaderFactory.GetInstance().Returns(new AttributeMetadataReader());
        }

        #region GetImportsText

        [Fact]
        public void GetImportsText_TypeNull_ExceptionThrown()
        {
        
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, _typeService, _templateService, _tsContentParser, _metadataReaderFactory, generatorOptionsProvider, null);
            
            //act,assert
            Assert.Throws<ArgumentNullException>(() => tsContentGenerator.GetImportsText(null, "asdf"));
        }
        
        [Fact]
        public void GetImportsText_FileNameConvertersNull_ExceptionThrown()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions { FileNameConverters = null } };
            
            //act,assert
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, _typeService, _templateService, _tsContentParser, _metadataReaderFactory, generatorOptionsProvider, null);
            Assert.Throws<ArgumentNullException>(() => tsContentGenerator.GetImportsText(typeof(string), "asdf"));
        }
        
        [Fact]
        public void GetImportsText_TypeNameConvertersNull_ExceptionThrown()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions { TypeNameConverters = null } };
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, _typeService, _templateService, _tsContentParser, _metadataReaderFactory, generatorOptionsProvider, null);
            
            //act,assert
            Assert.Throws<ArgumentNullException>(() => tsContentGenerator.GetImportsText(typeof(string), "asdf"));
        }
        
        [Theory]
        [MemberData(nameof(GetImportsText_TestCases))]
        public void GetImportsText_TypeGiven_ImportsTextGenerated(Type type,
            string outputDir,
            TypeNameConverterCollection fileNameConverters,
            TypeNameConverterCollection typeNameConverters,
            IEnumerable<object> typeDependencies,
                string expectedOutput)
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions
            {
                FileNameConverters = fileNameConverters,
                TypeNameConverters = typeNameConverters
            } };
            _typeDependencyService.GetTypeDependencies(Arg.Any<Type>()).Returns(typeDependencies);
            _templateService.FillImportTemplate(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(i => $"{i.ArgAt<string>(0)} | {i.ArgAt<string>(1)} | {i.ArgAt<string>(2)};");
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, _typeService, _templateService, _tsContentParser, _metadataReaderFactory, generatorOptionsProvider, null);

            //act
            string actualOutput = tsContentGenerator.GetImportsText(type, outputDir);

            //assert
            Assert.Equal(expectedOutput, actualOutput);
        }

        public static readonly IEnumerable<object[]> GetImportsText_TestCases = new[]
        {
            new object[] { typeof(GetImportsText_TestData.Parent), null, new TypeNameConverterCollection(new PascalCaseToKebabCaseConverter()), new TypeNameConverterCollection(),
                GetImportsText_TestData.ParentTypeDependencies,
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
                "Base |  | base/directory/base;\r\n"
            },
            
            new object[] { typeof(GetImportsText_TestData.ParentCustomBaseAlias), null, new TypeNameConverterCollection(new PascalCaseToKebabCaseConverter()), new TypeNameConverterCollection(),
                new TypeDependencyInfo[] {},
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
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, _typeService, _templateService, _tsContentParser, _metadataReaderFactory, generatorOptionsProvider, null);
            
            //act,assert
            Assert.Throws<ArgumentNullException>(() => tsContentGenerator.GetExtendsText(null));
        }
        
        [Fact]
        public void GetExtendsText_TypeNameConvertersNull_ExceptionThrown()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions { TypeNameConverters = null } };
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, _typeService, _templateService, _tsContentParser, _metadataReaderFactory, generatorOptionsProvider, null);
            
            //act,assert
            Assert.Throws<ArgumentNullException>(() => tsContentGenerator.GetExtendsText(typeof(string)));
        }

        [Fact]
        public void GetImplementsText_TypeNull_ExceptionThrown()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, _typeService, _templateService, _tsContentParser, _metadataReaderFactory, generatorOptionsProvider, null);

            //act,assert
            Assert.Throws<ArgumentNullException>(() => tsContentGenerator.GetImplementsText(null));
        }

        [Fact]
        public void GetImplementsText_TypeNameConvertersNull_ExceptionThrown()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions { TypeNameConverters = null } };
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, _typeService, _templateService, _tsContentParser, _metadataReaderFactory, generatorOptionsProvider, null);

            //act,assert
            Assert.Throws<ArgumentNullException>(() => tsContentGenerator.GetImplementsText(typeof(string)));
        }

        [Fact]
        public void GetCustomBody_FilePathNull_ExceptionThrown()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, _typeService, _templateService, _tsContentParser, _metadataReaderFactory, generatorOptionsProvider, null);
            
            //act,assert
            Assert.Throws<ArgumentNullException>(() => tsContentGenerator.GetCustomBody(null, 0));
        }
        
        [Fact]
        public void GetCustomHead_FilePathNull_ExceptionThrown()
        {
            //arrange
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, _typeService, _templateService, _tsContentParser, _metadataReaderFactory, generatorOptionsProvider, null);
            
            //act,assert
            Assert.Throws<ArgumentNullException>(() => tsContentGenerator.GetCustomHead(null));
        }

        [Theory]
        [MemberData(nameof(GetMemberValueText_Data))]
        public void GetMemberValueText_MemberGiven_CorrectValueReturned(MemberInfo memberInfo, bool convertTypesToString, string expected)
        {
            //arrange
            ITypeService typeService = GetTypeService(convertTypesToString);
            var generatorOptionsProvider = new GeneratorOptionsProvider { GeneratorOptions = new GeneratorOptions() };
            var tsContentGenerator = new TsContentGenerator(_typeDependencyService, typeService, _templateService, _tsContentParser, _metadataReaderFactory, generatorOptionsProvider, null);
            
            //act
            string actual = tsContentGenerator.GetMemberValueText(memberInfo);

            //assert
            Assert.Equal(expected, actual);
        }
        
        public static IEnumerable<object[]> GetMemberValueText_Data = new[]
        {
            new object[] { typeof(GetMemberValueText_TestClass).GetField(nameof(GetMemberValueText_TestClass.IntFieldNoValue)), false, null },
            new object[] { typeof(GetMemberValueText_TestClass).GetProperty(nameof(GetMemberValueText_TestClass.IntPropertyNoValue)), false, null },
            new object[] { typeof(GetMemberValueText_TestClass).GetField(nameof(GetMemberValueText_TestClass.IntFieldValue)), false, 2 },
            new object[] { typeof(GetMemberValueText_TestClass).GetProperty(nameof(GetMemberValueText_TestClass.IntPropertyValue)), false, 2 },
            new object[] { typeof(GetMemberValueText_TestClass).GetField(nameof(GetMemberValueText_TestClass.StringFieldValue)), false, @"""value""" },
            new object[] { typeof(GetMemberValueText_TestClass).GetProperty(nameof(GetMemberValueText_TestClass.StringPropertyValue)), false, @"""value""" },
            new object[] { typeof(GetMemberValueText_TestClass).GetField(nameof(GetMemberValueText_TestClass.DateTimeFieldValue)), false, $@"new Date(""{GetMemberValueText_TestClass.TestDateTime}"")" },
            new object[] { typeof(GetMemberValueText_TestClass).GetProperty(nameof(GetMemberValueText_TestClass.DateTimePropertyValue)), false, $@"new Date(""{GetMemberValueText_TestClass.TestDateTime}"")" },
            new object[] { typeof(GetMemberValueText_TestClass).GetField(nameof(GetMemberValueText_TestClass.DateTimeOffsetFieldValue)), false, $@"new Date(""{GetMemberValueText_TestClass.TestDateTimeOffset}"")" },
            new object[] { typeof(GetMemberValueText_TestClass).GetProperty(nameof(GetMemberValueText_TestClass.DateTimeOffsetPropertyValue)), false, $@"new Date(""{GetMemberValueText_TestClass.TestDateTimeOffset}"")" },
            new object[] { typeof(GetMemberValueText_TestClass).GetField(nameof(GetMemberValueText_TestClass.TestClassFieldValue)), false, @"{""TestField"":2,""TestProperty"":""value""}" },
            new object[] { typeof(GetMemberValueText_TestClass).GetProperty(nameof(GetMemberValueText_TestClass.TestClassPropertyValue)), false, @"{""TestField"":2,""TestProperty"":""value""}" },
            new object[] { typeof(GetMemberValueText_TestClass).GetField(nameof(GetMemberValueText_TestClass.GuidFieldValue)), true, $@"""{GetMemberValueText_TestClass.TestGuid}""" },
            new object[] { typeof(GetMemberValueText_TestClass).GetProperty(nameof(GetMemberValueText_TestClass.GuidPropertyValue)), true, $@"""{GetMemberValueText_TestClass.TestGuid}""" },
            new object[] { typeof(GetMemberValueText_TestClass).GetField(nameof(GetMemberValueText_TestClass.DateTimeFieldValue)), true, $@"""{GetMemberValueText_TestClass.TestDateTime}""" },
            new object[] { typeof(GetMemberValueText_TestClass).GetProperty(nameof(GetMemberValueText_TestClass.DateTimePropertyValue)), true, $@"""{GetMemberValueText_TestClass.TestDateTime}""" },
            new object[] { typeof(GetMemberValueText_TestClass).GetField(nameof(GetMemberValueText_TestClass.DateTimeOffsetFieldValue)), true, $@"""{GetMemberValueText_TestClass.TestDateTimeOffset}""" },
            new object[] { typeof(GetMemberValueText_TestClass).GetProperty(nameof(GetMemberValueText_TestClass.DateTimeOffsetPropertyValue)), true, $@"""{GetMemberValueText_TestClass.TestDateTimeOffset}""" },
        };

        private class GetMemberValueText_TestClass
        {
            public static readonly Guid TestGuid = Guid.Parse("0ac1c484-d88b-4f5d-a789-440893b3a982");
            public static readonly DateTime TestDateTime = new DateTime(2000, 5, 10);
            public static readonly DateTimeOffset TestDateTimeOffset = new DateTimeOffset(TestDateTime);

            public class TestClass
            {
                public int TestField;
                public string TestProperty { get; set; }
            }
            
            public int IntFieldNoValue;
            public int IntPropertyNoValue { get; set; }
            
            public int IntFieldValue = 2;
            public int IntPropertyValue { get; set; } = 2;
            
            public string StringFieldValue = "value";
            public string StringPropertyValue { get; set; } = "value";
            
            public Guid GuidFieldValue = TestGuid;
            public Guid GuidPropertyValue { get; set; } = TestGuid;
            
            public DateTime DateTimeFieldValue = TestDateTime;
            public DateTime DateTimePropertyValue { get; set; } = TestDateTime;
            
            public DateTimeOffset DateTimeOffsetFieldValue = TestDateTimeOffset;
            public DateTimeOffset DateTimeOffsetPropertyValue { get; set; } = TestDateTimeOffset;

            public TestClass TestClassFieldValue = new TestClass { TestField = 2, TestProperty = "value" };
            public TestClass TestClassPropertyValue { get; set; } = new TestClass { TestField = 2, TestProperty = "value" };
        }

        private ITypeService GetTypeService(bool convertTypesToString)
        {
            var typeService = Substitute.For<ITypeService>();
            typeService.GetTsTypeName(Arg.Any<MemberInfo>()).Returns(args =>
            {
                var memberInfo = args.Arg<MemberInfo>();
                Type memberType;

                switch (memberInfo)
                {
                    case PropertyInfo propertyInfo:
                        memberType = propertyInfo.PropertyType;
                        break;
                    case FieldInfo fieldInfo:
                        memberType = fieldInfo.FieldType;
                        break;
                    default:
                        throw new Exception("memberInfo must be either PropertyInfo or FieldInfo");
                }

                if (convertTypesToString)
                {
                    switch (memberType.FullName)
                    {
                        case "System.Guid":
                        case "System.DateTime":
                        case "System.DateTimeOffset":
                        case "System.String":
                            return "string";
                        case "System.Int32":
                            return "number";
                        default:
                            return memberType.Name;
                    }
                }
                else
                {
                    switch (memberType.FullName)
                    {
                        case "System.String":
                            return "string";
                        case "System.Guid":
                            return "Guid";
                        case "System.DateTime":
                        case "System.DateTimeOffset":
                            return "Date";
                        case "System.Int32":
                            return "number";
                        default:
                            return memberType.Name;
                    }
                }
            });

            return typeService;
        }
    }
}
