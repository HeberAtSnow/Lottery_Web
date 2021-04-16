using System;
using System.Collections.Generic;
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
        public IEnumerable<TicketSale> Sales { get; private set; }

        private readonly ILogger<LotteryHistoricalStatsModel> _logger;

        public string ResetErrorMessage = "";

        public LotteryHistoricalStatsModel(LotteryStatistics lotteryStatistics, ILogger<LotteryHistoricalStatsModel> logger)
        {
            _logger = logger;
            lotteryStats = lotteryStatistics;
        }
        public void OnPost()
        {
            _logger.LogInformation("Clicked button to get all lottery results");
            try
            {
                var startTime = DateTime.Now;
                Sales = lotteryStats.DBStatsAllPeriods();

                _logger.LogInformation("To get the all stats in all periods took {elapsed}", DateTime.Now - startTime);

                if (Sales == null)
                {
                    _logger.LogWarning("There is not ticket sale period history");
                }
                else
                {
                    _logger.LogInformation("website returned {count} ticker sale periods", Sales.Count());
                }
            }
            catch
            {
                ResetErrorMessage = "failed to get all periods history";
                _logger.LogWarning(ResetErrorMessage);
            }

        }
    }
}
