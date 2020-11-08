using System;
using System.Collections.Generic;
using System.Text;
using Gen = TypeGen.Core.Generator;

namespace TypeGen.Core.Test.GeneratorTestingUtils
{
    /// <summary>
    /// Intercepts the output produced by the <see cref="Gen.Generator"/>
    /// so it can be evaluated in tests
    /// </summary>
    public class GeneratorOutputInterceptor
    {

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="disableDefaultOutput"></param>
        /// <returns></returns>
        public static GeneratorOutputInterceptor CreateInterceptor(Gen.Generator generator, bool disableDefaultOutput = true)
        {
            var interceptor = new GeneratorOutputInterceptor();
            if (disableDefaultOutput)
                generator.UnsubscribeDefaultFileContentGeneratedHandler();

            generator.FileContentGenerated += interceptor.OnOutputCreated;
            return interceptor;

        }

        private GeneratorOutputInterceptor()
        {
        }

        #endregion

        #region Properties


        public IReadOnlyDictionary<Type, GeneratedOutput> GeneratedOutputs => _GeneratedOutputs;

        private Dictionary<Type, GeneratedOutput> _GeneratedOutputs { get; set; } = new Dictionary<Type, GeneratedOutput>();

        #endregion

        #region Generator interception

        private void OnOutputCreated(object sender, Gen.FileContentGeneratedArgs e)
        {
            _GeneratedOutputs.Add(e.Type, new GeneratedOutput(e));
        }

        #endregion
    }
}
