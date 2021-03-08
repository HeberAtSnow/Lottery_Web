using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FrontEnd.Pages
{
    public class HistoricalStatsModel : PageModel
    {
        private readonly LotteryStatistics LotteryStatistics;
        private readonly ILogger<HistoricalStatsModel> logger;

        public IEnumerable<TicketSale> Sales { get; private set; }
        
        public HistoricalStatsModel(LotteryStatistics lotteryStatistics, ILogger<HistoricalStatsModel> logger)
        {
            LotteryStatistics = lotteryStatistics;
            this.logger = logger;
        }

        public void OnGet()
        {
            var stopwatch = new Stopwatch();
            logger.LogDebug("{File}: Historical stats page was loaded", "[ACC]");

            stopwatch.Start();
            try
            {
                Sales = LotteryStatistics.DBStatsAllPeriods();
                stopwatch.Stop();

                logger.LogDebug("{File}: Historical stats were loaded. Elapsed time: {Time}", "[PRF]", stopwatch.ElapsedMilliseconds);
            }
            catch
            {
                logger.LogError("{File}: Failed to retreive historical stats", "[ACC]");
            }
            finally
            {
                Sales = new List<TicketSale>();
            }
        }
    }
}
