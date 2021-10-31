using System.Collections.Generic;
using NSubstitute;
using TypeGen.Core.Generator.Services;
using TypeGen.Core.Storage;
using Xunit;

namespace TypeGen.Core.Test.Generator.Services
{
    public class TsContentParserTest
    {
        private readonly IFileSystem _fileSystem = Substitute.For<IFileSystem>();

        [Theory]
        [MemberData(nameof(GetTagContent_TestData))]
        public void GetTagContent_ParametersGiven_TagContentReturned(string fileContent, int indentSize, string[] tags, string expectedResult)
        {
            _fileSystem.FileExists(Arg.Any<string>()).Returns(true);
            _fileSystem.ReadFile(Arg.Any<string>()).Returns(fileContent);
            var tsContentParser = new TsContentParser(_fileSystem);

            string actualResult = tsContentParser.GetTagContent("some/file", indentSize, tags);
            
            Assert.Equal(expectedResult, actualResult);
        }

        public static IEnumerable<object[]> GetTagContent_TestData = new[]
        {
            new object[] { "asdf", 1, new [] { "custom" }, "" },
            new object[] { "//<custom-body>\r\nasdf//</custom-body>", 1, new [] { "custom" }, "" },
            new object[] { "//<custom-body>\r\nasdf//</custom-body>", 2, new [] { "custom-body" }, "asdf\r\n" },
            new object[] { "//<cusTom-bodY>\r\nasdf//</cuStOM-Body>", 2, new [] { "custom-body" }, "asdf\r\n" },
            new object[] { "//<custom-head>\r\nasdf\r\nzxcv\r\n//</custom-head>", 3, new [] { "custom-head" }, "asdf\r\nzxcv\r\n" },
            new object[] { "//<mytag>\r\nasdf//</mytag>\r\n  //<mytag>\r\nzxcv\r\n//</mytag>", 4, new [] { "mytag" }, "asdf\r\n    zxcv\r\n" },
            new object[] { "//<sometag>\r\nasdf//</sometag>  \r\n//<sometag>\r\nzxcv\r\nqwer\r\n//</sometag>", 3, new [] { "sometag" }, "asdf\r\n   zxcv\r\nqwer\r\n" },
        };
    }
}