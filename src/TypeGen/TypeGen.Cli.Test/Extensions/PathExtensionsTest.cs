using NSubstitute;
using TypeGen.Cli.Extensions;
using TypeGen.Core.Storage;
using Xunit;

namespace TypeGen.Cli.Test.Extensions
{
    public class PathExtensionsTest
    {
        private readonly IFileSystem _fileSystem = Substitute.For<IFileSystem>();

        [Theory]
        [InlineData("my/path", @"C:\projects", @"C:\projects\my/path")]
        [InlineData(@"my\very\nested\path", @"Z:\very nested\path", @"Z:\very nested\path\my\very\nested\path")]
        [InlineData(@"C:\Program Files\program.exe", @"C:\path", @"C:\Program Files\program.exe")]
        public void ToWindowsAbsolutePath_PathGiven_AbsolutePathReturned(string path, string currentWorkingDirectory, string expectedResult)
        {
            if (!System.OperatingSystem.IsWindows()) return;

            _fileSystem.GetCurrentDirectory().Returns(currentWorkingDirectory);
            string actualResult = path.ToAbsolutePath(_fileSystem);
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("my\\path", @"/projects", @"/projects/my\path")]
        [InlineData(@"my/very/nested/path", @"/very nested/path", @"/very nested/path/my/very/nested/path")]
        [InlineData(@"/etc/program.dll", @"/path", @"/etc/program.dll")]
        public void ToUnixAbsolutePath_PathGiven_AbsolutePathReturned(string path, string currentWorkingDirectory, string expectedResult)
        {
            if (!System.OperatingSystem.IsMacOS() && !System.OperatingSystem.IsLinux()) return;

            _fileSystem.GetCurrentDirectory().Returns(currentWorkingDirectory);
            string actualResult = path.ToAbsolutePath(_fileSystem);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}