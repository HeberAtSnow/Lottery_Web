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
        private readonly ILogger logger;

        public IEnumerable<TicketSale> Sales { get; private set; }
        
        public HistoricalStatsModel(LotteryStatistics lotteryStatistics, ILogger logger)
        {
            LotteryStatistics = lotteryStatistics;
            this.logger = logger;
        }

        public void OnGet()
        {
            var stopwatch = new Stopwatch();
            logger.LogDebug("Historical stats page was loaded");

            stopwatch.Start();
            try
            {
                Sales = LotteryStatistics.DBStatsAllPeriods();
            }
            catch
            {
                logger.LogError("Failed to retreive historical stats");
            }
            stopwatch.Stop();

            logger.LogDebug($"Historical stats were loaded. Elapsed time: {stopwatch.ElapsedMilliseconds}");
        }
    }
}
