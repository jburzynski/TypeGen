﻿using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities.Structs
{
    [ExportTsClass]
    [TsCustomBase("AcmeCustomBase<string>")]
    public struct CustomBaseClass
    {
        public string SomeProperty { get; set; }
    }
}
