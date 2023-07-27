using LiteDB;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities.Structs
{
    [ExportTsClass]
    public struct LiteDbEntity
    {
        [TsIgnore]
        public BsonArray MyBsonArray { get; set; }

        public string TestString { get; set; }
    }
}
