using System;
using System.Collections.Generic;
using System.Text;

namespace TypeGen.Core.Business
{
    internal interface IFileContentGeneratedEventHandlerProvider
    {
        EventHandler<FileContentGeneratedArgs> FileContentGenerated { get; }
    }
}
