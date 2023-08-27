namespace TypeGen.Core.Logging
{
    public interface ILogger
    {
        LogLevel MinLevel { get; set; }
        void Log(string message, LogLevel level);
    }
}