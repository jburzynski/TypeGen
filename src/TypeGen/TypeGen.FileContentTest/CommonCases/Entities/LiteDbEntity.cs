using LiteDB;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities
{
    [ExportTsClass]
    public class LiteDbEntity
    {
        [TsIgnore]
        public BsonArray MyBsonArray { get; set; }

        public string TestString { get; set; }
    }
}
