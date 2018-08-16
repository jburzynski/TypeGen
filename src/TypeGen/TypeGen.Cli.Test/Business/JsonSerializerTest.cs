using System.Collections.Generic;
using System.Runtime.Serialization;
using NSubstitute;
using TypeGen.Cli.Business;
using TypeGen.Core.Storage;
using Xunit;

namespace TypeGen.Cli.Test.Business
{
    public class JsonSerializerTest
    {
        private const string JsonContent = "{\"stringField\":\"init string\",\"IntProperty\":100,\"stringArray\":[\"a\",\"b\",\"c\"],\"boolField\":true,\"nullableBoolField1\":null,\"nullableBoolField2\":false}";
        
        private readonly IFileSystem _fileSystem = Substitute.For<IFileSystem>();
        
        [Fact]
        public void DeserializeFromFile_JsonInFileContent_JsonFileContentDeserialized()
        {
            _fileSystem.ReadFile(Arg.Any<string>()).Returns(JsonContent);
            var jsonSerializer = new JsonSerializer(_fileSystem);

            var actualResult = jsonSerializer.DeserializeFromFile<TestClass>("filepath");
            
            Assert.Equal("init string", actualResult.stringField);
            Assert.Equal(100, actualResult.IntProperty);
            Assert.Equal(new[] { "a", "b", "c" }, actualResult.stringArray);
            Assert.Equal(true, actualResult.boolField);
            Assert.Equal(null, actualResult.nullableBoolField1);
            Assert.Equal(false, actualResult.nullableBoolField2);
        }
        
        [Fact]
        public void Deserialize_JsonStringPassed_JsonStringDeserialized()
        {
            var jsonSerializer = new JsonSerializer(_fileSystem);
            var actualResult = jsonSerializer.Deserialize<TestClass>(JsonContent);
            
            Assert.Equal("init string", actualResult.stringField);
            Assert.Equal(100, actualResult.IntProperty);
            Assert.Equal(new[] { "a", "b", "c" }, actualResult.stringArray);
            Assert.Equal(true, actualResult.boolField);
            Assert.Equal(null, actualResult.nullableBoolField1);
            Assert.Equal(false, actualResult.nullableBoolField2);
        }
        
        [DataContract]
        private class TestClass
        {
            [DataMember(Name = "stringField")]
            public string stringField;
            
            [DataMember(Name = "IntProperty")]
            public int IntProperty { get; set; }
            
            [DataMember(Name = "stringArray")]
            public string[] stringArray { get; set; }
            
            [DataMember(Name = "boolField")]
            public bool boolField;
            
            [DataMember(Name = "nullableBoolField1")]
            public bool? nullableBoolField1;
            
            [DataMember(Name = "nullableBoolField2")]
            public bool? nullableBoolField2;
        }
    }
}