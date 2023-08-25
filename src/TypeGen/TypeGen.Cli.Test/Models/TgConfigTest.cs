using System.Linq;
using FluentAssertions;
using TypeGen.Cli.Models;
using TypeGen.Core;
using Xunit;
using TypeGen.Cli.Extensions;
using TypeGen.Core.Extensions;

namespace TypeGen.Cli.Test.Models
{
    public class TgConfigTest
    {
        [Fact]
        public void Normalize_GlobalPackagesFolderInExternalAssemblyPaths_GlobalPackagesFolderRemovedFromExternalAssemblyPaths()
        {
            var expectedResult = new[] { "some/path", @"C:\some\other\path" };
            var tgConfig = new TgConfig { ExternalAssemblyPaths = new[] { "some/path", "<global-packages>", @"C:\some\other\path" } };
            
            tgConfig.Normalize();
            
            Assert.Equal(expectedResult, tgConfig.ExternalAssemblyPaths);
        }

        [Fact]
        public void MergeWithDefaultParams_ParametersNull_DefaultValuesAssignedToParameters()
        {
            var tgConfig = new TgConfig();
            tgConfig.MergeWithDefaultParams();
            
            tgConfig.Assemblies.Should().BeEmpty();
            tgConfig.ExplicitPublicAccessor.Should().BeFalse();
            tgConfig.SingleQuotes.Should().BeFalse();
            tgConfig.AddFilesToProject.Should().BeFalse();
            tgConfig.TypeScriptFileExtension.Should().Be("ts");
            tgConfig.TabLength.Should().Be(4);
            tgConfig.FileNameConverters.Should().BeEquivalentTo("PascalCaseToKebabCaseConverter");
            tgConfig.TypeNameConverters.Should().BeEmpty();
            tgConfig.PropertyNameConverters.Should().BeEquivalentTo("PascalCaseToCamelCaseConverter");
            tgConfig.EnumValueNameConverters.Should().BeEmpty();
            tgConfig.ExternalAssemblyPaths.Should().BeEmpty();
            tgConfig.CreateIndexFile.Should().BeFalse();
            tgConfig.CsNullableTranslation.Should().BeEmpty();
            tgConfig.OutputPath.Should().BeEmpty();
            tgConfig.TypeBlacklist.Should().BeEmpty();
            tgConfig.TypeWhitelist.Should().BeEmpty();
        }

        [Fact]
        public void GetAssemblies_AssembliesIsNullOrEmpty_ReturnsAssemblyPath()
        {
            var tgConfig = new TgConfig { AssemblyPath = "some/path" };
            string[] actualResult = tgConfig.GetAssemblies();
            Assert.Equal(new[] { "some/path" }, actualResult);
        }
        
        [Fact]
        public void GetAssemblies_AssembliesIsNotNullOrEmpty_ReturnsAssemblies()
        {
            var assemblies = new[] { "my/assembly.dll", "other/assembly.dll" };
            var tgConfig = new TgConfig { AssemblyPath = "some/path", Assemblies = assemblies };
            
            string[] actualResult = tgConfig.GetAssemblies();
            
            Assert.Equal(assemblies, actualResult);
        }
    }
}