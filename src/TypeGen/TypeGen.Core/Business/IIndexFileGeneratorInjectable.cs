using System;
using System.Collections.Generic;
using System.Text;

namespace TypeGen.Core.Business
{
    internal interface IIndexFileGeneratorInjectable
    {
        ITemplateService TemplateService { get; set; }
        IGeneratorOptionsProvider GeneratorOptionsProvider { get; set; }
        IFileContentGeneratedEventHandlerProvider FileContentHandlerProvider { get; set; }
    }
}
