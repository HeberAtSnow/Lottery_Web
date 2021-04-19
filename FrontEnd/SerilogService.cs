using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd
{

    public interface ILoggingService
    {
        void SetLoggingLevel(string logEventLevel);
    }

    public class LoggingService : ILoggingService
    {
        public readonly LoggingLevelSwitch _loggingLevelSwitch;

        public LoggingService()
        {
            _loggingLevelSwitch = Startup.loggingLevelSwitch;
        }

        public void SetLoggingLevel(string logEventLevel)
        {
            
            if (logEventLevel == "Verbose")
                _loggingLevelSwitch.MinimumLevel = LogEventLevel.Verbose;
            if (logEventLevel == "Debug")
                _loggingLevelSwitch.MinimumLevel = LogEventLevel.Debug;
            if (logEventLevel == "Information")
                _loggingLevelSwitch.MinimumLevel = LogEventLevel.Information;
            if (logEventLevel == "Warning")
                _loggingLevelSwitch.MinimumLevel = LogEventLevel.Warning;
            if (logEventLevel == "Error")
                _loggingLevelSwitch.MinimumLevel = LogEventLevel.Error;
            if (logEventLevel == "Fatal")
                _loggingLevelSwitch.MinimumLevel = LogEventLevel.Fatal;
        }
    }
}
