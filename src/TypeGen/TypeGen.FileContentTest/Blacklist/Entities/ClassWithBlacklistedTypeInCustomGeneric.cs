using System;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.Blacklist.Entities;

[ExportTsClass]
public class ClassWithBlacklistedTypeInCustomGeneric
{
    public CustomGeneric<string, CustomGeneric<int, Baz, object>, DateTime> BazProp { get; set; }
}