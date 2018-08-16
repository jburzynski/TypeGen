using System.Collections.Generic;

namespace TypeGen.Cli.Business
{
    internal interface IAssemblyResolver
    {
        IEnumerable<string> Directories { get; set; }
        void Register();
        void Unregister();
    }
}