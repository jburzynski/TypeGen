using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities
{
    [ExportTsClass]
    public class TypeUnions
    {
        [TsTypeUnions("null", "undefined")]
        public string StringNullUndefinedProp { get; set; }
        
        [TsTypeUnions("null")]
        public int IntNullProp { get; set; }
     
        public string StringProp { get; set; }
        
        [TsNotUndefined]
        public int? IntNullableProp { get; set; }
    }
}