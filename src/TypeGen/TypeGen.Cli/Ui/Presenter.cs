using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeGen.Cli.Build;
using TypeGen.Cli.GenerationConfig;
using TypeGen.Cli.ProjectFileManagement;
using TypeGen.Cli.TypeResolution;
using TypeGen.Cli.Ui.Validation;
using TypeGen.Core.Extensions;
using TypeGen.Core.Generator;
using TypeGen.Core.Logging;
using TypeGen.Core.Storage;

namespace TypeGen.Cli.Ui;

internal class Presenter : IPresenter
{
    private readonly ILogger _logger;
    private readonly IConsoleOutput _consoleOutput;
    private readonly IFileSystem _fileSystem;
    private readonly IConfigProvider _configProvider;
    private readonly IProjectBuild _projectBuild;
    private readonly IProjectFileManager _projectFileManager;
    
    private readonly IGenerateValidator _generateValidator;

    public Presenter(
        IConsoleOutput consoleOutput,
        IFileSystem fileSystem,
        ILogger logger,
        IConfigProvider configProvider,
        IProjectBuild projectBuild,
        IProjectFileManager projectFileManager,
        IGenerateValidator generateValidator)
    {
        _consoleOutput = consoleOutput;
        _fileSystem = fileSystem;
        _logger = logger;
        _configProvider = configProvider;
        _projectBuild = projectBuild;
        _projectFileManager = projectFileManager;
        _generateValidator = generateValidator;
    }

    public ActionResult GetCwd()
    {
        _consoleOutput.ShowCwd();
        return ActionResult.Success();
    }

    public ActionResult Generate(bool verbose, IReadOnlyCollection<string> projectFolderPaths, IReadOnlyCollection<string> configPaths, string outputFolder)
    {
        if (projectFolderPaths.IsNullOrEmpty()) projectFolderPaths = new[] { "." };
        var validationResult = _generateValidator.Validate(verbose, projectFolderPaths, configPaths, outputFolder);
        
        validationResult
            .Match(
                () => GeneratePrivate(verbose, projectFolderPaths, configPaths, outputFolder),
                messages => _consoleOutput.ShowErrors(messages)
                );

        return validationResult.IsSuccess ? ActionResult.Success() : ActionResult.Failure();
    }

    private void GeneratePrivate(bool verbose, IReadOnlyCollection<string> projectFolderPaths, IReadOnlyCollection<string> configPaths, string outputFolder)
    {
        SetLoggerVerbosity(verbose);
        
        if (configPaths.None()) configPaths = Enumerable.Repeat((string)null, projectFolderPaths.Count).ToList();
        var projectConfigPairs = projectFolderPaths
            .Zip(configPaths)
            .Select(x => (projectFolderPath: x.First, configPath: x.Second));
        
        foreach (var pair in projectConfigPairs)
            GenerateSingle(pair.projectFolderPath, pair.configPath, outputFolder);
    }

    private void GenerateSingle(string projectFolder, string configPath, string outputFolder)
    {
        // prep
        
        var configConsoleOptions = new ConfigConsoleOptions(outputFolder);
        var config = _configProvider.GetConfig(configPath, projectFolder, configConsoleOptions);
        var assemblies = config.GetAssemblies().Select(Assembly.LoadFrom).ToList();
        
        using var assemblyResolver = new AssemblyResolver(_fileSystem, _logger, projectFolder, config.ExternalAssemblyPaths);
        var typeResolver = new TypeResolver(_logger, _fileSystem, projectFolder, assemblies);
        var converterResolver = new ConverterResolver(typeResolver);
        var generationSpecResolver = new GenerationSpecResolver(typeResolver);
        
        var generatorOptionsProvider = new GeneratorOptionsProvider(converterResolver);
        var generatorOptions = generatorOptionsProvider.GetGeneratorOptions(config, assemblies, projectFolder);
        var generator = new Generator(generatorOptions, _logger);
        
        // generate
        
        if (config.ClearOutputDirectory == true) _fileSystem.ClearDirectory(generatorOptions.BaseOutputDirectory);
        if (config.BuildProject == true) _projectBuild.Build(projectFolder);

        _consoleOutput.ShowGenerationBegin(projectFolder);
        
        var generatedFiles = new List<string>();

        if (config.GenerationSpecs.None() || config.GenerateFromAssemblies == true)
            generatedFiles.AddRange(generator.Generate(assemblies));
        
        if (config.GenerationSpecs.Any())
        {
            var generationSpecs = generationSpecResolver.Resolve(config.GenerationSpecs);
            generatedFiles.AddRange(generator.Generate(generationSpecs));
        }

        if (config.AddFilesToProject == true) AddFilesToProject(projectFolder, generatedFiles);
        
        _consoleOutput.ShowGeneratedFiles(generatedFiles);
        _consoleOutput.ShowGenerationEnd(projectFolder);
    }

    private void AddFilesToProject(string projectFolder, IEnumerable<string> generatedFiles)
    {
        var projectDocument = _projectFileManager.ReadFromProjectFolder(projectFolder);
        _projectFileManager.AddTsFiles(projectDocument, generatedFiles);
        _projectFileManager.SaveProjectFile(projectFolder, projectDocument);
    }

    private void SetLoggerVerbosity(bool verbose)
    {
        _logger.MinLevel = verbose ? LogLevel.Debug : LogLevel.Info;
    }
}