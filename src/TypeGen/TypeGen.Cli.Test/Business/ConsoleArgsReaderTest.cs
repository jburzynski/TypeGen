using System.Collections.Generic;
using System.Linq;
using TypeGen.Cli.Business;
using Xunit;

namespace TypeGen.Cli.Test.Business
{
    public class ConsoleArgsReaderTest
    {
        [Theory]
        [MemberData(nameof(ContainsGetCwdCommand_TestData))]
        public void ContainsGetCwdCommand_Test(string[] args, bool expectedResult)
        {
            bool actualResult = ConsoleArgsReader.ContainsGetCwdCommand(args);
            Assert.Equal(expectedResult, actualResult);
        }

        public static IEnumerable<object[]> ContainsGetCwdCommand_TestData = new[]
        {
            new object[] { new[] { "asdf", "getcwd", "d" }, true },
            new object[] { new[] { "asdf", "GETCWD", "d" }, true },
            new object[] { new[] { "asdf", "GeTCwD", "d" }, true },
            new object[] { new[] { "getcwd" }, true },
            new object[] { new[] { "asdf", "d" }, false },
            new object[] { new string[] {}, false }
        };
        
        [Theory]
        [MemberData(nameof(ContainsGenerateCommand_TestData))]
        public void ContainsGenerateCommand_Test(string[] args, bool expectedResult)
        {
            bool actualResult = ConsoleArgsReader.ContainsGenerateCommand(args);
            Assert.Equal(expectedResult, actualResult);
        }

        public static IEnumerable<object[]> ContainsGenerateCommand_TestData = new[]
        {
            new object[] { new[] { "asdf", "generate", "d" }, true },
            new object[] { new[] { "asdf", "GENERATE", "d" }, true },
            new object[] { new[] { "asdf", "GeNeRAtE", "d" }, true },
            new object[] { new[] { "generate" }, true },
            new object[] { new[] { "asdf", "d" }, false },
            new object[] { new string[] {}, false }
        };
        
        [Theory]
        [MemberData(nameof(ContainsAnyCommand_TestData))]
        public void ContainsAnyCommand_Test(string[] args, bool expectedResult)
        {
            bool actualResult = ConsoleArgsReader.ContainsAnyCommand(args);
            Assert.Equal(expectedResult, actualResult);
        }

        public static IEnumerable<object[]> ContainsAnyCommand_TestData = new[]
        {
            new object[] { new[] { "asdf", "generate", "d" }, true },
            new object[] { new[] { "asdf", "GetCWD", "d" }, true },
            new object[] { new[] { "asdf", "GeNeRAtE", "gETcwd" }, true },
            new object[] { new[] { "generate" }, true },
            new object[] { new[] { "generatea" }, false },
            new object[] { new[] { "getcwd" }, true },
            new object[] { new[] { "agetcwd" }, false },
            new object[] { new[] { "agenerate" }, false },
            new object[] { new[] { "getcwda" }, false },
            new object[] { new[] { "-getcwd" }, false },
            new object[] { new[] { "-generate" }, false },
            new object[] { new[] { "asdf", "d" }, false },
            new object[] { new string[] {}, false }
        };
        
        [Theory]
        [MemberData(nameof(ContainsHelpOption_TestData))]
        public void ContainsHelpOption_Test(string[] args, bool expectedResult)
        {
            bool actualResult = ConsoleArgsReader.ContainsHelpOption(args);
            Assert.Equal(expectedResult, actualResult);
        }

        public static IEnumerable<object[]> ContainsHelpOption_TestData = new[]
        {
            new object[] { new[] { "asdf", "-h", "d" }, true },
            new object[] { new[] { "asdf", "--help", "d" }, true },
            new object[] { new[] { "asdf", "-H", "d" }, true },
            new object[] { new[] { "asdf", "--HELP", "d" }, true },
            new object[] { new[] { "asdf", "--HeLp", "d" }, true },
            new object[] { new[] { "-h" }, true },
            new object[] { new[] { "--help" }, true },
            new object[] { new[] { "asdf", "d" }, false },
            new object[] { new string[] {}, false }
        };
        
        [Theory]
        [MemberData(nameof(ContainsProjectFolderOption_TestData))]
        public void ContainsProjectFolderOption_Test(string[] args, bool expectedResult)
        {
            bool actualResult = ConsoleArgsReader.ContainsProjectFolderOption(args);
            Assert.Equal(expectedResult, actualResult);
        }

        public static IEnumerable<object[]> ContainsProjectFolderOption_TestData = new[]
        {
            new object[] { new[] { "asdf", "-p", "d" }, true },
            new object[] { new[] { "asdf", "--project-folder", "d" }, true },
            new object[] { new[] { "asdf", "-P", "d" }, true },
            new object[] { new[] { "asdf", "--PROJECT-FOLDER", "d" }, true },
            new object[] { new[] { "asdf", "--PrOjECt-FoldER", "d" }, true },
            new object[] { new[] { "-p" }, true },
            new object[] { new[] { "--project-folder" }, true },
            new object[] { new[] { "asdf", "d" }, false },
            new object[] { new string[] {}, false }
        };

        [Theory]
        [MemberData(nameof(ContainsOutputFolderOption_TestData))]
        public void ContainsOutputFolderOption_Test(string[] args, bool expectedResult)
        {
            bool actualResult = ConsoleArgsReader.ContainsOutputOption(args);
            Assert.Equal(expectedResult, actualResult);
        }

        public static IEnumerable<object[]> ContainsOutputFolderOption_TestData = new[]
        {
            new object[] { new[] { "asdf", "-o", "d" }, true },
            new object[] { new[] { "asdf", "--output-folder", "d" }, true },
            new object[] { new[] { "asdf", "-O", "d" }, true },
            new object[] { new[] { "asdf", "--OUTPUT-FOLDER", "d" }, true },
            new object[] { new[] { "asdf", "--OuTpUt-FoldER", "d" }, true },
            new object[] { new[] { "-o" }, true },
            new object[] { new[] { "--output-folder" }, true },
            new object[] { new[] { "asdf", "d" }, false },
            new object[] { new string[] {}, false }
        };

        [Theory]
        [MemberData(nameof(ContainsVerboseOption_TestData))]
        public void ContainsVerboseOption_Test(string[] args, bool expectedResult)
        {
            bool actualResult = ConsoleArgsReader.ContainsVerboseOption(args);
            Assert.Equal(expectedResult, actualResult);
        }

        public static IEnumerable<object[]> ContainsVerboseOption_TestData = new[]
        {
            new object[] { new[] { "asdf", "-v", "d" }, true },
            new object[] { new[] { "asdf", "--verbose", "d" }, true },
            new object[] { new[] { "asdf", "-V", "d" }, true },
            new object[] { new[] { "asdf", "--VERBOSE", "d" }, true },
            new object[] { new[] { "asdf", "--VeRbOSE", "d" }, true },
            new object[] { new[] { "-v" }, true },
            new object[] { new[] { "--verbose" }, true },
            new object[] { new[] { "asdf", "d" }, false },
            new object[] { new string[] {}, false }
        };
        
        [Theory]
        [MemberData(nameof(GetProjectFolders_TestData))]
        public void GetProjectFolders_Test(string[] args, IEnumerable<string> expectedResult)
        {
            IEnumerable<string> actualResult = ConsoleArgsReader.GetProjectFolders(args);
            Assert.Equal(expectedResult, actualResult);
        }
        
        public static IEnumerable<object[]> GetProjectFolders_TestData = new[]
        {
            new object[] { new[] { "-p", "asdf", "project/folder" }, new [] { "asdf" } },
            new object[] { new[] { "--project-folder", "asdf", @"C:\project\folder" }, new [] { "asdf" } },
            new object[] { new[] { "-p", "asdf|qwer", "--config-path", "zxcv" }, new [] { "asdf", "qwer" } },
            new object[] { new[] { "--project-folder", @"D:\my\folder|some/other/folder|that/folder", "--config-path", "zxcv|qwer" }, new [] { @"D:\my\folder", "some/other/folder", "that/folder" } },
            new object[] { new[] { "asdf" }, new string[] {} },
            new object[] { new string[] {}, new string[] {} }
        };
        
        [Fact]
        public void GetProjectFolders_ParameterPresentAndNoPathsSpecified_ExceptionThrown()
        {
            var args = new[] { "--project-folder" };
            Assert.Throws<CliException>(() => ConsoleArgsReader.GetProjectFolders(args));
        }

        [Theory]
        [MemberData(nameof(GetOutputFolder_TestData))]
        public void GetOutputFolder_Test(string[] args, string expectedResult)
        {
            string actualResult = ConsoleArgsReader.GetOutputFolder(args);
            Assert.Equal(expectedResult, actualResult);
        }

        public static IEnumerable<object[]> GetOutputFolder_TestData = new[]
        {
            new object[] { new[] { "-o", "asdf", "project/folder" },  "asdf" },
            new object[] { new[] { "--output-folder", "asdf", @"C:\project\folder" }, "asdf" },
            new object[] { new[] { "-o", "asdf|qwer", "--config-path", "zxcv" }, "asdf" },
            new object[] { new[] { "--output-folder", @"D:\my\folder|some/other/folder|that/folder", "--config-path", "zxcv|qwer" }, @"D:\my\folder" },
            new object[] { new[] { "asdf" }, null },
            new object[] { new string[] {}, null }
        };

        [Fact]
        public void GetOutputFolder_ParameterPresentAndNoPathsSpecified_ExceptionThrown()
        {
            var args = new[] { "--output-folder" };
            Assert.Throws<CliException>(() => ConsoleArgsReader.GetOutputFolder(args));
        }

        [Theory]
        [MemberData(nameof(GetConfigPaths_TestData))]
        public void GetConfigPaths_Test(string[] args, IEnumerable<string> expectedResult)
        {
            IEnumerable<string> actualResult = ConsoleArgsReader.GetConfigPaths(args);
            Assert.Equal(expectedResult, actualResult);
        }
        
        public static IEnumerable<object[]> GetConfigPaths_TestData = new[]
        {
            new object[] { new[] { "asdf", "-c", "zxcv" }, new [] { "zxcv" } },
            new object[] { new[] { "asdf", "--config-path", "zxcv" }, new [] { "zxcv" } },
            new object[] { new[] { "asdf", "-c", "zxcv", "qwer" }, new [] { "zxcv" } },
            new object[] { new[] { "asdf", "--config-path", "zxcv|qwer" }, new [] { "zxcv", "qwer" } },
            new object[] { new[] { "asdf", "-c", @"my\path|C:\Program Files\path" }, new [] { @"my\path", @"C:\Program Files\path" } },
            new object[] { new[] { "asdf", "--config-path", @"my\path|C:\Program Files\path|other/path" }, new [] { @"my\path", @"C:\Program Files\path", "other/path" } },
            new object[] { new[] { "asdf" }, new string[] {} },
            new object[] { new string[] {}, new string[] {} }
        };

        [Fact]
        public void GetConfigPaths_ParameterPresentAndNoPathsSpecified_ExceptionThrown()
        {
            var args = new[] { "--config-path" };
            Assert.Throws<CliException>(() => ConsoleArgsReader.GetConfigPaths(args));
        }
    }
}