using System.Linq;
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
            
            Assert.Equal(new string[0], tgConfig.Assemblies);
            Assert.Equal(false, tgConfig.ExplicitPublicAccessor);
            Assert.Equal(false, tgConfig.SingleQuotes);
            Assert.Equal(false, tgConfig.AddFilesToProject);
            Assert.Equal("ts", tgConfig.TypeScriptFileExtension);
            Assert.Equal(4, tgConfig.TabLength);
            Assert.Equal(new [] { "PascalCaseToKebabCaseConverter" }, tgConfig.FileNameConverters);
            Assert.Equal(new string[0], tgConfig.TypeNameConverters);
            Assert.Equal(new[] { "PascalCaseToCamelCaseConverter" }, tgConfig.PropertyNameConverters);
            Assert.Equal(new string[0], tgConfig.EnumValueNameConverters);
            Assert.Equal(new string[0], tgConfig.ExternalAssemblyPaths);
            Assert.Equal(false, tgConfig.CreateIndexFile);
            Assert.Equal(false, tgConfig.StrictNullChecks);
            Assert.Equal("", tgConfig.CsNullableTranslation);
            Assert.Equal("", tgConfig.OutputPath);
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