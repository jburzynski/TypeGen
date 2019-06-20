using System;
using System.Collections.Generic;
using System.Text;

namespace TypeGen.Core.Business
{
    public interface IIndexFileGenerator
    {
        IEnumerable<string> Generate(IIndexFileGeneratorParams parameters);
    }
}
