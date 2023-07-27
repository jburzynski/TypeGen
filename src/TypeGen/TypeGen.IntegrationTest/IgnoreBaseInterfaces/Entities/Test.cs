using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.IgnoreBaseInterfaces.Entities;

[ExportTsClass]
[TsIgnoreBase]
public class Test : ITest
{
    public string Name { get; set; }
}