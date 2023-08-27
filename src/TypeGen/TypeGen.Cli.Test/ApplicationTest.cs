using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using TypeGen.Cli.Ui;
using TypeGen.Core.Logging;
using Xunit;

namespace TypeGen.Cli.Test;

public class ApplicationTest
{
    [Fact]
    public async Task Run_CommandIsGetCwd_CorrectPresenterMethodCalled()
    {
        // arrange
        var args = new[] { "getcwd" };
        var logger = Substitute.For<ILogger>();
        var presenter = Substitute.For<IPresenter>();
        var sut = new Application(logger, presenter);

        // act
        await sut.Run(args);

        // assert
        presenter.Received(1).GetCwd();
    }
    
    [Fact]
    public async Task Run_CommandIsGenerate_CorrectPresenterMethodCalled()
    {
        // arrange
        var args = new[] { "generate" };
        var logger = Substitute.For<ILogger>();
        var presenter = Substitute.For<IPresenter>();
        var sut = new Application(logger, presenter);

        // act
        await sut.Run(args);

        // assert
        presenter.Received(1).Generate(Arg.Any<bool>(), Arg.Any<IReadOnlyCollection<string>>(),
            Arg.Any<IReadOnlyCollection<string>>(), Arg.Any<string>());
    }
}