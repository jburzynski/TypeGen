using System;
using System.Threading.Tasks;
using TypeGen.IntegrationTest.CircularGenericConstraint.Entities;
using TypeGen.IntegrationTest.CommonCases;
using TypeGen.IntegrationTest.TestingUtils;
using Xunit;

namespace TypeGen.IntegrationTest.CircularGenericConstraint
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
        [InlineData(typeof(RecursiveConstraintClass<>), @"TypeGen.IntegrationTest.CircularGenericConstraint.Expected.RecursiveConstraintClass.ts")]
        [InlineData(typeof(IRecursiveConstraintInterface<>), @"TypeGen.IntegrationTest.CircularGenericConstraint.Expected.IRecursiveConstraintInterface.ts")]
        [InlineData(typeof(IRecursiveConstraintInterfaceWithClassConstraint<>), @"TypeGen.IntegrationTest.CircularGenericConstraint.Expected.IRecursiveConstraintInterfaceWithClassConstraint.ts")]
        [InlineData(typeof(IRecursiveConstraintInterfaceWithStructConstraint<>), @"TypeGen.IntegrationTest.CircularGenericConstraint.Expected.IRecursiveConstraintInterfaceWithStructConstraint.ts")]
        [InlineData(typeof(IMultipleConstraintInterface<>), @"TypeGen.IntegrationTest.CircularGenericConstraint.Expected.IMultipleConstraintInterface.ts")]
        [InlineData(typeof(ICicrularConstraintInterface<,>), @"TypeGen.IntegrationTest.CircularGenericConstraint.Expected.ICicrularConstraintInterface.ts")]
        public async Task GeneratesCorrectly(Type type, string expectedLocation)
        {
            await TestFromAssembly(type, expectedLocation);
        }
    }
}
