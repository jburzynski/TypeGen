using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core.Generator;
using TypeGen.FileContentTest.TestingUtils;
using Xunit.Abstractions;
using Xunit;

namespace TypeGen.FileContentTest;

public class PerformanceTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public PerformanceTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void test_performance()
    {
        // arrange
        var assembly = GetType().Assembly;
        var generator = new Generator();
        var interceptor = GeneratorOutputInterceptor.CreateInterceptor(generator);
        var sw = new Stopwatch();

        // act
        sw.Start();
        generator.Generate(assembly);
        sw.Stop();

        // assert
        var typeCount = interceptor.GeneratedOutputs.Count;
        var totalMs = (double)sw.ElapsedTicks / TimeSpan.TicksPerMillisecond;
        var avgMsPerType = totalMs / typeCount;

        _testOutputHelper.WriteLine($"Number of types: {typeCount}");
        _testOutputHelper.WriteLine($"Total elapsed [ms]: {totalMs}");
        _testOutputHelper.WriteLine($"Average per type [ms]: {avgMsPerType}");
    }
}
