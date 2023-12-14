using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.Storage;
using Xunit;

namespace TypeGen.Core.Test.Generator;

public class GeneratorTest
{
    [Fact]
    public async Task generation_callbacks_should_be_invoked_in_correct_order()
    {
        // arrange
        var options = new GeneratorOptions();
        var fileSystemMock = Substitute.For<IFileSystem>();
        var generator = new Core.Generator.Generator(options, fileSystemMock);
        var generationSpec = new GenerationCallbackOrderSpec();

        // act
        await generator.GenerateAsync(generationSpec);

        // assert
        generationSpec.CallbackOrder.Should().BeEquivalentTo(new[]
        {
            nameof(GenerationSpec.OnBeforeGeneration),
            nameof(GenerationSpec.OnBeforeBarrelGeneration),
            nameof(GenerationSpec.OnAfterGeneration),
        });
    }

    private class GenerationCallbackOrderSpec : GenerationSpec
    {
        public List<string> CallbackOrder { get; } = new();
        
        public GenerationCallbackOrderSpec()
        {
            AddClass<Foo>();
        }

        public override void OnBeforeGeneration(OnBeforeGenerationArgs args)
        {
            base.OnBeforeGeneration(args);
            CallbackOrder.Add(nameof(OnBeforeGeneration));
            Console.WriteLine(nameof(OnBeforeGeneration));
        }

        public override void OnBeforeBarrelGeneration(OnBeforeBarrelGenerationArgs args)
        {
            base.OnBeforeBarrelGeneration(args);
            CallbackOrder.Add(nameof(OnBeforeBarrelGeneration));
            Console.WriteLine(nameof(OnBeforeBarrelGeneration));
        }

        public override void OnAfterGeneration(OnAfterGenerationArgs args)
        {
            base.OnAfterGeneration(args);
            CallbackOrder.Add(nameof(OnAfterGeneration));
            Console.WriteLine(nameof(OnAfterGeneration));
        }

        private class Foo
        {
            public string HelloWorld { get; set; }
        }
    }
}