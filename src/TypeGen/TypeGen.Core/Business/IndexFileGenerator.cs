using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Business
{
    internal class IndexFileGenerator : IIndexFileGenerator
    {
        public GeneratorOptions GeneratorOptions => _generatorOptionsProvider.GeneratorOptions;

        private readonly ITemplateService _templateService;
        private readonly IGeneratorOptionsProvider _generatorOptionsProvider;
        private readonly IFileContentGeneratedEventHandlerProvider _fileContentHandlerProvider;

        public IndexFileGenerator(ITemplateService templateService, IGeneratorOptionsProvider generatorOptionsProvider, IFileContentGeneratedEventHandlerProvider fileContentHandlerProvider)
        {
            _templateService = templateService;
            _generatorOptionsProvider = generatorOptionsProvider;
            _fileContentHandlerProvider = fileContentHandlerProvider;
        }

        /// <summary>
        /// Generates an `index.ts` file(s) which exports types within the generated files
        /// </summary>
        /// <param name="generatedFiles"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        public IEnumerable<string> Generate(IEnumerable<string> generatedFiles)
        {
            var indexFileExtension = string.IsNullOrEmpty(GeneratorOptions.IndexFileExtension) ? 
                "" : 
                "." + GeneratorOptions.IndexFileExtension;

            var fileContent = CreateIndexFileContent(generatedFiles, indexFileExtension);

            string filename = "index" + indexFileExtension;
            _fileContentHandlerProvider.FileContentGenerated?.Invoke(this, new FileContentGeneratedArgs(null, Path.Combine(GeneratorOptions.BaseOutputDirectory, filename), fileContent));

            return new[] { filename };
        }

        private string CreateIndexFileContent(IEnumerable<string> generatedFiles, string extension)
        {
            string exports = generatedFiles.Aggregate("", (prevExports, file) =>
            {
                string fileNameWithoutExt = file.Remove(file.Length - extension.Length).Replace("\\", "/");
                return prevExports + _templateService.FillIndexExportTemplate(fileNameWithoutExt);
            });
            return _templateService.FillIndexTemplate(exports);
        }
    }
}
