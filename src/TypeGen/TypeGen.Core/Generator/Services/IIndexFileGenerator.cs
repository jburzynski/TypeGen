using System.Collections.Generic;

namespace TypeGen.Core.Generator.Services
{
    public interface IIndexFileGenerator
    {
        IEnumerable<string> Generate(IIndexFileGeneratorParams parameters);
    }
}
