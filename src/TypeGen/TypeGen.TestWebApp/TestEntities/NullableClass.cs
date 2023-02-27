#nullable enable
using System;
using System.Collections.Generic;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    [ExportTsClass]
    public class NullableClass
    {
        public int NumberNotNullable { get; set; }
        public int? NumberNullable { get; set; }

        public string StringNotNullable { get; set; } = string.Empty;
        public string? StringNullable { get; set; }

        public Guid GuidNotNullable { get; set; }
        public Guid? GuidNullable { get; set; }

        public List<string> ListNotNullable { get; set; } = new();
        public List<string>? ListNullable { get; set; }

        public Dictionary<string, string> DictionaryNotNullable { get; set; } = new();
        public Dictionary<string, string>? DictionaryNullable { get; set; }
    }
}
