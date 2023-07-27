using System;
using System.Threading.Tasks;
using TypeGen.IntegrationTest.IgnoreBaseInterfaces.Entities;
using TypeGen.IntegrationTest.TestingUtils;
using Xunit;

namespace TypeGen.IntegrationTest.IgnoreBaseInterfaces;

public class IgnoreBaseInterfacesTest : GenerationTestBase
{
    [Theory]
    [InlineData(typeof(Test), "TypeGen.IntegrationTest.IgnoreBaseInterfaces.Expected.test.ts")]
    public async Task TestIgnoreBaseInterfaces(Type type, string expectedLocation)
    {
        await TestFromAssembly(type, expectedLocation);
    }
}