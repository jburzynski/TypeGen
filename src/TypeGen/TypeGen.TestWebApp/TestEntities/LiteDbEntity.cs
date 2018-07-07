using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using TypeGen.Core.TypeAnnotations;

namespace Core2WebApp.TestEntities
{
    [ExportTsClass]
    public class LiteDbEntity
    {
        [TsIgnore]
        public BsonArray MyBsonArray { get; set; }

        public string TestString { get; set; }
    }
}
