﻿using System;

namespace TypeGen.Core.TypeAnnotations
{
    /// <summary>
    /// Identifies a nullable TypeScript property (used only with enabled strict null checking mode)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TsNullAttribute : Attribute
    {
    }
}
