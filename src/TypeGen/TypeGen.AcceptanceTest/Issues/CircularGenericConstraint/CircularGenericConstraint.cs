using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TypeGen.AcceptanceTest.GeneratorTestingUtils;
using TypeGen.Core.Test.GeneratorTestingUtils;
using TypeGen.Core.TypeAnnotations;
using Xunit;
using Gen = TypeGen.Core.Generator;


namespace TypeGen.AcceptanceTest.Issues
{
    public class CircularGenericConstraint
    {

        [ExportTsClass]
        class RecursiveConstraintClass<TSelf> 
            where TSelf : RecursiveConstraintClass<TSelf>
        {
        }

        [ExportTsInterface]
        interface IRecursiveConstraintInterface<TSelf>
            where TSelf : IRecursiveConstraintInterface<TSelf>
        {

        }

        [ExportTsInterface]
        interface IRecursiveConstraintInterfaceWithClassConstraint<TSelf>
            where TSelf : class, IRecursiveConstraintInterfaceWithClassConstraint<TSelf>, new()
        {

        }

        [ExportTsInterface]
        interface ICicrularConstraintInterface<TSelf, TOther>
            where TSelf : ICicrularConstraintInterface<TSelf, TOther>
            where TOther : ICicrularConstraintInterface<TOther, TSelf>
        {

        }



        interface IA
        {

        }

        interface IB
        {

        }

        [ExportTsInterface]
        interface IMultipleConstraintInterface<T>
            where T : IA, IB
        {

        }

        /// <summary>
        /// Looks into generating classes and interfaces with circular type constraints
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [Theory]
        [InlineData(typeof(RecursiveConstraintClass<>), @"TypeGen.AcceptanceTest.Issues.CircularGenericConstraint.Expected.RecursiveConstraintClass.ts")]
        [InlineData(typeof(IRecursiveConstraintInterface<>), @"TypeGen.AcceptanceTest.Issues.CircularGenericConstraint.Expected.IRecursiveConstraintInterface.ts")]
        [InlineData(typeof(IRecursiveConstraintInterfaceWithClassConstraint<>), @"TypeGen.AcceptanceTest.Issues.CircularGenericConstraint.Expected.IRecursiveConstraintInterfaceWithClassConstraint.ts")]
        [InlineData(typeof(IMultipleConstraintInterface<>), @"TypeGen.AcceptanceTest.Issues.CircularGenericConstraint.Expected.IMultipleConstraintInterface.ts")]
        [InlineData(typeof(ICicrularConstraintInterface<,>), @"TypeGen.AcceptanceTest.Issues.CircularGenericConstraint.Expected.ICicrularConstraintInterface.ts")]
        public async Task GeneratesCorrectly(Type type, string expectedLocation)
        {

            var readExpectedTask = EmbededResourceReader.GetEmbeddedResourceAsync(expectedLocation);

            var generator = new Gen.Generator();
            var interceptor = GeneratorOutputInterceptor.CreateInterceptor(generator);

            await generator.GenerateAsync(type);
            var expected = (await readExpectedTask).Trim();

            Assert.True(interceptor.GeneratedOutputs.ContainsKey(type));
            Assert.Equal(expected, interceptor.GeneratedOutputs[type].Content.Trim());


        }
    }
}
