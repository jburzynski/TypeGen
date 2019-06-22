using System;

namespace TypeGen.Core.Generator.Services
{
    internal interface IFileContentGeneratedEventHandlerProvider
    {
        EventHandler<FileContentGeneratedArgs> FileContentGenerated { get; }
    }
}
