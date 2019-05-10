using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Utils
{
    internal static class ActivatorUtils
    {
        public static object CreateInstanceAutoFillGenericParameters(Type type)
        {
            Requires.NotNull(type, nameof(type));

            if (!type.GetTypeInfo().ContainsGenericParameters) return Activator.CreateInstance(type);

            Type[] genericArguments = type.GetGenericArguments();
            IList<Type> filledGenericArguments = new List<Type>();

            foreach (Type genericArgument in genericArguments)
            {
                if (!genericArgument.IsGenericParameter)
                {
                    filledGenericArguments.Add(genericArgument);
                    continue;
                }
                
                GenericParameterAttributes specialConstraints = genericArgument.GetTypeInfo().GenericParameterAttributes & GenericParameterAttributes.SpecialConstraintMask;

                // generic parameter has struct constraint
                if ((specialConstraints & GenericParameterAttributes.NotNullableValueTypeConstraint) != GenericParameterAttributes.None)
                {
                    filledGenericArguments.Add(typeof(int));
                    continue;
                }
                
                filledGenericArguments.Add(typeof(object));
            }

            Type genericType = type.MakeGenericType(filledGenericArguments.ToArray());
            return Activator.CreateInstance(genericType);
        }
    }
}