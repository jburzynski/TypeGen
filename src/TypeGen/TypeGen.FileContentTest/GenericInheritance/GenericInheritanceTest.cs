#nullable enable
using System;
using System.Threading.Tasks;
using TypeGen.FileContentTest.GenericInheritance.Entities;
using TypeGen.FileContentTest.TestingUtils;
using Xunit;
using Xunit.Abstractions;

namespace TypeGen.FileContentTest.GenericInheritance;

public class GenericInheritanceTest : GenerationTestBase
{
    public GenericInheritanceTest(ITestOutputHelper output) : base(output) { }

    [Theory]
    [InlineData(typeof(GetCustomersResponseDto), "TypeGen.FileContentTest.GenericInheritance.Expected.get-customers-response-dto.ts")]
    public async Task TestGenericInheritance(Type type, string expectedLocation)
    {
        await TestFromAssembly(type, expectedLocation);
    }
}