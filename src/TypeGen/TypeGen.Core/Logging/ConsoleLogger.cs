using System;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Logging
{
    /// <summary>
    /// Logs messages to the Console
    /// </summary>
    internal class ConsoleLogger : ILogger
    {
        private readonly bool _verbose;

        public ConsoleLogger(bool verbose)
        {
            _verbose = verbose;
        }

        /// <summary>
        /// Logs messages
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public void Log(string message, LogLevel level)
        {
            Requires.NotNullOrEmpty(message, nameof(message));

            LogLevel minLevel = _verbose ? LogLevel.Debug : LogLevel.Info;

            if (level < minLevel) return;

            ConsoleColor oldColor = Console.ForegroundColor;
            
            switch (level)
            {
                case LogLevel.Warning:
                    message = "WARNING: " + message;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
            
            Console.WriteLine(message);

            Console.ForegroundColor = oldColor;
        }
    }
}
