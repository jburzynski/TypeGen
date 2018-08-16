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

        [Theory]
        [MemberData(nameof(GetConfigPaths_TestData))]
        public void GetConfigPaths_Test(string[] args, IEnumerable<string> expectedResult)
        {
            IEnumerable<string> actualResult = _consoleArgsReader.GetConfigPaths(args);
            Assert.Equal(expectedResult, actualResult);
        }
        
        public static IEnumerable<object[]> GetConfigPaths_TestData = new[]
        {
            new object[] { new[] { "asdf", "-Config-Path", "zxcv" }, new [] { "zxcv" } },
            new object[] { new[] { "asdf", "-CoNfIg-PaTh", "zxcv" }, new [] { "zxcv" } },
            new object[] { new[] { "asdf", "-Config-Path", "zxcv", "qwer" }, new [] { "zxcv" } },
            new object[] { new[] { "asdf", "-Config-Path", "zxcv|qwer" }, new [] { "zxcv", "qwer" } },
            new object[] { new[] { "asdf", "-Config-Path", @"my\path|C:\Program Files\path" }, new [] { @"my\path", @"C:\Program Files\path" } },
            new object[] { new[] { "asdf", "-Config-Path", @"my\path|C:\Program Files\path|other/path" }, new [] { @"my\path", @"C:\Program Files\path", "other/path" } },
            new object[] { new[] { "asdf" }, new string[] {} }
        };

        [Fact]
        public void GetConfigPaths_ParameterPresentAndNoPathsSpecified_ExceptionThrown()
        {
            var args = new[] { "-Config-Path" };
            Assert.Throws<CliException>(() => _consoleArgsReader.GetConfigPaths(args));
        }

        [Theory]
        [MemberData(nameof(GetProjectFolders_TestData))]
        public void GetProjectFolders_Test(string[] args, IEnumerable<string> expectedResult)
        {
            IEnumerable<string> actualResult = _consoleArgsReader.GetProjectFolders(args);
            Assert.Equal(expectedResult, actualResult);
        }
        
        public static IEnumerable<object[]> GetProjectFolders_TestData = new[]
        {
            new object[] { new[] { "asdf", "project/folder" }, new [] { "asdf" } },
            new object[] { new[] { "asdf", @"C:\project\folder" }, new [] { "asdf" } },
            new object[] { new[] { "asdf|qwer", "-Config-Path", "zxcv" }, new [] { "asdf", "qwer" } },
            new object[] { new[] { @"D:\my\folder|some/other/folder|that/folder", "-Config-Path", "zxcv|qwer" }, new [] { @"D:\my\folder", "some/other/folder", "that/folder" } },
            new object[] { new string[] { }, new string[] { } }
        };
    }
}