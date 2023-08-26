using System.Collections.Generic;

namespace TypeGen.Core.TypeModel.TypeScript;

internal class TsClass : TsType
{
    public IReadOnlyCollection<TsImport> Imports { get; }
    public string Base { get; }
    public IReadOnlyCollection<string> ImplementedInterfaces { get; set; }
    public IReadOnlyCollection<TsProperty> Properties { get; set; }
    
    public TsClass(string name, IReadOnlyCollection<TsImport> imports)
        : base(name)
    {
    }
}