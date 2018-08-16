using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using TypeGen.Core.Storage;
using TypeGen.Core.Utils;
using Xunit;

namespace TypeGen.Core.Test.Utils
{
    public class FileSystemUtilsTest
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

        [Theory]
        [InlineData(@"my\project\folder", @"my\project\folder\jkl.csproj")]
        [InlineData("my/project/folder", @"my/project/folder\jkl.csproj")]
        public void GetProjectFilePath_PathGiven_GetsProjectFile(string projectFolder, string expectedResult)
        {
            //arrange
            var files = new[] { "abc", "def.txt", ".ghi", "jkl.csproj" };

            var fileSystem = Substitute.For<IFileSystem>();
            fileSystem.GetDirectoryFiles(Arg.Any<string>()).Returns(files);

            //act, assert
            string filePath = FileSystemUtils.GetProjectFilePath(fileSystem, projectFolder);
            Assert.Equal(expectedResult, filePath);
        }

        [Theory]
        [InlineData(@"path\to\file.txt", @"path\file.txt", @"../file.txt")]
        [InlineData("path/to/file.txt", "path/file.txt", @"../file.txt")]
        [InlineData("path/to/some/nested/file.txt", "path/file.txt", @"../../../file.txt")]
        [InlineData("path/to/some/nested", "path/file.txt", @"../../file.txt")]
        [InlineData("path/to/some/nested/", "path/", @"../../../")]
        [InlineData(@"path\file.txt", "path/to/some/nested/file.txt", @"to/some/nested/file.txt")]
        [InlineData("path/files/file.txt", @"path\to\some\nested\file.txt", @"../to/some/nested/file.txt")]
        [InlineData("path/files/", @"path\to\some\nested\file.txt", @"../to/some/nested/file.txt")]
        public void GetPathDiff_Test(string pathFrom, string pathTo, string expectedResult)
        {
            string actualResult = FileSystemUtils.GetPathDiff(pathFrom, pathTo);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
