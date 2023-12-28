using System;

namespace TypeGen.Core.Generator;

public record TsMemberInfo(string DotNetMemberName, Type DotNetMemberType, string TsMemberName, string TsMemberType);