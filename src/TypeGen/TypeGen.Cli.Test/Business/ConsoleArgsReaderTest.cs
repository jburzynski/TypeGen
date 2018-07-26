using System.Collections.Generic;
using TypeGen.Cli.Business;
using Xunit;

namespace TypeGen.Cli.Test.Business
{
    public class ConsoleArgsReaderTest
    {
        private readonly IConsoleArgsReader _consoleArgsReader = new ConsoleArgsReader();
        
        [Theory]
        [MemberData(nameof(ContainsHelpParam_TestData))]
        public void ContainsHelpParam_Test(string[] args, bool expectedResult)
        {
            bool actualResult = _consoleArgsReader.ContainsHelpParam(args);
            Assert.Equal(expectedResult, actualResult);
        }

        public static IEnumerable<object[]> ContainsHelpParam_TestData = new[]
        {
            new object[] { new[] { "asdf", "-h", "d" }, true },
            new object[] { new[] { "asdf", "-help", "d" }, true },
            new object[] { new[] { "asdf", "-H", "d" }, true },
            new object[] { new[] { "asdf", "-HELP", "d" }, true },
            new object[] { new[] { "asdf", "-HeLp", "d" }, true },
            new object[] { new[] { "-h" }, true },
            new object[] { new[] { "-help" }, true },
            new object[] { new[] { "asdf", "d" }, false },
            new object[] { new string[] {}, false }
        };
        
        [Theory]
        [MemberData(nameof(ContainsGetCwdParam_TestData))]
        public void ContainsGetCwdParam_Test(string[] args, bool expectedResult)
        {
            bool actualResult = _consoleArgsReader.ContainsGetCwdParam(args);
            Assert.Equal(expectedResult, actualResult);
        }

        public static IEnumerable<object[]> ContainsGetCwdParam_TestData = new[]
        {
            new object[] { new[] { "asdf", "get-cwd", "d" }, true },
            new object[] { new[] { "asdf", "GET-CWD", "d" }, true },
            new object[] { new[] { "asdf", "GeT-CwD", "d" }, true },
            new object[] { new[] { "get-cwd" }, true },
            new object[] { new[] { "asdf", "d" }, false },
            new object[] { new string[] {}, false }
        };
        
        [Theory]
        [MemberData(nameof(ContainsVerboseParam_TestData))]
        public void ContainsVerboseParam_Test(string[] args, bool expectedResult)
        {
            bool actualResult = _consoleArgsReader.ContainsVerboseParam(args);
            Assert.Equal(expectedResult, actualResult);
        }

        public static IEnumerable<object[]> ContainsVerboseParam_TestData = new[]
        {
            new object[] { new[] { "asdf", "-v", "d" }, true },
            new object[] { new[] { "asdf", "-verbose", "d" }, true },
            new object[] { new[] { "asdf", "-V", "d" }, true },
            new object[] { new[] { "asdf", "-VERBOSE", "d" }, true },
            new object[] { new[] { "asdf", "-VeRbOSE", "d" }, true },
            new object[] { new[] { "-v" }, true },
            new object[] { new[] { "-verbose" }, true },
            new object[] { new[] { "asdf", "d" }, false },
            new object[] { new string[] {}, false }
        };
    }
}