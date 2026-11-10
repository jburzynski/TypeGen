using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
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

    public Application(ILogger logger, IPresenter presenter)
    {
        _logger = logger;
        _presenter = presenter;
    }

    public async Task<ExitCode> Run(string[] args)
    {
        var rootCommand = GetRootCommand();
        var parseResult = rootCommand.Parse(args);

        if (parseResult.Errors.Count > 0)
        {
            foreach (var error in parseResult.Errors)
                _logger.Log(error.Message, LogLevel.Error);

            return ExitCode.Error;
        }

        return (ExitCode)await parseResult.InvokeAsync();
    }

    private RootCommand GetRootCommand()
    {
        var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
        var versionOutput = $"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}";

        var rootCommand = new RootCommand
        {
            Description = $"TypeGen {versionOutput}"
        };

        rootCommand.SetAction((ParseResult parseResult) =>
        {
            return rootCommand.Parse("--help").InvokeAsync();
        });

        var generateCommand = new Command("generate", "Generate TypeScript files.");

        var verboseOption = new Option<bool>("--verbose", "-v")
        {
            Description = "Show verbose output.",
            DefaultValueFactory = x => false
        };

        var projectFolderOption = new Option<List<string>>(name: "--project-folder", "-p")
        {
            Description = "The project folder path(s)."
        };

        var configPathOption = new Option<List<string>>(name: "--config-path", "-c")
        {
            Description = "The config file path(s)."
        };

        var outputFolderOption = new Option<string>(name: "--output-folder", "-o")
        {
            Description = "Project's output folder."
        };
        
        generateCommand.Options.Add(verboseOption);
        generateCommand.Options.Add(projectFolderOption);
        generateCommand.Options.Add(configPathOption);
        generateCommand.Options.Add(outputFolderOption);

        generateCommand.SetAction(parseResult =>
            {
                var verbose = parseResult.GetValue(verboseOption);
                var projectFolderPaths = parseResult.GetValue(projectFolderOption);
                var configPaths = parseResult.GetValue(configPathOption);
                var outputFolder = parseResult.GetValue(outputFolderOption);

                var exitCode = ExecuteHandler(() => GetExitCodeFromActionResult(_presenter.Generate(verbose, projectFolderPaths, configPaths, outputFolder)));
                return Task.FromResult((int)exitCode);
            });
            
        var getCwdCommand = new Command("getcwd", "Get current working directory.");
        getCwdCommand.SetAction(parseResult =>
        {
            var exitCode = ExecuteHandler(() => GetExitCodeFromActionResult(_presenter.GetCwd()));
            return Task.FromResult((int)exitCode);
        });
        
        rootCommand.Subcommands.Add(generateCommand);
        rootCommand.Subcommands.Add(getCwdCommand);

        return rootCommand;
    }

    private static ExitCode GetExitCodeFromActionResult(ActionResult actionResult) =>
        actionResult.IsSuccess ? ExitCode.Success : ExitCode.Error;

    private ExitCode ExecuteHandler(Func<ExitCode> handler)
    {
        try
        {
            return handler();
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
            LogExceptions(e);
            return ExitCode.Error;
        }
    }

    private void LogExceptions(Exception e)
    {
        while (e != null)
        {
            _logger.Log($"{e.Message}{Environment.NewLine}{e.StackTrace}", LogLevel.Error);
            e = e.InnerException;
        }
    }
}
