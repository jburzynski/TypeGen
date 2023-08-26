using System.Collections.Generic;

namespace TypeGen.Cli.TypeResolution
{
    internal interface IAssemblyResolver
    {
        IEnumerable<string> Directories { get; set; }
        void Register();
        void Unregister();
    }
}