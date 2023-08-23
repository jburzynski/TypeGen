using System.Collections.Generic;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.Blacklist.Entities;

[ExportTsClass]
public class ClassWithBlacklistedTypeInDictionary
{
    public IDictionary<string, Baz> BazProp { get; set; }
}