using System.Collections.Generic;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities;

[ExportTsClass]
public class DictionaryStringObjectErrorCase
{
    public Dictionary<string, object> Foo { get; set; }
}