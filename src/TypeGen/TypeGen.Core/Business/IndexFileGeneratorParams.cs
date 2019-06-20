using System;
using System.Collections.Generic;
using System.Text;

namespace TypeGen.Core.Business
{
    internal class IndexFileGeneratorParams : IIndexFileGeneratorParams
    {
        IGeneratorOptionsProvider _optionsProvider;
        ITemplateService _templateService;
        IFileContentGeneratedEventHandlerProvider _fileContentGeneratedEventHandlerProvider;

        public IndexFileGeneratorParams(
            IEnumerable<string> generatedFiles, 
            IGeneratorOptionsProvider optionsProvider, 
            ITemplateService templateService,
            IFileContentGeneratedEventHandlerProvider fileContentGeneratedEventHandlerProvider)
        {
            GeneratedFiles = generatedFiles;
            _optionsProvider = optionsProvider;
            _templateService = templateService;
            _fileContentGeneratedEventHandlerProvider = fileContentGeneratedEventHandlerProvider;
        }

        public IEnumerable<string> GeneratedFiles { get; private set; }

        public GeneratorOptions GeneratorOptions => _optionsProvider.GeneratorOptions;

        public EventHandler<FileContentGeneratedArgs> FileContentGenerated => _fileContentGeneratedEventHandlerProvider.FileContentGenerated;

        public string FillIndexExportTemplate(string filename) => _templateService.FillIndexExportTemplate(filename);

        public string FillIndexTemplate(string exports) => _templateService.FillIndexTemplate(exports);
    }
}
