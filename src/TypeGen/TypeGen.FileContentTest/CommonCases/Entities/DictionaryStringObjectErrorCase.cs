using System.Collections.Generic;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities;

[ExportTsClass]
public class DictionaryStringObjectErrorCase
{
    public Dictionary<string, object> Foo { get; set; }
}