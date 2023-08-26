using System.Collections.Generic;

namespace TypeGen.Core.TypeModel.TypeScript;

internal class TsInterface : TsType
{
    public IReadOnlyCollection<TsImport> Imports { get; }
    public string Base { get; }
    public IReadOnlyCollection<TsProperty> Properties { get; set; }
    
    public TsInterface(string name) : base(name)
    {
    }
}