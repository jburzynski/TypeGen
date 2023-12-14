using System.Threading.Tasks;

namespace TypeGen.Cli;

internal interface IApplication
{
    Task<ExitCode> Run(string[] args);
}