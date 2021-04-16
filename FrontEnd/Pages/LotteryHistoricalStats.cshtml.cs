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
        private readonly ILogger<LotteryHistoricalStatsModel> _logger;

        public IEnumerable<TicketSale> Sales { get; private set; }
        public LotteryHistoricalStatsModel(LotteryStatistics lotteryStatistics, ILogger<LotteryHistoricalStatsModel> logger)
        {
            _logger = logger;
            lotteryStats = lotteryStatistics;
        }
        public IActionResult OnGet()
        {

            try
            {
                Sales = lotteryStats.DBStatsAllPeriods();
                _logger.LogDebug("{prefix}: Historical stats were successfully loaded at {time}", LogPrefix.WebTraffic, DateTime.Now);
                return Page();
            }
            catch(Exception ex)
            {
                _logger.LogWarning("{prefix}: Failed to retreive historical stats for all lottery periods on {dow}, {time} with the following exception:\n{ex}",
                    LogPrefix.WebTraffic, DateTime.Now.ToString("ddd"), DateTime.Now, ex);
                return RedirectToPage("./Error");
            }
        }
    }
}
