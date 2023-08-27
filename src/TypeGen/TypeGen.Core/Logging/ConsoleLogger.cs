using System;
using TypeGen.Core.Validation;
using static TypeGen.Core.Utils.ConsoleUtils;

namespace TypeGen.Core.Logging
{
    /// <summary>
    /// Logs messages to the Console
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        public LogLevel MinLevel { get; set; }

        public ConsoleLogger(LogLevel minLevel = LogLevel.Info)
        {
            MinLevel = minLevel;
        }

        /// <summary>
        /// Logs messages
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public void Log(string message, LogLevel level)
        {
            Requires.NotNullOrEmpty(message, nameof(message));

            if (level < MinLevel) return;

            var color = level switch
            {
                LogLevel.Warning => ConsoleColor.Yellow,
                LogLevel.Error => ConsoleColor.Red,
                _ => Console.ForegroundColor
            };
            
            WithColor(color, () => Console.WriteLine(message));
        }
    }
}
