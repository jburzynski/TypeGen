using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeGen.Core;

namespace TypeGen.Cli.Extensions
{
    internal static class FlagExtensions
    {
        public static string ToFlagString(this StrictNullFlags strictNullFlags)
        {
            var result = "";

            if (strictNullFlags.HasFlag(StrictNullFlags.Null)) result += "null ";
            if (strictNullFlags.HasFlag(StrictNullFlags.Undefined)) result += "undefined ";

            return result.TrimEnd().Replace(" ", "|");
        }

        public static StrictNullFlags ToStrictNullFlags(this string str)
        {
            var result = StrictNullFlags.Regular;
            if (string.IsNullOrWhiteSpace(str)) return result;

            string[] parts = str.Split('|');

            if (parts.Contains("null")) result |= StrictNullFlags.Null;
            if (parts.Contains("undefined")) result |= StrictNullFlags.Undefined;

            return result;
        }
    }
}
