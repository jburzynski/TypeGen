using System.Collections.Generic;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.Blacklist.Entities;

[ExportTsClass]
public class ClassWithBlacklistedTypeInDictionary
{
    public IDictionary<string, Baz> BazProp { get; set; }
}