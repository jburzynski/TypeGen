using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.Blacklist.Entities;

[ExportTsClass]
public class ClassWithBlacklistedInterface : Bar, IFoo
{
    
}