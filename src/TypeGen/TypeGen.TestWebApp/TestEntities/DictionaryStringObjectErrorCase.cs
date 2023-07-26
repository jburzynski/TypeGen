using System.Collections.Generic;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities;

[ExportTsClass]
public class DictionaryStringObjectErrorCase
{
    public Dictionary<string, object> Foo { get; set; }
}