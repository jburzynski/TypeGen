using System;
using System.Threading.Tasks;
using TypeGen.FileContentTest.IgnoreBaseInterfaces.Entities;
using TypeGen.FileContentTest.TestingUtils;
using Xunit;
using Xunit.Abstractions;

namespace TypeGen.FileContentTest.IgnoreBaseInterfaces;

public class IgnoreBaseInterfacesTest : GenerationTestBase
{
    public IgnoreBaseInterfacesTest(ITestOutputHelper output) : base(output) { }

    [Theory]
    [InlineData(typeof(Test), "TypeGen.FileContentTest.IgnoreBaseInterfaces.Expected.test.ts")]
    public async Task TestIgnoreBaseInterfaces(Type type, string expectedLocation)
    {
        await TestFromAssembly(type, expectedLocation);
    }
}