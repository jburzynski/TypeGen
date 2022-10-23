using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeGen.Core;

namespace TypeGen.Cli.Extensions
{
    internal static class FlagExtensions
    {
        public static string ToFlagString(this StrictNullTypeUnionFlags strictNullTypeUnionFlags)
        {
            var result = "";

            if (strictNullTypeUnionFlags.HasFlag(StrictNullTypeUnionFlags.Null)) result += "null ";
            if (strictNullTypeUnionFlags.HasFlag(StrictNullTypeUnionFlags.Undefined)) result += "undefined ";

            return result.TrimEnd().Replace(" ", "|");
        }

        public static StrictNullTypeUnionFlags ToStrictNullFlags(this string str)
        {
            var result = StrictNullTypeUnionFlags.None;
            if (string.IsNullOrWhiteSpace(str)) return result;

            string[] parts = str.Split('|');

            if (parts.Contains("null")) result |= StrictNullTypeUnionFlags.Null;
            if (parts.Contains("undefined")) result |= StrictNullTypeUnionFlags.Undefined;
            if (parts.Contains("optional")) result |= StrictNullTypeUnionFlags.Optional;

            return result;
        }
    }
}
