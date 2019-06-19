using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Business
{
    internal class IndexFileGenerator : IIndexFileGenerator, IIndexFileGeneratorInjectable
    {
        public GeneratorOptions GeneratorOptions => GeneratorOptionsProvider.GeneratorOptions;

        public ITemplateService TemplateService { get; set; }
        public IGeneratorOptionsProvider GeneratorOptionsProvider { get; set; }
        public IFileContentGeneratedEventHandlerProvider FileContentHandlerProvider { get; set; }

        public IndexFileGenerator()
        {

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
            FileContentHandlerProvider.FileContentGenerated?.Invoke(this, new FileContentGeneratedArgs(null, Path.Combine(GeneratorOptions.BaseOutputDirectory, filename), fileContent));

            return new[] { filename };
        }

        private string CreateIndexFileContent(IEnumerable<string> generatedFiles, string extension)
        {
            string exports = generatedFiles.Aggregate("", (prevExports, file) =>
            {
                string fileNameWithoutExt = file.Remove(file.Length - extension.Length).Replace("\\", "/");
                return prevExports + TemplateService.FillIndexExportTemplate(fileNameWithoutExt);
            });
            return TemplateService.FillIndexTemplate(exports);
        }
    }
}
