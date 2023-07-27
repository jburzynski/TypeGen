using System;
using Gen = TypeGen.Core.Generator;

namespace TypeGen.IntegrationTest.TestingUtils
{
    public class GeneratedOutput
    {
        #region Properties

        /// <summary>
        /// The path of the generated file
        /// </summary>
        public string Path { get; set; } = "";

        /// <summary>
        /// The content of the generated file
        /// </summary>
        public string Content { get; set; } = "";

        /// <summary>
        /// The type the file was created for
        /// </summary>
        public Type GeneratedFor { get; set; }

        /// <summary>
        /// Constructs output from <see cref="Gen.FileContentGeneratedArgs"/> produced
        /// by a <see cref="Gen.Generator"/>
        /// </summary>
        /// <param name="e"></param>
        public GeneratedOutput(Gen.FileContentGeneratedArgs e)
        {
            Path = e.FilePath;
            Content = e.FileContent;
            GeneratedFor = e.Type;
        }

        #endregion
    }
}
