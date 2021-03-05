using Serilog;
using Serilog.Core;

namespace FrontEnd.Services
{
    public interface IPerformanceLogger
    {
        ILogger Log { get; }
    }

    public class PerformanceLogger : IPerformanceLogger
    {
        private readonly Logger _logger;

        public ILogger Log { get { return _logger; } }

        public PerformanceLogger(Logger logger)
        {
            _logger = logger;
        }
    }

}
