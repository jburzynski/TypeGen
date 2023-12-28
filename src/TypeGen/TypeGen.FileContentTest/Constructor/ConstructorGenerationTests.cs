using System;
using System.Threading.Tasks;
using TypeGen.FileContentTest.TestingUtils;
using Xunit;

namespace TypeGen.FileContentTest.Constructor;

public class ConstructorGenerationTests: GenerationTestBase
{
    [Theory]
    [InlineData(typeof(Entities.ClassWithConstructorMatchingMembers), "TypeGen.FileContentTest.Constructor.Expected.test-constructors.ts")]
    [InlineData(typeof(Entities.ClassWithConstructorMatchingMembersMismatch), "TypeGen.FileContentTest.Constructor.Expected.test-constructors-mismatch.ts")]
    [InlineData(typeof(Entities.ClassWithConstructorMatchingMembersAndEmpty), "TypeGen.FileContentTest.Constructor.Expected.test-empty-constructors.ts")]
    [InlineData(typeof(Entities.MyRecord), "TypeGen.FileContentTest.Constructor.Expected.test-record-constructors.ts")]
    public async Task TestConstructor(Type type, string expectedLocation)
    {
        await TestFromAssembly(type, expectedLocation);
    }
}