using System.Collections.Generic;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.Blacklist.Entities;

[ExportTsClass]
public class ClassWithBlacklistedTypeInArray
{
    public Baz[] BazProp { get; set; }
}