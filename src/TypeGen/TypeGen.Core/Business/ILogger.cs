namespace TypeGen.Core.Business
{
    public interface ILogger
    {
        bool LogVerbose { get; set; }
        
        void Log(params string[] messageLines);
    }
}