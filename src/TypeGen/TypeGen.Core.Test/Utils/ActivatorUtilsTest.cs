using System;
using TypeGen.Core.Utils;
using Xunit;

namespace TypeGen.Core.Test.Utils
{
    public class ActivatorUtilsTest
    {
        private class NonGenericClass
        {
        }
        
        private class GenericClassWithNoConstraints<T1, T2, T3>
        {
        }
        
        private class GenericClassWithDefaultConstructorConstraint<T> where T : new()
        {
        }
        
        private class GenericClassWithClassConstraint<T> where T : class
        {
        }
        
        private class GenericClassWithStructConstraint<T> where T : struct
        {
        }

        private class GenericClassWithClassAndStructConstraints<T1, T2, T3>
            where T1 : class
            where T2 : struct
        {
        }

        private class BaseClass
        {
        }

        private interface Interface
        {
        }

        private class GenericClassWithBaseClassConstraint<T> where T : BaseClass
        {
        }
        
        private class GenericClassWithInterfaceConstraint<T> where T : Interface
        {
        }
        
        [Fact]
        public void CreateInstanceAutoFillGenericParameters_NonGenericTypePassed_InstanceCreated()
        {
            object instance = ActivatorUtils.CreateInstanceAutoFillGenericParameters(typeof(NonGenericClass));
            
            Assert.True(instance is NonGenericClass);
        }
        
        [Fact]
        public void CreateInstanceAutoFillGenericParameters_GenericTypeWithNoConstraintsPassed_InstanceCreated()
        {
            object instance = ActivatorUtils.CreateInstanceAutoFillGenericParameters(typeof(GenericClassWithNoConstraints<,,>));
            
            Assert.True(instance is GenericClassWithNoConstraints<object, object, object>);
        }
        
        [Fact]
        public void CreateInstanceAutoFillGenericParameters_GenericTypeWithDefaultConstructorConstraintPassed_InstanceCreated()
        {
            object instance = ActivatorUtils.CreateInstanceAutoFillGenericParameters(typeof(GenericClassWithDefaultConstructorConstraint<>));
            
            Assert.True(instance is GenericClassWithDefaultConstructorConstraint<object>);
        }
        
        [Fact]
        public void CreateInstanceAutoFillGenericParameters_GenericTypeWithClassConstraintsPassed_InstanceCreated()
        {
            object instance = ActivatorUtils.CreateInstanceAutoFillGenericParameters(typeof(GenericClassWithClassConstraint<>));
            
            Assert.True(instance is GenericClassWithClassConstraint<object>);
        }
        
        [Fact]
        public void CreateInstanceAutoFillGenericParameters_GenericTypeWithStructConstraintsPassed_InstanceCreated()
        {
            object instance = ActivatorUtils.CreateInstanceAutoFillGenericParameters(typeof(GenericClassWithStructConstraint<>));
            
            Assert.True(instance is GenericClassWithStructConstraint<int>);
        }
        
        [Fact]
        public void CreateInstanceAutoFillGenericParameters_GenericTypeWithClassAndStructConstraintsPassed_InstanceCreated()
        {
            object instance = ActivatorUtils.CreateInstanceAutoFillGenericParameters(typeof(GenericClassWithClassAndStructConstraints<,,>));
            
            Assert.True(instance is GenericClassWithClassAndStructConstraints<object, int, object>);
        }
        
        [Fact]
        public void CreateInstanceAutoFillGenericParameters_GenericTypeWithBaseClassConstraintPassed_ExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                ActivatorUtils.CreateInstanceAutoFillGenericParameters(typeof(GenericClassWithBaseClassConstraint<>));
            });
        }
        
        [Fact]
        public void CreateInstanceAutoFillGenericParameters_GenericTypeWithInterfaceConstraintPassed_ExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                ActivatorUtils.CreateInstanceAutoFillGenericParameters(typeof(GenericClassWithInterfaceConstraint<>));
            });
        }
    }
}