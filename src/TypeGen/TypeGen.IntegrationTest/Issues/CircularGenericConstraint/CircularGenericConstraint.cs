using System;
using System.Threading.Tasks;
using TypeGen.IntegrationTest.Generator;
using TypeGen.IntegrationTest.Issues.CircularGenericConstraint.TestClasses;
using Xunit;

namespace TypeGen.IntegrationTest.Issues.CircularGenericConstraint
{
    public class CircularGenericConstraint
    {
        /// <summary>
        /// Looks into generating classes and interfaces with circular type constraints
        /// </summary>
        /// <param name="type"></param>
        /// <param name="expectedLocation"></param>
        /// <returns></returns>
        [Theory]
        [InlineData(typeof(RecursiveConstraintClass<>), @"TypeGen.IntegrationTest.Issues.CircularGenericConstraint.Expected.RecursiveConstraintClass.ts")]
        [InlineData(typeof(IRecursiveConstraintInterface<>), @"TypeGen.IntegrationTest.Issues.CircularGenericConstraint.Expected.IRecursiveConstraintInterface.ts")]
        [InlineData(typeof(IRecursiveConstraintInterfaceWithClassConstraint<>), @"TypeGen.IntegrationTest.Issues.CircularGenericConstraint.Expected.IRecursiveConstraintInterfaceWithClassConstraint.ts")]
        [InlineData(typeof(IRecursiveConstraintInterfaceWithStructConstraint<>), @"TypeGen.IntegrationTest.Issues.CircularGenericConstraint.Expected.IRecursiveConstraintInterfaceWithStructConstraint.ts")]
        [InlineData(typeof(IMultipleConstraintInterface<>), @"TypeGen.IntegrationTest.Issues.CircularGenericConstraint.Expected.IMultipleConstraintInterface.ts")]
        [InlineData(typeof(ICicrularConstraintInterface<,>), @"TypeGen.IntegrationTest.Issues.CircularGenericConstraint.Expected.ICicrularConstraintInterface.ts")]
        public async Task GeneratesCorrectly(Type type, string expectedLocation)
        {
            await new GeneratorTest().TestGenerate(type, expectedLocation);
        }
    }
}
