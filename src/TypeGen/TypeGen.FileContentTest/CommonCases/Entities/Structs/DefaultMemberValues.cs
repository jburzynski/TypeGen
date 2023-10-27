using System;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities.Structs
{
    [ExportTsClass]
    public struct DefaultMemberValues
    {
        public string fieldString = "fieldString";
        public static int staticFieldNumber = 2;

        public int PropertyNumber { get; set; } = 3;
        public static string StaticPropertyString { get; set; } = "StaticPropertyString";

        public int fieldIntUnassigned;
        public int fieldIntAssignedDefaultValue = 0;

        public float fieldFloatAssignedDefaultValue = 0f;

        public DateTime fieldDateTimeUnassigned;
        
        public DefaultMemberComplexValues PropertyComplexDefaultValue { get; set; } = new();

        public DefaultMemberComplexValues PropertyComplexNotDefaultValue { get; set; } = new() { Number = 4 };

        public DefaultMemberValues()
        {
            fieldIntUnassigned = 0;
            fieldDateTimeUnassigned = default;
        }
    }
}