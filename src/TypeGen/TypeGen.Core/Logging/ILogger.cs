namespace TypeGen.Core.Logging
{
    public interface ILogger
    {
        bool LogVerbose { get; set; }
        
        void Log(params string[] messageLines);
    }
}