using System.Collections.Generic;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities;

[ExportTsClass]
public class DictionaryWithEnumKey
{
    public IDictionary<EnumAsUnionType, CustomBaseClass> EnumDict { get; set; }
}