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
        private readonly ILogger<LotteryHistoricalStatsModel> logger;

        public IEnumerable<TicketSale> Sales { get; private set; }

        public LotteryHistoricalStatsModel(LotteryStatistics lotteryStatistics, ILogger<LotteryHistoricalStatsModel> logger)
        {
            lotteryStats = lotteryStatistics;
            this.logger = logger;
        }

        public IActionResult OnGet()
        {
            try
            {
                Sales = lotteryStats.DBStatsAllPeriods();
                logger.LogDebug("[{prefix}]: Successfully retrieved the historical statistics for all lottery periods.",
                    LogPrefix.Stats);

                return Page();
            }
            catch (Exception ex)
            {
                logger.LogWarning("[{prefix}]: Failed to retrieve the historical statistics for all lottery periods. Reason: {ex}.",
                    LogPrefix.Stats, ex);

                return RedirectToPage("./Error");
            }
        }
    }
}
