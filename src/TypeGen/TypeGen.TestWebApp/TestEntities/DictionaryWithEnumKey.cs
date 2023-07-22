using System.Collections.Generic;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities;

[ExportTsClass]
public class DictionaryWithEnumKey
{
    public IDictionary<EnumAsUnionType, CustomBaseClass> EnumDict { get; set; }
}