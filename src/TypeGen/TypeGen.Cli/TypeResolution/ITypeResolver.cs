using System;
using System.Collections.Generic;

namespace TypeGen.Cli.TypeResolution;

internal interface ITypeResolver
{
    Type Resolve(string typeIdentifier, string typeNameSuffix = null, IEnumerable<Type> interfaceConstraints = null, IEnumerable<Type> baseTypeConstraints = null);
}