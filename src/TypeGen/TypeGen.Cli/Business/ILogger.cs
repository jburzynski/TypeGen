namespace TypeGen.Cli.Business
{
    internal interface ILogger
    {
        void Log(params string[] messageLines);
    }
}