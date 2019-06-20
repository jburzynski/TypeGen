using System;
using System.Collections.Generic;
using System.Text;

namespace TypeGen.Core.Business
{
    internal class FileContentGeneratedEventHandlerProvider : IFileContentGeneratedEventHandlerProvider
    {

        public EventHandler<FileContentGeneratedArgs> FileContentGenerated { get; set; }
    }
}
