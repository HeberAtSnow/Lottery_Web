using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.Models
{
    public static class LoggerStuff
    {
        public static void LogDebug(ILogger logger, int minLogLevel, string message)
        {
            if (minLogLevel <= (int)LogLevel.Debug)
            {
                logger.LogDebug(message);
            }
        }

        public static void LogInformation(ILogger logger, int minLogLevel, string message)
        {
            if (minLogLevel <= (int)LogLevel.Information)
            {
                logger.LogInformation(message);
            }
        }

        public static void LogWarning(ILogger logger, int minLogLevel, string message, Exception ex = null)
        {
            if (minLogLevel <= (int)LogLevel.Warning)
            {
                logger.LogWarning(ex, message);
            }
        }

        public static void LogError(ILogger logger, int minLogLevel, string message, Exception ex = null)
        {
            if (minLogLevel <= (int)LogLevel.Error)
            {
                logger.LogError(ex, message);
            }
        }
    }
}
