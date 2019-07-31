namespace TypeGen.Core.Logging
{
    internal interface ILogger
    {
        void Log(string message, LogLevel level);
    }
}