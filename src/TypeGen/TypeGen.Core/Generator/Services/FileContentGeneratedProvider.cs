using System;

namespace TypeGen.Core.Generator.Services
{
    internal class FileContentGeneratedEventHandlerProvider : IFileContentGeneratedEventHandlerProvider
    {

        public EventHandler<FileContentGeneratedArgs> FileContentGenerated { get; set; }
    }
}
