using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.Blacklist.Entities;

[ExportTsClass]
public class ClassWithBlacklistedBase : Bar, IFoo
{
    
}