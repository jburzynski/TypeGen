using LiteDB;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities
{
    [ExportTsClass]
    public class LiteDbEntity
    {
        [TsIgnore]
        public BsonArray MyBsonArray { get; set; }

        public string TestString { get; set; }
    }
}
