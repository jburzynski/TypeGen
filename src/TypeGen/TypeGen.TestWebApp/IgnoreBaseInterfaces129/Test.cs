using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.IgnoreBaseInterfaces129;

[ExportTsClass]
[TsIgnoreBase]
public class Test : ITest
{
    public string Name { get; set; }
}