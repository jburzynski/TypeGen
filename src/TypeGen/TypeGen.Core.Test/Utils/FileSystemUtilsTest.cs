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
            string[] splitPath = FileSystemUtils.SplitPathSeparator(path);
            Assert.Equal(new[] { "some", "test", "path" }, splitPath);
        }

        [Theory]
        [InlineData(@"path\to\file.txt", @"path\file.txt", @"../file.txt/")]
        [InlineData("path/to/file.txt", "path/file.txt", @"../file.txt/")]
        [InlineData("path/to/some/nested/file.txt", "path/file.txt", @"../../../file.txt/")]
        [InlineData("path/to/some/nested", "path/file.txt", @"../../file.txt/")]
        [InlineData("path/to/some/nested/", "path/", @"../../../")]
        [InlineData(@"path\file.txt", "path/to/some/nested/file.txt", @"to/some/nested/file.txt/")]
        [InlineData("path/files/file.txt", @"path\to\some\nested\file.txt", @"../to/some/nested/file.txt/")]
        [InlineData("path/files/", @"path\to\some\nested\file.txt", @"../to/some/nested/file.txt/")]
        public void GetPathDiff_Test(string pathFrom, string pathTo, string expectedResult)
        {
            string actualResult = FileSystemUtils.GetPathDiff(pathFrom, pathTo);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
