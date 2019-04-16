using System;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.Constants
{
    [ExportTsClass]
    public static class FooConstants
    {
        public const string FooConstant = "Foo";
        public const string BarConstant = "Bar";
        public const double DoubleConstant = 1.234;
        public const decimal DecimalConstant = 1.234m;
        public const float FloatConstant = 1.234f;

        public static readonly string SrFooConstant = "Foo";
        public static readonly string SrBarConstant = "Bar";
        public static readonly double SrDoubleConstant = 1.234;
        public static readonly decimal SrDecimalConstant = 1.234m;
        public static readonly float SrFloatConstant = 1.234f;
    }
}
