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
        [Fact]
        public void SplitPathSeparator_ForwardSlash_Split()
        {
            const string path = "some/test/path";
            string[] splitPath = FileSystemUtils.SplitPathSeperator(path);

            Assert.Equal(new[] { "some", "test", "path" }, splitPath);
        }

        [Fact]
        public void SplitPathSeparator_Backslash_Split()
        {
            const string path = @"some\test\path";
            string[] splitPath = FileSystemUtils.SplitPathSeperator(path);

            Assert.Equal(new[] { "some", "test", "path" }, splitPath);
        }

        [Fact]
        public void SplitPathSeparator_MixedSlashes_Split()
        {
            const string path = @"some\test/path";
            string[] splitPath = FileSystemUtils.SplitPathSeperator(path);

            Assert.Equal(new[] { "some", "test", "path" }, splitPath);
        }

        [Fact]
        public void GetFileNameFromPath_ForwardSlash_Return()
        {
            const string path = "some/test/file.ext";
            string fileName = FileSystemUtils.GetFileNameFromPath(path);

            Assert.Equal("file.ext", fileName);
        }

        [Fact]
        public void GetFileNameFromPath_Backslash_Return()
        {
            const string path = @"some\test\file.ext";
            string fileName = FileSystemUtils.GetFileNameFromPath(path);

            Assert.Equal("file.ext", fileName);
        }

        [Fact]
        public void GetFileNameFromPath_MixedSlashes_Return()
        {
            const string path = @"some\test/file.ext";
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
