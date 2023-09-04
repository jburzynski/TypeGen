using System;
using System.Threading.Tasks;
using TypeGen.FileContentTest.CircularGenericConstraint.Entities;
using TypeGen.FileContentTest.TestingUtils;
using Xunit;

namespace TypeGen.FileContentTest.CircularGenericConstraint
{
    public class CircularGenericConstraintTest : GenerationTestBase
    {
        /// <summary>
        /// Looks into generating classes and interfaces with circular type constraints
        /// </summary>
        /// <param name="type"></param>
        /// <param name="expectedLocation"></param>
        /// <returns></returns>
        [Theory]
        [InlineData(typeof(RecursiveConstraintClass<>), @"TypeGen.FileContentTest.CircularGenericConstraint.Expected.RecursiveConstraintClass.ts")]
        [InlineData(typeof(IRecursiveConstraintInterface<>), @"TypeGen.FileContentTest.CircularGenericConstraint.Expected.IRecursiveConstraintInterface.ts")]
        [InlineData(typeof(IRecursiveConstraintInterfaceWithClassConstraint<>), @"TypeGen.FileContentTest.CircularGenericConstraint.Expected.IRecursiveConstraintInterfaceWithClassConstraint.ts")]
        [InlineData(typeof(IRecursiveConstraintInterfaceWithStructConstraint<>), @"TypeGen.FileContentTest.CircularGenericConstraint.Expected.IRecursiveConstraintInterfaceWithStructConstraint.ts")]
        [InlineData(typeof(IMultipleConstraintInterface<>), @"TypeGen.FileContentTest.CircularGenericConstraint.Expected.IMultipleConstraintInterface.ts")]
        [InlineData(typeof(ICicrularConstraintInterface<,>), @"TypeGen.FileContentTest.CircularGenericConstraint.Expected.ICicrularConstraintInterface.ts")]
        public async Task GeneratesCorrectly(Type type, string expectedLocation)
        {
            await TestFromAssembly(type, expectedLocation);
        }
    }
}
