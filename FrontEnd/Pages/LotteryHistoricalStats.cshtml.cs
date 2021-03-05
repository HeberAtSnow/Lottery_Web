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
    public class LotteryHistoricalStatsModel : PageModel
    {
        private readonly LotteryStatistics lotteryStats;
        private readonly ILogger<IndexModel> _logger;

        public IEnumerable<TicketSale> Sales { get; private set; }
        public LotteryHistoricalStatsModel(LotteryStatistics lotteryStatistics, ILogger<IndexModel> logger)
        {
            _logger = logger;
            lotteryStats = lotteryStatistics;
        }
        public void OnGet() {

            var elapsedTime = new Stopwatch();
            _logger.LogInformation("Historical stats page loaded");



            elapsedTime.Start();
            try
            {
                Sales = lotteryStats.DBStatsAllPeriods();
                elapsedTime.Stop();



                _logger.LogInformation($"Historical stats were loaded in {elapsedTime.ElapsedMilliseconds}");
            }
            catch
            {
                _logger.LogError("Failed to retreive historical stats");
            }
            finally
            {
                Sales = new List<TicketSale>();
            }
        }
    }
}
