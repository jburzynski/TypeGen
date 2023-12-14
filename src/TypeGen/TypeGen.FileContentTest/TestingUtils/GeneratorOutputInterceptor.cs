using System;
using System.Collections.Generic;
using Gen = TypeGen.Core.Generator;

namespace TypeGen.FileContentTest.TestingUtils;

/// <summary>
///     Intercepts the output produced by the <see cref="Gen.Generator" />
///     so it can be evaluated in tests
/// </summary>
public class GeneratorOutputInterceptor
{
    private GeneratorOutputInterceptor()
    {
    }

    private Dictionary<Type, GeneratedOutput> _GeneratedOutputs { get; } = new();

    public IReadOnlyDictionary<Type, GeneratedOutput> GeneratedOutputs => _GeneratedOutputs;

    /// <summary>
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

    private void OnOutputCreated(object sender, Gen.FileContentGeneratedArgs e)
    {
        _GeneratedOutputs[e.Type] = new GeneratedOutput(e);
    }
}