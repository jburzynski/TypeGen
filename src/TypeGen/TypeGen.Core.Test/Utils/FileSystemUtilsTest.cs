using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using TypeGen.Core.Storage;
using TypeGen.Core.Utils;
using Xunit;

namespace TypeGen.Core.Test.Utils
{
    public class FileSystemUtilsTest : TestBase
    {
        [Theory]
        [InlineData("some/test/path")]
        [InlineData(@"some\test\path")]
        [InlineData(@"some\test/path")]
        public void SplitPathSeparator_PathGiven_PathSplit(string path)
        {
            string[] splitPath = FileSystemUtils.SplitPathSeperator(path);

            Assert.Equal(new[] { "some", "test", "path" }, splitPath);
        }

        [Theory]
        [InlineData("some/test/file.ext")]
        [InlineData(@"some\test\file.ext")]
        [InlineData(@"some\test/file.ext")]
        public void GetFileNameFromPath_PathGiven_FileNameReturned(string path)
        {
            string fileName = FileSystemUtils.GetFileNameFromPath(path);

            Assert.Equal("file.ext", fileName);
        }

        [Fact]
        public void GetProjectFilePath_PathGiven_GetsProjectFile()
        {
            //arrange
            const string projectFolder = @"my\project\folder";
            var files = new[] { "abc", "def.txt", ".ghi", "jkl.csproj" };

            var fileSystem = GetInstance<IFileSystem>();
            fileSystem.GetDirectoryFiles(Arg.Any<string>()).Returns(files);

            //act, assert
            string filePath = FileSystemUtils.GetProjectFilePath(fileSystem, projectFolder);

            Assert.Equal(@"my\project\folder\jkl.csproj", filePath);
        }
    }
}
