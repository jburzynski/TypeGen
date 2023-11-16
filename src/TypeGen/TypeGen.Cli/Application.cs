using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Parsing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TypeGen.Cli.Ui;
using TypeGen.Core;
using TypeGen.Core.Logging;

namespace TypeGen.Cli;

internal class Application : IApplication
{
    private readonly ILogger _logger;
    private readonly IPresenter _presenter;
    
    private ExitCode _exitCode = ExitCode.Success;

    public Application(ILogger logger, IPresenter presenter)
    {
        _logger = logger;
        _presenter = presenter;
    }

    public async Task<ExitCode> Run(string[] args)
    {
        try
        {
            var parser = BuildCommandLine();
            await parser.InvokeAsync(args);
            return _exitCode;
        }
        catch (AssemblyResolutionException e)
        {
            var message = e.Message +
                             "Consider adding any external assembly directories in the externalAssemblyPaths parameter. " +
                             "If you're using ASP.NET Core, add your NuGet directory to externalAssemblyPaths parameter (you can use global NuGet packages directory alias: \"<global-packages>\")";
            _logger.Log($"{message}{Environment.NewLine}{e.StackTrace}", LogLevel.Error);
            return ExitCode.Error;
        }
        catch (ReflectionTypeLoadException e)
        {
            foreach (var loaderException in e.LoaderExceptions)
            {
                _logger.Log($"Type load error: {loaderException.Message}{Environment.NewLine}{e.StackTrace}", LogLevel.Error);
            }
            return ExitCode.Error;
        }
        catch (Exception e)
        {
            _logger.Log($"{e.Message}{Environment.NewLine}{e.StackTrace}", LogLevel.Error);
            return ExitCode.Error;
        }
    }

    private Parser BuildCommandLine()
    {
        var rootCommand = new RootCommand
        {
            Name = "[dotnet-]typegen"
        };

        var generateCommand = new Command("generate", "Generate TypeScript sources");
            
        var verboseOption = new Option<bool>
            (name: "--verbose",
            description: "Show verbose output",
            getDefaultValue: () => false);
        verboseOption.AddAlias("-v");

        var projectFolderOption = new Option<List<string>>
            (name: "--project-folder",
            description: "The project folder path(s)");
        projectFolderOption.AddAlias("-p");
            
        var configPathOption = new Option<List<string>>
            (name: "--config-path",
            description: "The config file path(s)");
        configPathOption.AddAlias("-c");
        
        var outputFolderOption = new Option<string>
            (name: "--output-folder",
            description: "Project's output folder");
        configPathOption.AddAlias("-o");
            
        generateCommand.AddOption(verboseOption);
        generateCommand.AddOption(projectFolderOption);
        generateCommand.AddOption(configPathOption);
        generateCommand.AddOption(outputFolderOption);
        
        generateCommand.SetHandler((v, p, c, o) =>
            {
                _exitCode = ExitCodeFromActionResult(_presenter.Generate(v, p, c, o));
            },
            verboseOption,
            projectFolderOption,
            configPathOption,
            outputFolderOption);
            
        var getCwdCommand = new Command("getcwd", "Get current working directory");
        getCwdCommand.SetHandler(() =>
        {
            _exitCode = ExitCodeFromActionResult(_presenter.GetCwd());
        });
            
        rootCommand.AddCommand(generateCommand);
        rootCommand.AddCommand(getCwdCommand);

        return new CommandLineBuilder(rootCommand)
            .UseDefaults()
            .UseHelp(ctx => ctx.HelpBuilder.CustomizeLayout(
                _ => HelpBuilder.Default
                    .GetLayout()
                    .Skip(1)
                    .Prepend(_ =>
                    {
                        Console.WriteLine($"TypeGen v{ApplicationConfig.Version}");
                    })))
            .Build();
    }

    private static ExitCode ExitCodeFromActionResult(ActionResult actionResult) =>
        actionResult.IsSuccess ? ExitCode.Success : ExitCode.Error;
}