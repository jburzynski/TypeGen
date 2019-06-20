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
        public IndexFileGenerator() { }

        /// <summary>
        /// Generates an `index.ts` file(s) which exports types within the generated files
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>Generated TypeScript file paths (relative to the Options.BaseOutputDirectory)</returns>
        public IEnumerable<string> Generate(IIndexFileGeneratorParams parameters)
        {
            var indexFileExtension = string.IsNullOrEmpty(parameters.GeneratorOptions.IndexFileExtension) ? 
                "" : 
                "." + parameters.GeneratorOptions.IndexFileExtension;

            string exports = parameters.GeneratedFiles.Aggregate("", (prevExports, file) =>
            {
                string fileNameWithoutExt = file.Remove(file.Length - indexFileExtension.Length).Replace("\\", "/");
                return prevExports + parameters.FillIndexExportTemplate(fileNameWithoutExt);
            });
            var fileContent = parameters.FillIndexTemplate(exports);

            string filename = "index" + indexFileExtension;
            parameters?.FileContentGenerated?.Invoke(this, new FileContentGeneratedArgs(null, Path.Combine(parameters.GeneratorOptions.BaseOutputDirectory, filename), fileContent));

            return new[] { filename };
        }
    }
}
